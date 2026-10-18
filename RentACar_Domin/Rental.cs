using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar_Domin
{
    public class Rental
    {
        public int RentalId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }

        public int VehicleID { get; set; }
        public Vehicle Vehicle { get; set; } = new Vehicle();

        public Customer Customer { get; set; } = new Customer();
        public int CustomerID { get; set; }
        //عقد واحد اكثر من دفعة
        public List<Payment> Payments { get; set; } = new List<Payment>();

    }
}
