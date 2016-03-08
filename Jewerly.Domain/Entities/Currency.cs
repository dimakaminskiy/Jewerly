namespace Jewerly.Domain
{
    public class Currency
    {
        public int CurrencyId { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Rate { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class Discount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Value { get; set; }
       
    }

    public class Markup // наценка
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Retail { get; set; }
        public int Trade { get; set; }
       
    }

}