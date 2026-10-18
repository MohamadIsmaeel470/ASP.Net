namespace RentACar_API.Models
{
    public class EmployeeForCreation
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime HiredDate { get; set; }
        public int BranchID { get; set; }
    }

    public class EmployeeForUpdate
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime HiredDate { get; set; }
        public int BranchID { get; set; }
    }

    public class EmployeeDto
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime HiredDate { get; set; }
        public int BranchID { get; set; }
    }
}
