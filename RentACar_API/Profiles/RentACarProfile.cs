using AutoMapper;
using RentACar_API.Models;
using RentACar_Domin;

namespace RentACar_API.Profiles
{
    public class RentACarProfile : Profile
    {
        public RentACarProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerForCreation, Customer>();
            CreateMap<CustomerForUpdate, Customer>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeForCreation, Employee>()
                .ConstructUsing(e => new Employee(e.FirstName, e.LastName, e.Title, e.DOB, e.Email))
                .ForMember(e => e.Branch, option => option.Ignore());
            CreateMap<EmployeeForUpdate, Employee>()
                .ForMember(e => e.Branch, option => option.Ignore());

            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleForCreation, Vehicle>()
                .ForMember(v => v.VehicleType, option => option.Ignore())
                .ForMember(v => v.Branch, option => option.Ignore());
            CreateMap<VehicleForUpdate, Vehicle>()
                .ForMember(v => v.VehicleType, option => option.Ignore())
                .ForMember(v => v.Branch, option => option.Ignore());

            CreateMap<Branch, BranchDto>()
                .ForMember(b => b.Stret, option => option.MapFrom(b => b.Address.Stret));
            CreateMap<BranchForCreation, Branch>()
                .ForMember(b => b.Address, option => option.Ignore())
                .ForMember(b => b.Vehicles, option => option.Ignore())
                .ForMember(b => b.Employees, option => option.Ignore());
            CreateMap<BranchForUpdate, Branch>()
                .ForMember(b => b.Address, option => option.Ignore())
                .ForMember(b => b.Vehicles, option => option.Ignore())
                .ForMember(b => b.Employees, option => option.Ignore());

            CreateMap<Payment, PaymentDto>();
            CreateMap<PaymentForCreation, Payment>()
                .ForMember(p => p.Rental, option => option.Ignore())
                .ForMember(p => p.CreatedDate, option => option.Ignore());
            CreateMap<PaymentForUpdate, Payment>()
                .ForMember(p => p.Rental, option => option.Ignore())
                .ForMember(p => p.CreatedDate, option => option.Ignore());
        }
    }
}
