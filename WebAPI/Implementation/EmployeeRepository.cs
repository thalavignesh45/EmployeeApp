using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Contract;
using WebAPI.Models;

namespace WebAPI.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        EmployeeDBContext _dbContext;
        public EmployeeRepository(EmployeeDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        #region Department
        public async Task<List<MstDepartment>> GetDepartments()
        {
            if (_dbContext != null)
            {
                return await _dbContext.MstDepartment.OrderBy(x=>x.DepartmentName).ToListAsync();
            }

            return null;
        }
        public async Task<List<MstDepartment>> GetDepById(int DepId)
        {
            if (_dbContext != null)
            {
                return await _dbContext.MstDepartment.Where(x => x.DepartmentId == DepId).OrderBy(x => x.DepartmentName).ToListAsync();

            }

            return null;
        }
       
        
        public async Task<MstDepartment> AddDepartment(DepartmentDto detail)
        {
            MstDepartment dto = new MstDepartment();
            dto.DepartmentName = detail.DepartmentName;
            dto.IsActive =true;

           
            var result = await _dbContext.MstDepartment.AddAsync(dto);
            await _dbContext.SaveChangesAsync();
            return result.Entity;

        }

        public async Task<MstDepartment> UpdateDepartment(DepartmentDto detail)
        {
            var result = await _dbContext.MstDepartment
           .FirstOrDefaultAsync(e => e.DepartmentId == detail.DepartmentId);
            if (result != null)
            {
                result.DepartmentName = detail.DepartmentName;
               
                await _dbContext.SaveChangesAsync();

                return result;
            }

            return null;
            
        }

        #endregion

        #region Employee
        public async Task<List<EmployeeDto>> GetEmployees()
        {

            
            if (_dbContext != null)
            {
                return await (from e in _dbContext.EmployeeDetails
                              join d in _dbContext.MstDepartment on e.DepartmentId equals d.DepartmentId
                              select new EmployeeDto
                              {
                                  EmployeeId = e.EmployeeId,
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  DepartmentName = d.DepartmentName,
                                  DepartmentId = e.DepartmentId,
                                  EmailId = e.EmailId,
                                  ManagerId = e.ManagerId
                              }).OrderBy(x => x.FirstName).ToListAsync();
            }
            return null;
        }
        public async Task<List<EmployeeDto>> GetEmpById(int EmpId)
        {
            if (_dbContext != null)
            {
                return await (from e in _dbContext.EmployeeDetails
                              join d in _dbContext.MstDepartment on e.DepartmentId equals d.DepartmentId
                              where (e.EmployeeId == EmpId)
                                     
                              select new EmployeeDto
                              {
                                  EmployeeId = e.EmployeeId,
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  DepartmentName = d.DepartmentName,
                                  DepartmentId = e.DepartmentId,
                                  EmailId = e.EmailId,
                                  ManagerId = e.ManagerId
                              }).OrderBy(x => x.FirstName).ToListAsync();
            }

            return null;
        }
        public async Task<List<EmployeeDto>> GetBySearch(int EmployeeId, string DepName, string FirstName, string LastName)
        {
            if (_dbContext != null)
            {
                return await (from e in _dbContext.EmployeeDetails
                              join d in _dbContext.MstDepartment on e.DepartmentId equals d.DepartmentId
                              where (e.EmployeeId == EmployeeId || EmployeeId == 0) &&
                                     (e.FirstName == FirstName || string.IsNullOrEmpty(FirstName)) &&
                                     (e.LastName == LastName || string.IsNullOrEmpty(LastName)) &&
                                     (d.DepartmentName == DepName || string.IsNullOrEmpty(DepName))
                              select new EmployeeDto
                              {
                                  EmployeeId = e.EmployeeId,
                                  FirstName = e.FirstName,
                                  LastName = e.LastName,
                                  DepartmentName = d.DepartmentName,
                                  DepartmentId = e.DepartmentId,
                                  EmailId = e.EmailId,
                                  ManagerId = e.ManagerId
                              }).OrderBy(x => x.FirstName).ToListAsync();
            }
            return null;
        }

        public async Task<EmployeeDetail> AddEmployee(EmployeeDto detail)
        {
            EmployeeDetail dto = new EmployeeDetail();

            dto.FirstName = detail.FirstName;
            dto.LastName = detail.LastName;
            dto.EmailId = detail.EmailId;
            dto.DepartmentId = detail.DepartmentId;
            dto.ManagerId = detail.ManagerId;
            dto.IsActive = true;
            var result = await _dbContext.EmployeeDetails.AddAsync(dto);
            await _dbContext.SaveChangesAsync();
            return result.Entity;

        }

        public async Task<EmployeeDetail> UpdateEmployee(EmployeeDto detail)
        {
            var result = await _dbContext.EmployeeDetails
           .FirstOrDefaultAsync(e => e.EmployeeId == detail.EmployeeId);
            if (result != null)
            {
                result.FirstName = detail.FirstName;
                result.LastName = detail.LastName;
                result.EmailId = detail.EmailId;
                result.DepartmentId = detail.DepartmentId;
                result.ManagerId = detail.ManagerId;
                await _dbContext.SaveChangesAsync();

                return result;
            }

            return null;

            }

        public async Task<int> DeleteEmployee(int? empId)
        {
            int result = 0;

            if (_dbContext != null)
            {
                //Find the post for specific post id
                var post = await _dbContext.EmployeeDetails.FirstOrDefaultAsync(x => x.EmployeeId == empId);

                if (post != null)
                {
                    //Delete that post
                    _dbContext.EmployeeDetails.Remove(post);

                    //Commit the transaction
                    result = await _dbContext.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        #endregion

    }
}
