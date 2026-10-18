using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar_Domin
{
    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int ManufactureYear { get; set; }
        public VehicleState Status { get; set; }

        public VehicleType VehicleType { get; set; } = new VehicleType();
        public int VehicleTypeID { get; set; }
        public Branch Branch { get; set; } = new Branch();
        public int BranchID { get; set; }


    }
}
