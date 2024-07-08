using AutoMapper;
using BLL.Interfaces;
using BLL.Models.DTO.ApplicationUser;
using BLL.Services;
using DAL.Domain;
using DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetProjectMVCElLibrary.ViewModel.Authorization;
using PetProjectMVCElLibrary.ViewModel.Login;

namespace PetProjectMVCElLibrary.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(AppDbContext context, SignInManager<ApplicationUser> signInManager, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _applicationUserService = new ApplicationUserService(context);
            _signInManager = signInManager;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;

        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUserDTO user = await _applicationUserService.GetUserByEmail(model.Email ?? string.Empty);
                if (user != null)
                {
                    ApplicationUser applicationUser = new ApplicationUser();
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(applicationUser, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel.Email), "Неверный email или пароль");
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.MaxLengthName = false;
                model.UniqueName = false;
                if (model.UserName != null)
                {
                    IEnumerable<ApplicationUserDTO> users = await _applicationUserService.GetAllUsers();
                    var sortUsers = users.Where(x => x.UserName == model.UserName);
                    if (sortUsers.Any())
                    {
                        model.UniqueName = true;
                        return View(model);
                    }
                    if (model.UserName.Length > 20)
                    {
                        model.MaxLengthName = true;
                        return View(model);
                    }
                    ApplicationUserDTO userDTO = new ApplicationUserDTO()
                    { 
                        UserEmail = model.UserName,
                        UserName = model.UserName,
                    };
                    //_applicationUserService.CreateUser(userDTO);
                    //ApplicationUser user = new() { UserName = model.UserName, Email = model.Email, CreateOn = DateTime.Now };
                    //var result = await userManager.CreateAsync(user, model.Password);
                    //if (result.Succeeded)
                    //{
                    //    await signInManager.SignInAsync(user, false);
                    //    return RedirectToAction("Index", "Home");
                    //}
                    //else
                    //{
                    //    foreach (var error in result.Errors)
                    //    {
                    //        ModelState.AddModelError(string.Empty, error.Description);
                    //    }
                    //}
                }
            }
            return View(model);
        }
    }
}
