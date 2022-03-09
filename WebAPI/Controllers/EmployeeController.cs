using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebAPI.Contract;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {


        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeRepository employeeRepository;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository)
        {
            _logger = logger;
            this.employeeRepository = employeeRepository;
        }

        #region Department
        [HttpGet("GetDepartments")]
        public async Task<ActionResult> GetDepartments()
        {
            try
            {
                return Ok(await employeeRepository.GetDepartments());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [HttpGet("GetDepById")]
        public async Task<ActionResult> GetDepById(int DepId)
        {
            try
            {
                return Ok(await employeeRepository.GetDepById(DepId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("AddDepartment")]
        public async Task<ActionResult<MstDepartment>> AddDepartment([FromBody] DepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post= await employeeRepository.AddDepartment(model);
                    if (post == null)
                    {
                        return NotFound();
                    }

                    return Ok(post);
                }
                catch (Exception ex)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateDepartment")]
        public async Task<ActionResult<MstDepartment>> UpdateDepartment([FromBody] DepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post= await employeeRepository.UpdateDepartment(model);
                    if (post == null)
                    {
                        return NotFound();
                    }

                    return Ok(post);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        #endregion


        #region Employee
        [HttpGet("GetEmployees")]
        public async Task<ActionResult> GetEmployees()
        {
            try
            {
                return Ok(await employeeRepository.GetEmployees());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
        [HttpGet("GetEmpById")]
        public async Task<ActionResult> GetEmpById(int EmpId)
        {
            try
            {
                return Ok(await employeeRepository.GetEmpById(EmpId));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }


        [HttpGet]
        [Route("GetBySearch")]
        public async Task<ActionResult> GetBySearch(string EmpId, string FName, string LName, string DepName)
        {
           

            try
            {
                int EmployeeId = 0;
                if(EmpId==string.Empty)
                {
                    EmployeeId = 0;
                }
                else
                {
                    EmployeeId =Convert.ToInt32(EmpId);
                }
                var post = await employeeRepository.GetBySearch(EmployeeId, DepName, FName, LName);

                if (post == null)
                {
                    return NotFound();
                }

                return Ok(post);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("AddEmployee")]
        public async Task<ActionResult<EmployeeDetail>> AddEmployee([FromBody] EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post = await employeeRepository.AddEmployee(model);
                    if (post == null)
                    {
                        return NotFound();
                    }

                    return Ok(post);
                }
                catch (Exception ex)
                {

                    return BadRequest();
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("UpdateEmployee")]
        public async Task<ActionResult<EmployeeDetail>> UpdateEmployee([FromBody] EmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var post = await employeeRepository.UpdateEmployee(model);
                    if (post == null)
                    {
                        return NotFound();
                    }

                    return Ok(post);
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] int? empId)
        {
            int result = 0;

            if (empId == null)
            {
                return BadRequest();
            }

            try
            {
                result = await employeeRepository.DeleteEmployee(empId);
                if (result == 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        #endregion

    }

}
