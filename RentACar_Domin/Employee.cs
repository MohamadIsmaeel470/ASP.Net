using System;
using System.Collections.Generic;
using System.Text;

namespace RentACar_Domin
{
    public class Employee
    {
        public Employee(string firstName , string LastName , String Title , DateTime DOB , string Email)
        {
            this.FirstName = firstName;
            this.LastName = LastName;
            this.Title = Title;
            this.DOB = DOB;
            this.Email = Email;
        }
        public int EmployeeID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime DOB { get; set; }   
        public string Email { get; set; } = string.Empty;
        public DateTime HiredDate { get; set; }

        public Branch Branch { get; set; } = new Branch();
        public int BranchID { get; set; } 
    }
}
