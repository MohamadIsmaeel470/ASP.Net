using RentACar_Domin;

namespace RentACar_API.Models
{
    public class VehicleForCreation
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int ManufactureYear { get; set; }
        public VehicleState Status { get; set; }
        public int VehicleTypeID { get; set; }
        public int BranchID { get; set; }
    }

    public class VehicleForUpdate
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int ManufactureYear { get; set; }
        public VehicleState Status { get; set; }
        public int VehicleTypeID { get; set; }
        public int BranchID { get; set; }
    }

    public class VehicleDto
    {
        public int VehicleID { get; set; }
        public string PlateNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int ManufactureYear { get; set; }
        public VehicleState Status { get; set; }
        public int VehicleTypeID { get; set; }
        public int BranchID { get; set; }
    }
}
