using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Contract
{
    public interface IEmployeeRepository
    {
        // public Task<T> Create(T _object);

        //public void Update(T _object);

        //public IEnumerable<T> GetAll();

        //public T GetById(int Id);

        //public void Delete(T _object);
        Task<List<MstDepartment>> GetDepartments();
        Task<List<MstDepartment>> GetDepById(int DepId);
       
        Task<MstDepartment> AddDepartment(DepartmentDto post);
         Task<MstDepartment> UpdateDepartment(DepartmentDto post);
        Task<List<EmployeeDto>> GetEmployees();
        Task<List<EmployeeDto>> GetEmpById(int EmpId);
        Task<List<EmployeeDto>> GetBySearch(int EmployeeId, string DepName, string FirstName, string LastName);
        Task<EmployeeDetail> AddEmployee(EmployeeDto post);
        Task<EmployeeDetail> UpdateEmployee(EmployeeDto post);
        Task<int> DeleteEmployee(int? empId);

    }
}
