using Microsoft.AspNetCore.Mvc;
using NexusApp.Constants;
using NexusApp.Data;
using NexusApp.ModelDTOs;
using AutoMapper;
using NexusApp.Repository;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Employee.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using NexusApp.Areas.Financial.Reposetory.Payment;


namespace NexusApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginrepository;
        private readonly IConfiguration _configuration;
        private readonly IPaymentReposetory paymentReposetory;

        public LoginController(ApplicationDbContext context, IMapper mapper, ILoginRepository loginRepository, IConfiguration configuration, IPaymentReposetory _paymentReposetory)
        {
            _context = context;
            _mapper = mapper;
            _loginrepository = loginRepository;
            _configuration = configuration;
            paymentReposetory = _paymentReposetory;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CheckLogin(string Email, LoginDTOs loginDTOs)
        {

            try
            {
                if (ModelState.IsValid)
                {

                    if (loginDTOs.IsAdmin == true)
                    {

                        var user = Authenticate(loginDTOs);

                        if (user != null)
                        {
                            var token = GenerateToken(user);
                            HttpContext.Session.SetString("Email", Email);
                            HttpContext.Session.SetString("Role", user.Role);                         
                            var userToken = new EmployeeModel
                            {
                                Email = user.Email,
                                Name = user.Name,
                                Role = user.Role
                            };
                            return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
                        }
                        ModelState.AddModelError(string.Empty, "Password invalid");
                            return View("~/Views/Login/Index.cshtml");
                    }
                    else
                    {
                        var data = await _loginrepository.CheckLogin(loginDTOs.Email);
                        if(data != null)
                        {
                            var pay = await paymentReposetory.GetPaymentByCusId(data.CustomerId);
                            if (pay != null)
                            {
                                HttpContext.Session.SetInt32("PaymentId", pay.PaymentId);
                            }
                        }    
                        var dataDTOs = _mapper.Map<CustomerModel>(data);
                        if (dataDTOs == null)
                        {
                            ModelState.AddModelError(string.Empty, "Account was not actived");
                            return View("~/Views/Login/Index.cshtml");
                        }
                        else
                        {
                            if (loginDTOs.Password != data.Password)
                            {
                                ModelState.AddModelError(string.Empty, "Password invalid");
                                return View("~/Views/Login/Index.cshtml");
                            }
                            else
                            {
                                HttpContext.Session.SetString(ConstantService.SessionLogin, data.Email);
                                return RedirectToAction("Index", "Home");

                            }

                        }
                    }
                   
                }

            }
            catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View("~/Views/Login/Index.cshtml");
        }
        public IActionResult Logout()
        {
            _loginrepository.Checkout();
            return RedirectToAction("Index", "Home");

        }
        private EmployeeModel Authenticate(LoginDTOs userLogin)
        {
            var listUser = _context.Employees.ToList();
            if (listUser != null && listUser.Count > 0)
            {
                var currentUser = listUser.FirstOrDefault(u => u.Email.ToLower() == userLogin.Email && u.Password == userLogin.Password && u.Role !="");
                if(currentUser != null)
                {
                    return currentUser;
                }
            }
            return null;
        }
        //to generate token
        private string GenerateToken(EmployeeModel employee)
        {
            var securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Name",employee.Name),
                new Claim("Email",employee.Email),
                new Claim(ClaimTypes.Role,employee.Role)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
