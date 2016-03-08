using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jewerly.Domain.Entities
{
   
     public partial class Order
    {
        public Order()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            OrderDate = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public double? Total { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string TextInfo { get; set; }        
        public int OrderStatusId { get; set; }
        public int? MethodOfPaymentId { get; set; }
        public int MethodOfDeliveryId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual MethodOfDelivery MethodOfDelivery { get; set; }
        public virtual MethodOfPayment MethodOfPayment { get; set; }
    }


    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public  string ChoiceAttributesInJson { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int CurrencyId { get; set; }
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }

    public partial class OrderStatus
    {
        public OrderStatus()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

        public class MethodOfDelivery
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public virtual ICollection<Order> Orders { get; set; }
        }

        public class MethodOfPayment
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public virtual ICollection<Order> Orders { get; set; }
        }

}



