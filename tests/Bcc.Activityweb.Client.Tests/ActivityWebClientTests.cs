using Bcc.ActivityWeb.Client.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bcc.ActivityWeb.Client.Tests
{
   // [Ignore("Live integration tests")]
    public class ActivityWebClientTests
    {
        public ActivityWebOptions Options { get; set; }


        [SetUp]
        public void Setup()
        {
            Options = ConfigHelper.GetApplicationConfiguration(TestContext.CurrentContext.TestDirectory);
        }


        [Test]
        public async Task GetActivitiesAndRegistrationsForReferenceCodeTest()
        {
            var client = new ActivityWebClient(Options, new TestHttpClientFactory());
            var activities = await client.Activities.GetActivitiesAsync(false, DateTime.Today, DateTime.Today.AddMonths(3), 
                $"{nameof(ActivityModel.Start)},{nameof(ActivityModel.Finish)},{nameof(ActivityModel.Reference)},{nameof(ActivityModel.Status)}"); // "SC21"

            var scActivities = activities.Where(r => r.Reference.ToLower().Equals("sommer21")).ToList();
            var persons = new HashSet<int>();

            foreach (var scActivity in scActivities)
            {
                var registrations = await client.Registrations.GetRegistrationBasicsForActivityAsync(scActivity.ActivityId, default(CancellationToken));
                foreach (var registration in registrations)
                {
                    if (!persons.Contains(registration.User.UserId))
                    {
                        persons.Add(registration.User.UserId);
                    }
                }
            }

            Assert.That(persons.Count > 0);
            Assert.NotNull(activities);
        }


        // https://registration.activityweb.no/administration/activities/getActivitiesForActivitiesSelector


    }
}