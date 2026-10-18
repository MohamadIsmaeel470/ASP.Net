namespace RentACar_API.Models
{
    public class CustomerForCreation
    {
        public string FirstName { get; set; } = string.Empty;
        public string LasttName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class CustomerForUpdate
    {
        public string FirstName { get; set; } = string.Empty;
        public string LasttName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class CustomerDto
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LasttName { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
