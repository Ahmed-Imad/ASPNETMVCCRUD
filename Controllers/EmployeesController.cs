using ASPNETMVCCRUD.Data;
using ASPNETMVCCRUD.Models;
using ASPNETMVCCRUD.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace ASPNETMVCCRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mVCDemoDbContext;

        public EmployeesController(MVCDemoDbContext mVCDemoDbContext)
        {
            this.mVCDemoDbContext = mVCDemoDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
             var employees= await mVCDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                Department = addEmployeeRequest.Department,
                DateOfBirth = addEmployeeRequest.DateOfBirth
            };

            await mVCDemoDbContext.Employees.AddAsync(employee);
            await mVCDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id) 
        {
            var employee = await mVCDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null)
            {
               var viewModel = new UpdateEmployeeViewModel()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Email = employee.Email,
                    Salary = employee.Salary,
                    Department = employee.Department,
                    DateOfBirth = employee.DateOfBirth
                };
                return await Task.Run(() => View("View",viewModel));
            }

            return RedirectToAction("Index"); 
        }

        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployeeViewModel viewModel)
        {
            var employee = await mVCDemoDbContext.Employees.FindAsync(viewModel.Id);
            
            if (employee != null)
            {
                employee.Name = viewModel.Name;
                employee.Email = viewModel.Email;
                employee.Salary = viewModel.Salary;
                employee.Department = viewModel.Department;
                employee.DateOfBirth = viewModel.DateOfBirth;
                 
                await mVCDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            };
            return RedirectToAction("Index");
            
        }

        [HttpPost]

        public async Task<IActionResult> Delete(UpdateEmployeeViewModel viewModel)
        {
            var employee = await mVCDemoDbContext.Employees.FindAsync(viewModel.Id);
            if (employee != null)
            {
                mVCDemoDbContext.Employees.Remove(employee);
                await mVCDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
