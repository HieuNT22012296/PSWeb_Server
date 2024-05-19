namespace PSWeb_Server.Models
{
    public class Carts
    {
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    } 
}
