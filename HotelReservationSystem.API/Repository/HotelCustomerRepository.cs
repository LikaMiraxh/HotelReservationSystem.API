using HotelReservationSystem.API.Data;
using HotelReservationSystem.API.Interfaces;
using HotelReservationSystem.API.Models;
using HotelReservationSystem.API.Services;

namespace HotelReservationSystem.API.Repository
{
    public class HotelCustomerRepository : IHotelCustomerRepository
    {
        private readonly HotelContext _context;

        public HotelCustomerRepository(HotelContext context)
        {
            _context = context;
        }

        public void CreateCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            SaveChanges();
        }

        public void DeleteCustomer(int customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            _context.Customers.Remove(customer);
            SaveChanges();
        }

        public Customer GetCustomerById(int customerId)
        {
            var costumer = _context.Customers.FirstOrDefault(c => c.Id == customerId);
            return costumer;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public (IEnumerable<Customer>, PaginationMetadata) SearchCustomers(string? searchTerm, string? sortBy, bool ascending, int pageNumber, int pageSize)
        {
            var customers = _context.Customers.AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                customers = customers.Where(c => c.Name.Contains(searchTerm) || c.Email.Contains(searchTerm));
            }

            switch (sortBy)
            {
                case "Name":
                    customers = ascending ? customers.OrderBy(c => c.Name) : customers.OrderByDescending(c => c.Name);
                    break;
                case "Email":
                    customers = ascending ? customers.OrderBy(c => c.Email) : customers.OrderByDescending(c => c.Email);
                    break;
                default:
                    customers = ascending ? customers.OrderBy(c => c.Id) : customers.OrderByDescending(c => c.Id);
                    break;
            }

            var totalItemCount = customers.Count();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            customers = customers
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize);

            return (customers.ToList(), paginationMetadata);
        }

        public bool SaveChanges()
        {
            _context.SaveChanges();
            return true;
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            SaveChanges();
        }
    }
}
