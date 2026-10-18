using System.Net;

namespace RentACar_Domin
{
    public class Branch
    {
        public int BranchID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public Address Address { get; set; } = new Address();
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public int  VehicleID { get; set; }
        public int  EmployeeID { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        public List<Employee> Employees { get; set; } = new List<Employee>();

    }
}
