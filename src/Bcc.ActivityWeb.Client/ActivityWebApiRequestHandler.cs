using Newtonsoft.Json;
using Bcc.ActivityWeb.Client.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bcc.ActivityWeb.Client
{
    public class ActivityWebApiRequestHandler
    {
        public ActivityWebOptions Options { get; }
        protected IHttpClientFactory ClientFactory { get; set; }
        protected string UriPrefix { get; set; }

        public JsonSerializerSettings SerializationSettings { get; set; }

        public ActivityWebApiRequestHandler(ActivityWebOptions options, IHttpClientFactory clientFactory, string uriPrefix)
        {
            Options = options;
            ClientFactory = clientFactory;
            UriPrefix = uriPrefix;
            TeamId = options.DefaultTeamId;
            SerializationSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects
            };
        }

        protected HttpClient CreateClient(TimeSpan? timeout = null)
        {
            var client = ClientFactory.CreateClient();
            if (client.BaseAddress == null || client.BaseAddress.ToString() == "")
            {
                client.BaseAddress = new Uri(Options.BasePath);
            }
            client.Timeout = timeout ?? Timeout.InfiniteTimeSpan;
            return client;
        }

        public int? UserId { get; private set; }
        public int? TeamId { get; private set; }

        public ActivityWebApiRequestHandler ForUser(int teamId, int userId)
        {
            var userHandler = new ActivityWebApiRequestHandler(Options, ClientFactory, UriPrefix);
            userHandler.UserId = userId;
            userHandler.TeamId = teamId;
            return userHandler;
        }

        private static ConcurrentDictionary<string, (DateTimeOffset created, AuthResponse auth)> _tokenCache = new ConcurrentDictionary<string, (DateTimeOffset created, AuthResponse auth)>();
        public async Task<string> GetSystemTokenAsync(int teamId)
        {
            if (!_tokenCache.ContainsKey("SYSTEM_" + teamId) || _tokenCache["SYSTEM_" + teamId].created < DateTimeOffset.Now.AddDays(-1))
            {
                // Get user token if not found or more than 1 day old
                _tokenCache["SYSTEM_" + teamId] = (DateTimeOffset.Now, (await PostUnauthorizedAsync<AuthResponse, object>("authenticate", new { UserKey = Options.SystemUserKey }).ConfigureAwait(false)));
            }
            return _tokenCache["SYSTEM_" + teamId].auth.Token;
        }

        public async Task<string> GetUserTokenAsync(int teamId, int userId)
        {
            if (!_tokenCache.ContainsKey("USER_" + userId + "_" + teamId) || _tokenCache["USER_" + userId + "_" + teamId].created < DateTimeOffset.Now.AddDays(-1))
            {
                // Generate authentication key from userID
                var userKey = "";
                var key = Convert.FromBase64String(Options.UserEncryptionKey);
                var iv = Convert.FromBase64String(Options.UserEncryptionIV);

                var rijndael = new RijndaelManaged();
                var encryptor = rijndael.CreateEncryptor(key, iv);
                var ms = new MemoryStream();

                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(userId.ToString());
                }
                userKey = Convert.ToBase64String(ms.ToArray());
                rijndael.Clear();

                // Get user token
                _tokenCache["USER_" + userId + "_" + teamId] = (DateTimeOffset.Now, (await PostUnauthorizedAsync<AuthResponse, object>("authenticate", new { UserSecret = userKey }).ConfigureAwait(false)));
            }
            return _tokenCache["USER_" + userId + "_" + teamId].auth.Token;
        }


        protected virtual TResponse ParseContent<TResponse>(string content)
        {
            if (content == null)
            {
                return default(TResponse);
            }
            if (typeof(TResponse) == typeof(string))
            {
                return (TResponse)(object)content;
            }
            return JsonConvert.DeserializeObject<TResponse>(content);
        }

        protected virtual string GetQueryStringFromObject(object parameters)
        {
            if (parameters == null || parameters.ToString() == "")
            {
                return "";
            }
            var type = parameters.GetType();
            if (type.GetTypeInfo().IsPrimitive)
            {
                throw new ArgumentException("Parameters must be an object with fields or properties. Primative types are not supported.");
            }
            var output = new StringBuilder("");
            foreach (var field in type.GetTypeInfo().GetFields(System.Reflection.BindingFlags.Public))
            {
                var rawValue = field.GetValue(parameters);
                string value = null;
                if (rawValue != null && rawValue.GetType() == typeof(DateTime))
                {
                    value = ((DateTime)rawValue).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff");
                }
                else if (rawValue != null && rawValue.GetType() == typeof(DateTimeOffset))
                {
                    value = ((DateTimeOffset)rawValue).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                }
                else
                {
                    value = rawValue?.ToString();
                }

                if (!string.IsNullOrEmpty(value))
                {
                    output.Append($"{field.Name}={WebUtility.UrlEncode(value)}&");
                }
            }
            foreach (var property in type.GetTypeInfo().GetProperties())
            {
                var rawValue = property.GetValue(parameters);
                string value = null;
                if (rawValue != null && rawValue.GetType() == typeof(DateTime))
                {
                    value = ((DateTime)rawValue).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffff");
                }
                else if (rawValue != null && rawValue.GetType() == typeof(DateTimeOffset))
                {
                    value = ((DateTimeOffset)rawValue).ToString("yyyy-MM-ddTHH\\:mm\\:ss.fffffffzzz");
                }
                else
                {
                    value = rawValue?.ToString();
                }
                if (!string.IsNullOrEmpty(value))
                {
                    output.Append($"{property.Name}={WebUtility.UrlEncode(value)}&");
                }
            }
            var result = output.ToString().TrimEnd('&');
            return result.Length > 0 ? $"?{result}" : "";
        }

        protected virtual async Task<TResponse> HandleResponseErrorAsync<TResponse>(HttpResponseMessage response, string relativeUri)
        {
            var errorContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var path = CreateClient().BaseAddress + $"{UriPrefix}/{relativeUri}".TrimEnd('/');
            throw new ActivityWebApiException($"Request to {path} failed with status code {response.StatusCode}.{(!string.IsNullOrEmpty(errorContent) ? " Response: " + errorContent : "")}")
            {
                StatusCode = response.StatusCode,
                RequestUri = path,
                Response = errorContent ?? ""
            };
        }

        public Task<TResponse> GetAsync<TResponse>(object parameters)
        {
            return GetAsync<TResponse>("", parameters);
        }

        public async Task<TResponse> GetAsync<TResponse>(string relativeUri, object parameters = null, CancellationToken cancellation = default)
        {
            var client = CreateClient(Timeout.InfiniteTimeSpan);
            var uri = $"{UriPrefix}/{relativeUri}{GetQueryStringFromObject(parameters)}".TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            if (!UserId.HasValue || UserId == 0)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Token", await GetSystemTokenAsync(TeamId.GetValueOrDefault()).ConfigureAwait(false));
            }
            else
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetUserTokenAsync(TeamId.GetValueOrDefault(), UserId.Value).ConfigureAwait(false));
            }

            var response = await client.SendAsync(request, cancellation).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ParseContent<TResponse>(content);
            }
            else
            {
                return await HandleResponseErrorAsync<TResponse>(response, relativeUri).ConfigureAwait(false);
            }
        }

        public Task<TResponse> GetFirstOrDefaultAsync<TResponse>(object parameters)
        {
            return GetFirstOrDefaultAsync<TResponse>("", parameters);
        }

        public async Task<TResponse> GetFirstOrDefaultAsync<TResponse>(string relativeUri, object parameters = null)
        {
            var response = await GetAsync<List<TResponse>>(relativeUri, parameters).ConfigureAwait(false);
            return response != null ? response.FirstOrDefault() : default(TResponse);
        }

        public virtual Task<T> PostAsync<T>(T entity = default(T))
        {
            return PostAsync<T, T>("", entity);
        }

        public virtual Task<TEntity> PostAsync<TEntity>(string relativeUri, TEntity entity = default(TEntity))
        {
            return PostAsync<TEntity, TEntity>(relativeUri, entity);
        }
        public virtual async Task<TResponse> PostAsync<TResponse, TRequest>(string relativeUri, TRequest entity = default(TRequest))
        {
            var client = CreateClient(TimeSpan.FromMinutes(5));
            var postContent = JsonConvert.SerializeObject(entity, SerializationSettings);
            var uri = $"{UriPrefix}/{relativeUri}".TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = entity == null ? new StringContent("") : new StringContent(postContent, Encoding.UTF8, "application/json");
            if (!UserId.HasValue || UserId == 0)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetSystemTokenAsync(TeamId.GetValueOrDefault()).ConfigureAwait(false));
            }
            else
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetUserTokenAsync(TeamId.GetValueOrDefault(), UserId.Value).ConfigureAwait(false));
            }
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ParseContent<TResponse>(content);
            }
            else
            {
                return await HandleResponseErrorAsync<TResponse>(response, relativeUri).ConfigureAwait(false);
            }
        }

        public virtual async Task<TResponse> PostUnauthorizedAsync<TResponse, TRequest>(string relativeUri, TRequest entity = default(TRequest))
        {
            var client = CreateClient(TimeSpan.FromMinutes(5));
            var postContent = JsonConvert.SerializeObject(entity, SerializationSettings);
            var uri = $"{UriPrefix}/{relativeUri}".TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Post, uri);
            request.Content = entity == null ? new StringContent("") : new StringContent(postContent, Encoding.UTF8, "application/json");
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ParseContent<TResponse>(content);
            }
            else
            {
                return await HandleResponseErrorAsync<TResponse>(response, relativeUri).ConfigureAwait(false);
            }
        }


        public virtual Task<TEntity> PutAsync<TEntity>(string relativeUri, TEntity entity = default(TEntity))
        {
            return PutAsync<TEntity, TEntity>(relativeUri, entity);
        }
        public virtual async Task<TResponse> PutAsync<TResponse, TRequest>(string relativeUri, TRequest entity = default(TRequest))
        {
            var client = CreateClient(TimeSpan.FromMinutes(5));
            var postContent = JsonConvert.SerializeObject(entity, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            var uri = $"{UriPrefix}/{relativeUri}".TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Put, uri);
            if (!UserId.HasValue || UserId == 0)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetSystemTokenAsync(TeamId.GetValueOrDefault()).ConfigureAwait(false));
            }
            else
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetUserTokenAsync(TeamId.GetValueOrDefault(), UserId.Value).ConfigureAwait(false));
            }
            request.Content = entity == null ? new StringContent("") : new StringContent(postContent, Encoding.UTF8, "application/json");
            
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ParseContent<TResponse>(content);
            }
            else
            {
                return await HandleResponseErrorAsync<TResponse>(response, relativeUri).ConfigureAwait(false);
            }
        }

        public virtual async Task<TResponse> DeleteAsync<TResponse>(string relativeUri)
        {
            var client = CreateClient(TimeSpan.FromMinutes(5));
            var uri = $"{UriPrefix}/{relativeUri}".TrimEnd('/');
            var request = new HttpRequestMessage(HttpMethod.Delete, uri);
            if (!UserId.HasValue || UserId == 0)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetSystemTokenAsync(TeamId.GetValueOrDefault()).ConfigureAwait(false));
            }
            else
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await GetUserTokenAsync(TeamId.GetValueOrDefault(), UserId.Value).ConfigureAwait(false));
            }
            var response = await client.SendAsync(request).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return ParseContent<TResponse>(content);
            }
            else
            {
                return await HandleResponseErrorAsync<TResponse>(response, relativeUri).ConfigureAwait(false);
            }
        }


    }
}
