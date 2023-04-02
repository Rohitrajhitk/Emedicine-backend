namespace EMedicineBE.Models
{
    public class Cart
    {
        public int ID { get; set; }
        public int  UserId { get; set; }
        public decimal UnitPrce { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        
        public decimal TotlalPrice { get; set; }

        public int MedicineId  { get; set; }
    }
}
