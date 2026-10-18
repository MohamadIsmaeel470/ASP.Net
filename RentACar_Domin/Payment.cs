using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar_Domin
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        public int RentalID { get; set; }
        public Rental Rental { get; set; } = new Rental();
    }
}
