using System.ComponentModel.DataAnnotations;

namespace Jewerly.Domain
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Код валюты")]
        public string CurrencyCode { get; set; }
        [Display(Name = "Соотношение")]
        public decimal Rate { get; set; }
        [Display(Name = "Опубликовано?")]
        public bool Published { get; set; }
        [Display(Name = "Порядок сортировки")]
        public int DisplayOrder { get; set; }
    }

    public class Discount
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Значение")]
        public int Value { get; set; }
       
    }

    public class Markup // наценка
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Розничная")]
        public int Retail { get; set; }
        [Display(Name = "Оптовая")]
        public int Trade { get; set; }
       
    }

}