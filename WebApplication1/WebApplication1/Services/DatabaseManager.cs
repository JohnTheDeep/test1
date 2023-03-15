using Microsoft.EntityFrameworkCore;
using WebApplication1.DatabaseContext;
using WebApplication1.Interfaces;
using WebApplication1.Models.Entites;

namespace WebApplication1.Services
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseManager> _logger;
        public DatabaseManager(ApplicationDbContext context, ILogger<DatabaseManager> logger)
        {
            _context = context;
            _logger = logger;

        }

        public async Task<bool> CreateEmployeeAsync(Employee employee)
        {
            if (employee == null)
                return false;
            try
            {
                await _context.employees.AddAsync(employee);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to create new Employee");
            }
            return true;
        }

        public async Task<bool> FindEmployeeByName(string employeeName)
        {
            if (employeeName == "")
                return false;

            try
            {
                return await _context.employees
                   .FirstOrDefaultAsync(el => el.FullName == employeeName) != null ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to GetRank By Name");
            }
            return false;
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            return await _context.employees.Include(el => el.Rank).ToListAsync();
        }

        public async Task<Rank> GetRankByNameAsync(string name)
        {
            if (name == "")
                return null;

            try
            {
                return await _context.ranks.FirstOrDefaultAsync(el => el.Name == name);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to GetRank By Name");
            }
            return null;
        }

        public async Task<bool> RemoveEmployee(int employeeId)
        {
            if (employeeId == 0)
                return false;

            try
            {
                var employee = await _context.employees.FirstOrDefaultAsync(el => el.Id == employeeId);

                if (employee == null)
                    return false;

                _context.employees.Remove(employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to remove employee");
            }
            return false;
        }
        private void ChangeProperties(Employee empA, Employee empB)
        {
            empA.FullName = empB.FullName;
            empA.Rank = empB.Rank;
        }
        public async Task<bool> UpdateEmployee(int employeeId, Employee employee)
        {
            if (employeeId == 0 && employee.FullName == "" && employee.Rank == null)
                return false;

            try
            {
                var employeeObj = await _context.employees.FirstOrDefaultAsync(el => el.Id == employeeId);

                if (employeeObj == null)
                    return false;

                ChangeProperties(employeeObj, employee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to remove employee");
            }
            return false;
        }
    }
}
