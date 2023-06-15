using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NexusApp.Areas.Customer.Models;
using NexusApp.Areas.Financial.Models;
using NexusApp.Areas.Financial.Reposetory.Payment;
using NexusApp.Data;
using NexusApp.ModelDTOs;
using NexusApp.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NexusApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IPaymentReposetory paymentReposetory;

        public UserController(IMapper mapper, IUserRepository userRepository, ApplicationDbContext context, IHttpContextAccessor _httpContextAccessor, IPaymentReposetory _paymentReposetory)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _context = context;
            httpContextAccessor = _httpContextAccessor;
            paymentReposetory = _paymentReposetory;
        }
        [HttpGet]
        public async Task<IActionResult> UserDetail()
        {
            var email = HttpContext.Session.GetString("Login");
            if (email != null)
            {
                var data = await _userRepository.GetUserByEmail(email);
                var dataDTOs = _mapper.Map<UserDetailDTOs>(data);
                return View(dataDTOs);
            }
            throw new Exception();
        }
        [HttpGet]
        public async Task<IActionResult> UserPayment()
        {
            var payID = httpContextAccessor.HttpContext.Session.GetInt32("PaymentId");
            var Model = await paymentReposetory.GetPaymentById(payID.Value);
            if (Model != null)
            {
                string paymentMode = "Online";
                Model.PaymentMode = paymentMode;
                ViewBag.PaymentMode = paymentMode;
                return View(Model);
            }
            return View();
        }
        public async Task<IActionResult> PaymentSuccess()
        {
            return View();

        }
        public async Task<IActionResult> PaymentFailed()
        {
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Update()
        {
            var email = HttpContext.Session.GetString("Login");
            if (email != null)
            {
                var data = await _userRepository.GetUserByEmail(email);
                var dataDTOs = _mapper.Map<UpdateCustomerDTO>(data);
                return View(dataDTOs);
            }
            throw new Exception();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDTO updateCustomerDTO)
        {
            var email = HttpContext.Session.GetString("Login");
            if (email != null)
            {
                var data = await _userRepository.GetUserByEmail(email);
                var dataDTOs = _mapper.Map<CustomerModel>(data);
                dataDTOs.Name = updateCustomerDTO.Name;
                dataDTOs.Phone = updateCustomerDTO.Phone;
                dataDTOs.BirthDay = updateCustomerDTO.Birthday;
                dataDTOs.Street = updateCustomerDTO.Street;
                dataDTOs.Ward = updateCustomerDTO.Ward;
                dataDTOs.District = updateCustomerDTO.District;
                dataDTOs.City = updateCustomerDTO.City;
                await _userRepository.Update(dataDTOs);
                return RedirectToAction("UserDetail", "User");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var email = HttpContext.Session.GetString("Login");
            var data = await _userRepository.GetUserByEmail(email);
            var dataDTOs = _mapper.Map<ChangePasswordDTOs>(data);
            dataDTOs.Id = data.CustomerId;
            return View("~/Views/Login/ChangePassword.cshtml", dataDTOs);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTOs model)
        {
            var email = HttpContext.Session.GetString("Login");
            var data = await _userRepository.GetUserByEmail(email);
            if (data.Password != model.OldPassword)
            {
                ModelState.AddModelError(string.Empty, "Old password is not matching");
                return View("~/Views/Login/ChangePassword.cshtml");
            }
            else
            {
                if (model.NewPassword != model.ConfirmPassword)
                {
                    ModelState.AddModelError(string.Empty, "New password is not matching with confirm password");
                    return View("~/Views/Login/ChangePassword.cshtml");
                }
                else
                {
                    var dataDtOs = _mapper.Map<CustomerModel>(data);
                    dataDtOs.Password = model.ConfirmPassword;
                    await _userRepository.ChangePasswold(dataDtOs);
                    var session = HttpContext.Session.GetString("Login");
                    if (session != null)
                    {
                        HttpContext.Session.Clear();

                    }
                    return RedirectToAction("Index", "Home");
                }
            }


        }
    }
}
