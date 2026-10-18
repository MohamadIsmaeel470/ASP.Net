namespace RentACar_API.Models
{
    public class PaymentForCreation
    {
        public double Amount { get; set; }
        public int RentalID { get; set; }
    }

    public class PaymentForUpdate
    {
        public double Amount { get; set; }
        public int RentalID { get; set; }
    }

    public class PaymentDto
    {
        public int PaymentID { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
        public int RentalID { get; set; }
    }
}
