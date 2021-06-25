using Newtonsoft.Json;
using Bcc.ActivityWeb.Client.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bcc.ActivityWeb.Client
{

    public class ActivityWebClient
    {
        /// <summary>
        /// Constructor for integration testing
        /// </summary>
        /// <param name="httpClient"></param>
        public ActivityWebClient(ActivityWebOptions options, IHttpClientFactory httpClientFactory)
        {
            Options = options;
            HttpClientFactory = httpClientFactory;
            Initialize(options, httpClientFactory);
        }

        protected void Initialize(ActivityWebOptions options, IHttpClientFactory clientFactory)
        {
            Api = new ActivityWebApiRequestHandler(options, clientFactory, "api");
            Registrations = new RegistrationsClient(this);
            Activities = new ActivitiesClient(this);
            Products = new ProductsClient(this);
            RegistrationDetails = new RegistrationDetailsClient(this);
            Orders = new OrdersClient(this);
            Users = new UsersClient(this);
        }

        public ActivityWebClient ForUser(int userId)
        {
            var userClient = new ActivityWebClient(Options, HttpClientFactory);
            userClient.Api = userClient.Api.ForUser(Options.DefaultTeamId, userId);
            return userClient;
        }

        public IHttpClientFactory HttpClientFactory { get; private set; }

        public ActivityWebOptions Options { get; private set; }

        protected ActivityWebApiRequestHandler Api { get; private set; }

        public RegistrationsClient Registrations { get; private set; }

        public RegistrationDetailsClient RegistrationDetails { get; private set; }

        public ActivitiesClient Activities { get; private set; }

        public ProductsClient Products { get; private set; }

        public UsersClient Users { get; private set; }

        public OrdersClient Orders { get; private set; }


        public abstract class ClientBase
        {
            public ClientBase(ActivityWebClient activityWeb)
            {
                ActivityWeb = activityWeb;
            }
            protected ActivityWebClient ActivityWeb { get; }

            protected ActivityWebOptions Options => ActivityWeb.Options;

            protected ActivityWebApiRequestHandler Api => ActivityWeb.Api;
        }

        /// <summary>
        /// /api/registrations
        /// </summary>
        public class RegistrationDetailsClient : ClientBase
        {
            public RegistrationDetailsClient(ActivityWebClient activityWeb) : base(activityWeb)
            {
            }

            public Task<List<RegistrationDetailModel>> GetRegistrationDetailsAsync(int activityId)
            {
                var uri = $"registrationdetails?activityId={activityId}";
                return Api.GetAsync<List<RegistrationDetailModel>>(uri);
            }

        }


        /// <summary>
        /// /api/registrations
        /// </summary>
        public class RegistrationsClient : ClientBase
        {
            public RegistrationsClient(ActivityWebClient activityWeb) : base(activityWeb)
            {
            }

            public Task<List<RegistrationModelBase>> GetRegistrationBasicsForActivityAsync(int activityId, CancellationToken cancellationToken)
            {
                var uri = $"registrations/getRegistrationBasicsForActivity/{activityId}";
                return Api.GetAsync<List<RegistrationModelBase>>(uri, null, cancellationToken);
            }

            public Task<List<RegistrationModel>> GetRegistrationForActivityAsync(int activityId, CancellationToken cancellationToken)
            {
                var uri = $"registrations?activityId={activityId}";
                return Api.GetAsync<List<RegistrationModel>>(uri, null, cancellationToken);
            }

        }

        /// <summary>
        /// /api/activities
        /// </summary>
        public class ActivitiesClient : ClientBase
        {
            public ActivitiesClient(ActivityWebClient activityWeb) : base(activityWeb)
            {
            }

            public Task<CommonApiResponse<List<UserActivityProductOverview>>> GetUserActivityProductOverviewsByRefAsync(string reference = "", int teamId = 0, CancellationToken cancellationToken = default)
            {
                var uri = $"activities/getUserActivtyProductOverviewsByRef/{(teamId != 0 ? teamId : Options.DefaultTeamId)}/{WebUtility.UrlEncode(reference)}";
                return Api.GetAsync<CommonApiResponse<List<UserActivityProductOverview>>>(uri, null, cancellationToken);
            }

            public Task<ActivityModel> GetActivityAsync(int activityId, object queryOptions = null, CancellationToken cancellation = default)
            {
                return Api.GetAsync<ActivityModel>($"activities/{activityId}", queryOptions, cancellation);
            }

            public Task<List<ActivityModel>> GetActivitiesAsync(bool includeFinished = false, DateTime? activityFrom = null, DateTime? activityTo = null, string fieldSelection = null, CancellationToken cancellation = default)
            {
                return Api.GetAsync<List<ActivityModel>>($"activities", new { includeFinished, activityFrom, activityTo, fieldSelection }, cancellation);
            }

        }

        /// <summary>
        /// /api/activities
        /// </summary>
        public class OrdersClient : ClientBase
        {
            public OrdersClient(ActivityWebClient activityWeb) : base(activityWeb)
            {
            }

            public Task<List<OrderModel>> GetOrdersByActivityRefAsync(string reference = null, int teamId = 0, CancellationToken cancellationToken = default)
            {
                var uri = $"order/byActivityRef/{(teamId != 0 ? teamId : Options.DefaultTeamId)}{(reference != null ? "/" + WebUtility.UrlEncode(reference) : "")}";
                return Api.GetAsync<List<OrderModel>>(uri, null, cancellationToken);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="teamId"></param>
            /// <param name="criteria"></param>
            /// <param name="include">Optional: 
            /// OrderLines,
            /// OrderLines.Activity, 
            /// OrderedByUser, 
            /// OrderLines.OrderedForUser</param>
            /// <returns></returns>
            public async Task<List<OrderModel>> GetOrdersAsync(int userId, string reference = null, int teamId = 0, bool includeOrdersFromSpouse = true, string[] include = null, CancellationToken cancellation = default)
            {
                var criteria = new OrderSearchCriteriaDto { Reference = reference, IncludeOrdersFromSpouse = includeOrdersFromSpouse };


                var uri = $"order/{(teamId != 0 ? teamId : Options.DefaultTeamId)}/{userId}?Reference={Uri.EscapeDataString(criteria.Reference)}&IncludeOrdersFromSpouse={criteria.IncludeOrdersFromSpouse}";
                if (include != null && include.Length > 0)
                {
                    foreach (var field in include)
                    {
                        uri += "&include=" + Uri.EscapeDataString(field);
                    }
                }
                var response = await Api.GetAsync<CommonApiResponse<List<OrderModel>>>(uri, null, cancellation);
                return response.Value;
            }


        }

        /// <summary>
        /// /api/products
        /// </summary>
        public class ProductsClient : ClientBase
        {
            public ProductsClient(ActivityWebClient activityWeb) : base(activityWeb)
            {
            }            

        }


        /// <summary>
        /// /api/products
        /// </summary>
        public class UsersClient : ClientBase
        {
            public UsersClient(ActivityWebClient activityWeb) : base(activityWeb)
            {
            }

            public Task<UserSearchModel> GetUserAsync(int userId, CancellationToken cancellationToken)
            {
                string uri = $"users?id={userId}";
                return Api.GetAsync<UserSearchModel>(uri, null, cancellationToken);
            }


        }

    }

}
