using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Sales
{
    public class CustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Customer>> GetCustomersAsync() => _repository.GetAllAsync();
        public Task<Customer> GetCustomerAsync(int id) => _repository.GetByIdAsync(id);
        public Task AddCustomerAsync(Customer customer) => _repository.AddAsync(customer);
        public Task UpdateCustomerAsync(Customer customer) => _repository.UpdateAsync(customer);
        public Task DeleteCustomerAsync(int id) => _repository.DeleteAsync(id);

        // 🔹 Nuevo método: clientes por ciudad
        public Task<IEnumerable<Customer>> GetCustomersByCityAsync(string city)
            => _repository.GetByCityAsync(city);
    }
}
