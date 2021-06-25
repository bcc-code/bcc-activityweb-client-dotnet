using System;
using System.Collections.Generic;
using System.Text;

namespace Bcc.ActivityWeb.Client.Model
{
    public class OrderModel
    {
        public int ActivityId { get; set; }

        public string CellPhone { get; set; }

        public DateTime? Changed { get; set; }

        public string Comment { get; set; }

        public DateTime? Created { get; set; }

        public string Currency { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int OrderedByUserId { get; set; }

        public UserModel OrderedByUser { get; set; }

        public List<OrderLineModel> OrderLines { get; set; } = new List<OrderLineModel>();

        public string OrderNumber { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
