namespace EMedicineBE.Models
{
    public class OrderItems
    {
        public int ID { get; set; }
        public int OrderID { get; set; }

        public int MedicineID { get; set; }
        public decimal UnitPrce { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }

        public decimal TotlalPrice { get; set; }
    }
}
