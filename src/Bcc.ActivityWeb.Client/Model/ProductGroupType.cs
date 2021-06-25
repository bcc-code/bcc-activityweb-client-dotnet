using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public enum ProductGroupType
    {
        Unknown = 0,
        SelectSingle = 1,
        FollowActivity = 2,
        SelectMultiple = 3,
        SelectWithChildSelect = 4,
        Check = 5,
        NumSelect = 6,
        FollowActivityAfterRegistrationDeadline = 7,
        FollowActivityButFreeIfOtherRegistration = 8,
        FoodCoupon = 9,
        DiscountFoodCoupon = 10,
        CustomPrice = 11
    }
}
