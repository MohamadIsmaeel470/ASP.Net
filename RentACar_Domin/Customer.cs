using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar_Domin
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LasttName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public String Phone { get; set; } = String.Empty;

        public List<Rental> Rentals { get; set; } = new List<Rental>();

        //public int RentalID { get; set; }
        //public List<Payment> Payments { get; set; } = new List<Payment>();
        //public int PaymentID { get; set; }

        //public Vehicle Vehicle { get; set; } = new Vehicle();
        //public int VehicleID { get; set; }

    }
}
