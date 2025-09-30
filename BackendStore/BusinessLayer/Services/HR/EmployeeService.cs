using Domain.Entities.HR;
using Domain.Interfaces.Repositories.HR;

namespace Domain.Services.HR
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Employee>> GetAllAsync() => _repository.GetAllAsync();
        public Task<Employee> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

    }
}
