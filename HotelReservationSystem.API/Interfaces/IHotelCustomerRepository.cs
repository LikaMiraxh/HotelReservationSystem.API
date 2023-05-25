using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Services;

namespace HotelReservationSystem.API.Interfaces
{
    public interface IHotelCustomerRepository
    {
        public bool SaveChanges();

        public IEnumerable<Customer> GetCustomers();
        public Customer GetCustomerById(int customerId);
        public void CreateCustomer(Customer customer);
        public void UpdateCustomer(Customer customer);
        public void DeleteCustomer(int customerId);

        public (IEnumerable<Customer> Customers, PaginationMetadata PaginationMetadata) SearchCustomers(string? searchTerm, string? sortBy, bool ascending, int pageNumber, int pageSize);
    }
}
