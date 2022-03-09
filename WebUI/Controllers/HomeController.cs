using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _Configure;
        private string apiBaseUrl;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _Configure = configuration;

            apiBaseUrl = _Configure.GetValue<string>("WebAPIBaseUrl");
        }

        #region Employee
        public async Task<IActionResult> Index()
        {
            List<Employee> empInfo = new List<Employee>();
            List<Department> depInfo = new List<Department>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetEmployees");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    empInfo = JsonConvert.DeserializeObject<List<Employee>>(depResponse);
                }

                //returning the employee list to view
                return View(Tuple.Create<Employee, IEnumerable<Employee>>(new Employee(), empInfo));
            }
        }

       
        public async Task<IActionResult> Search()
        {
            List<Employee> emp = new List<Employee>();
           
            string EmpId = HttpContext.Request.Form["EmployeeId"].ToString();
            string FName= HttpContext.Request.Form["FirstName"].ToString();
            string LName= HttpContext.Request.Form["LastName"].ToString();
            string DepName= HttpContext.Request.Form["Department"].ToString();
            if (EmpId== string.Empty && FName== string.Empty && LName== string.Empty && DepName==string.Empty)
            {
             return RedirectToAction("Index");

            }
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetBySearch?EmpId=" + EmpId+ "&FName="+FName + "&LName=" + LName + "&DepName=" + DepName);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    emp = JsonConvert.DeserializeObject<List<Employee>>(depResponse);
                }
                //returning the employee list to view
               
                return View("Index",Tuple.Create<Employee, IEnumerable<Employee>>(new Employee(), emp));
            }
        }

        public async Task<IActionResult> EmployeeList()
        {
            List<Employee> empInfo = new List<Employee>();
            List<Department> depInfo = new List<Department>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetEmployees");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    empInfo = JsonConvert.DeserializeObject<List<Employee>>(depResponse);
                }

                //returning the employee list to view
                return View(empInfo);
            }
        }
        public async Task<IActionResult> AddOrEdit(int? employeeId)
        {
            List<Employee> emp = new List<Employee>();
            List<Department> dep = new List<Department>();
            
            ViewBag.PageName = employeeId == null ? "Create Employee" : "Edit Employee";
            ViewBag.IsEditEmp = employeeId == null ? false : true;
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res1 = await client.GetAsync("api/Employee/GetDepartments");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res1.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res1.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    dep = JsonConvert.DeserializeObject<List<Department>>(depResponse);
                }
               
            }
            if (employeeId == null)
            {
                Employee em = new Employee();
                em.DepartmentList = dep;
                return View(em);
            }
            else
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/Employee/GetEmpById?EmpId=" + employeeId);
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var depResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        emp = JsonConvert.DeserializeObject<List<Employee>>(depResponse);
                    }
                    //returning the employee list to view
                    emp[0].DepartmentList = dep;
                    return View(emp[0]);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int employeeId, [Bind("EmployeeId,FirstName,LastName,EmailId,DepartmentId,ManagerId")] Employee obj)
        {


            if (ModelState.IsValid)
            {
                try
                {

                    Employee dep = new Employee();
                    using (var client = new HttpClient())
                    {
                        //Passing service base url
                        client.BaseAddress = new Uri(apiBaseUrl);
                        client.DefaultRequestHeaders.Clear();
                        //Define request data format
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                        if (employeeId > 0)
                        {
                            HttpResponseMessage Res = await client.PostAsJsonAsync("api/Employee/UpdateEmployee", obj);

                            //Checking the response is successful or not which is sent using HttpClient
                            if (Res.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api
                                var depResponse = Res.Content.ReadAsStringAsync().Result;
                                //Deserializing the response recieved from web api and storing into the Employee list
                                dep = JsonConvert.DeserializeObject<Employee>(depResponse);
                            }
                        }
                        else
                        {
                            HttpResponseMessage Res = await client.PostAsJsonAsync("api/Employee/AddEmployee", obj);

                            //Checking the response is successful or not which is sent using HttpClient
                            if (Res.IsSuccessStatusCode)
                            {
                                //Storing the response details recieved from web api
                                var depResponse = Res.Content.ReadAsStringAsync().Result;
                                //Deserializing the response recieved from web api and storing into the Employee list
                                dep = JsonConvert.DeserializeObject<Employee>(depResponse);
                            }
                        }

                    }
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    throw;
                }

            }
            return View(obj);
        }

        // GET: Employees/Delete/1
        public async Task<IActionResult> Delete(int? employeeId)
        {
            List<Employee> dep = new List<Employee>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetEmpById?EmpId=" + employeeId);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    dep = JsonConvert.DeserializeObject<List<Employee>>(depResponse);
                }
                //returning the employee list to view
                return View(dep[0]);
            }
        }

        // POST: Employees/Delete/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmp(int? employeeId)
        {
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.PostAsJsonAsync("api/Employee/DeleteEmployee", employeeId);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                   
                }
               
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion Employee


        #region Department
        public async Task<IActionResult> Department()
        {

            List<Department> depInfo = new List<Department>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                HttpResponseMessage Res = await client.GetAsync("api/Employee/GetDepartments");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var depResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Employee list
                    depInfo = JsonConvert.DeserializeObject<List<Department>>(depResponse);
                }
                //returning the employee list to view
                return View(depInfo);
            }
        }

        public async Task<IActionResult> AddOrEditDep(int? id)
        {
            List<Department> dep = new List<Department>();
            ViewBag.PageName = id == null ? "Create Department" : "Edit Department";
            ViewBag.IsEdit = id == null ? false : true;
            if (id == null)
            {
                return View();
            }
            else
            {
                using (var client = new HttpClient())
                {
                    //Passing service base url
                    client.BaseAddress = new Uri(apiBaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    //Define request data format
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    HttpResponseMessage Res = await client.GetAsync("api/Employee/GetDepById?DepId="+ id);
                    //Checking the response is successful or not which is sent using HttpClient
                    if (Res.IsSuccessStatusCode)
                    {
                        //Storing the response details recieved from web api
                        var depResponse = Res.Content.ReadAsStringAsync().Result;
                        //Deserializing the response recieved from web api and storing into the Employee list
                        dep = JsonConvert.DeserializeObject<List<Department>>(depResponse);
                    }
                    //returning the employee list to view
                    return View(dep[0]);
                }
            }
        }

        //AddOrEdit Post Method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditDep(int departmentId, [Bind("DepartmentId,DepartmentName")]Department obj)
        {
           

            if (ModelState.IsValid)
            {
                try
                {

                    Department dep = new Department();
                    using (var client = new HttpClient())
                    {
                        //Passing service base url
                        client.BaseAddress = new Uri(apiBaseUrl);
                        client.DefaultRequestHeaders.Clear();
                        //Define request data format
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Sending request to find web api REST service resource GetAllEmployees using HttpClient
                    if (departmentId > 0)
                    {
                        HttpResponseMessage Res = await client.PostAsJsonAsync("api/Employee/UpdateDepartment", obj);

                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            var depResponse = Res.Content.ReadAsStringAsync().Result;
                            //Deserializing the response recieved from web api and storing into the Employee list
                            dep = JsonConvert.DeserializeObject<Department>(depResponse);
                        }
                    }
                    else
                    {
                        HttpResponseMessage Res = await client.PostAsJsonAsync("api/Employee/AddDepartment", obj);

                        //Checking the response is successful or not which is sent using HttpClient
                        if (Res.IsSuccessStatusCode)
                        {
                            //Storing the response details recieved from web api
                            var depResponse = Res.Content.ReadAsStringAsync().Result;
                            //Deserializing the response recieved from web api and storing into the Employee list
                            dep = JsonConvert.DeserializeObject<Department>(depResponse);
                        }
                    }
                           
                    }
                    return RedirectToAction(nameof(Department));

                }
              catch (Exception ex)
              {
                  throw;
              }
             
            }
            return View(obj);
        }

        #endregion Department

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
