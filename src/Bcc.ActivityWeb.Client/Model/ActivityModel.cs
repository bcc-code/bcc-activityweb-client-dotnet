using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class ActivityModel : ActivityBaseModel
    {
        public Dictionary<string, string> ActivityCustomFields { get; set; }

        public bool? AfterRegistrationPeriod { get; set; }

        public bool? AfterUnregistrationDate { get; set; }

        public bool? AllowedToRegister { get; set; }

        public bool? AllowedToUnRegister { get; set; }

        public bool? AllowRegistration { get; set; }

        public bool? CanApproveHours { get; set; }

        public bool? CanRegisterHours { get; set; }

        public int? CollidingActivityId { get; set; }

        public List<CustomFieldModel> CustomFields { get; set; }

        public bool? DeadlineNextWeek { get; set; }

        public bool? DeadlineToday { get; set; }

        public decimal? DefaultHoursPerParticipant { get; set; }

        public bool HasCustomFields => CustomFields != null && CustomFields.Count > 0;

        public bool? HasProducts { get; set; }

        public bool? HasRegistrationDeadline { get; set; }

        public bool? HasReserveRegistrationDeadline { get; set; }

        /// <summary>
        ///     To support MyEvents blocking registration on activities where sales module is used - include HasTickets.
        ///     HasProducts is the new values to use, but MyEvents still looks for HasTickets.
        /// </summary>
        public bool? HasTickets => HasProducts;

        public bool? HasUnregistrationDeadline { get; set; }

        public bool? IsComingUp { get; set; }

        public bool? IsFull { get; set; }

        public bool? IsNext30Days { get; set; }

        public bool? IsNext3Days { get; set; }

        public bool? IsNextWeek { get; set; }

        public bool? IsOngoing { get; set; }

        public bool? IsRegistered { get; set; }

        public bool? IsResponsible { get; set; }

        public bool? IsSingleDayActivity { get; set; }

        public bool? IsToday { get; set; }

        public string ListText { get; set; }

        public int? MaxAge { get; set; }

        public int? MinAge { get; set; }

        public int? NeededParticipants { get; set; }

        public bool? NeedsRegistrations { get; set; }

        public int? NumRegistrations { get; set; }

        public int? OwnerGroup { get; set; }

        public string Reference { get; set; }

        public RegistrationModel Registration { get; set; }

        public DateTime? RegistrationDeadline { get; set; }

        /// <summary>
        ///     Hours registered for current user
        /// </summary>
        public decimal? RegistrationHours { get; set; }

        public RegistrationModel[] Registrations { get; set; }

        public int? RegistrationsLeft { get; set; }

        public bool? RequireDescriptionForHours { get; set; }

        public bool? RequireFromToHourRegistration { get; set; }

        public bool RequireRegistrationPeriod { get; set; }

        public DateTime? ReserveRegistrationDeadline { get; set; }

        public UserModel ResponsibleUser { get; set; }

        public int? ResponsibleUserId { get; set; }

        public ActivityStatus Status { get; set; }

        public string StatusText { get; set; }

        public int? TeamId { get; set; }

        public int? TypeId { get; set; }

        public string TypeName { get; set; }

        public DateTime? UnregistrationDeadline { get; set; }
    }
}
