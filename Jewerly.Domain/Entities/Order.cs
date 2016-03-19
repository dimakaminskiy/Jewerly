using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        [Display(Name = "Дата")]
        public DateTime OrderDate { get; set; }
        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }
        [Required]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
        [Display(Name = "Сумма")]
        public double Total { get; set; }
        [Display(Name = "Страна")]
        public int CountryId { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; }
        [Display(Name = "Комментарии")]
        public string TextInfo { get; set; }
        [Display(Name = "Статус")]
        public int OrderStatusId { get; set; }
        [Display(Name = "Способ оплаты")]
        public int MethodOfPaymentId { get; set; }
        [Display(Name = "Метод доставки")]
        public int MethodOfDeliveryId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual MethodOfDelivery MethodOfDelivery { get; set; }
        public virtual MethodOfPayment MethodOfPayment { get; set; }
        public virtual Country Country { get; set; }
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
        public virtual Currency Currency { get; set; }
       

    }

    public partial class OrderStatus
    {
        public OrderStatus()
        {
            this.Orders = new HashSet<Order>();
        }
    
        public int Id { get; set; }
         [Required]
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

        public class MethodOfDelivery
        {
            public int Id { get; set; }
             [Required]
            [Display(Name = "Название")]
            public string Name { get; set; }
             [Display(Name = "Вывод")]
             public bool Available { get; set; }
             [Display(Name = "Порядок")]
             public int DisplayOrder { get; set; }
            public virtual ICollection<Order> Orders { get; set; }
        }

        public class MethodOfPayment
        {
            public int Id { get; set; }
             [Required]
            [Display(Name = "Название")]
            public string Name { get; set; }
             [Display(Name = "Вывод")]
             public bool Available { get; set; }
             [Display(Name = "Порядок")]
             public int DisplayOrder { get; set; }
            public virtual ICollection<Order> Orders { get; set; }
        }

        public partial class Cart
        {
            public int Id { get; set; }
            public string CartId { get; set; }
            public int ProductId { get; set; }
            public DateTime DateCreated { get; set; }
            public int Count { get; set; }
            public virtual Product Product { get; set; }

        }

}



