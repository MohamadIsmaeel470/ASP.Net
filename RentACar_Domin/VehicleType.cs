using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar_Domin
{
    public class VehicleType
    {
        public int VehicleTypeID { get; set; }
        public string TypeName { get; set; } = string.Empty;    
        public int SeatsCount { get; set; }
        public double DailyRate { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    }
}
