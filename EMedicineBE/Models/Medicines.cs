namespace EMedicineBE.Models
{
    public class Medicines
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal UnitPrce { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }

        public DateTime ExpDate { get; set; }
        public string ImageUrl { get; set; }
        public int Status { get; set; }

        public  string Type { get; set; }

       
    }
}
