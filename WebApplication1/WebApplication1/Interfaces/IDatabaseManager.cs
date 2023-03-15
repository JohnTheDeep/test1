using WebApplication1.Models.Entites;

namespace WebApplication1.Interfaces
{
    public interface IDatabaseManager
    {
        #region Employees
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<bool> CreateEmployeeAsync(Employee employee);
        Task<bool> FindEmployeeByName(string employeeName);
        Task<bool> RemoveEmployee(int employeeId);
        Task<bool> UpdateEmployee(int employeeId, Employee employee);
        #endregion

        #region Ranks
        Task<Rank> GetRankByNameAsync(string name);
        #endregion
    }
}
