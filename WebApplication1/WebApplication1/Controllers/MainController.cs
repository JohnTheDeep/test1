using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Filters.Authorization;
using WebApplication1.Interfaces;
using WebApplication1.Models.Entites;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class MainController : Controller
    {
        private readonly IDatabaseManager _databaseManager;
        public MainController(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }
        /// <summary>
        /// Fetch all employees from database.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("/GetEmployees")]
        public async Task<IActionResult> GetEmployeesAsync() => Json(await _databaseManager.GetAllEmployeesAsync());
        /// <summary>
        /// Create new employee.
        /// </summary>
        [AuthorizationFilter]
        [HttpPost("/CreateEmployee")]
        public async Task<IActionResult> CreateEmployeeAsync(string FullName, string rank)
        {
            var rankObj = await _databaseManager.GetRankByNameAsync(rank);

            if (await _databaseManager.FindEmployeeByName(FullName))
                return BadRequest("Employee with this name already exist!");

            var employee = new Employee(FullName, rankObj);
            await _databaseManager.CreateEmployeeAsync(employee);

            return Json(employee);
        }
        /// <summary>
        /// Remove selected employee.
        /// </summary>
        [AuthorizationFilter]
        [HttpDelete("/RemoveEmployee")]
        public async Task<IActionResult> RemoveEmployeeAsync(int employeeId)
        {
            var state = await _databaseManager.RemoveEmployee(employeeId);

            return Json(state == true ? "Employee was deleted!" : "Someting wrong!");
        }
        /// <summary>
        /// Update selected employee.
        /// </summary>
        [AuthorizationFilter]
        [HttpPut("/UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, string fullname, string rank)
        {
            var state = await _databaseManager
                .UpdateEmployee(employeeId, new Employee(fullname, await _databaseManager.GetRankByNameAsync(rank) ?? new Rank()));

            return Json(state == true ? "Employee was updated!" : "Someting wrong!");
        }
    }
}
