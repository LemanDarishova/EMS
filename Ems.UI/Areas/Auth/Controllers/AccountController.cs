
using Ems.BusinessLogic.Abstract;
using Ems.BusinessLogic.Dtos;
using Ems.Core.Helpers;
using Ems.ExternalServices.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ems.Core.Enums;
using Ems.Core.Extensions;
using Ems.Core.Wrappers.Concrete;
using Ems.Core.Wrappers.Interfaces;
using Ems.DataAccessLayer.Abstract;
using Ems.DataAccessLayer.EntityFrameworkCore.Concrete;
using Ems.Entity.Accounds;
using System.Data;

namespace Ems.UI.Areas.Account.Controllers;
[Area("Auth")]

public class AccountController : Controller
{
    private readonly IUserService _userService;
    public readonly IEmailService _emailService;
    public readonly IUserRepository _userRepository;
    public readonly IRoleRepository _roleRepository;

    
    public AccountController(IUserService userService,
                             IEmailService emailService,
                             IUserRepository userRepository,
                             IRoleRepository roleRepository)
    {
        _userService = userService;
        _emailService = emailService;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<IActionResult> Register()
    {
        CreateUserDto userDto = new();
        var roles = await _roleRepository.GetAllAsync();

        if (roles == null || !roles.Any())
        {
            Console.WriteLine("No roles found.");
        }

        ViewBag.Roles = roles;
        return View(userDto);
    }


    [HttpPost]
    public async Task<IActionResult> Register([FromBody] CreateUserDto userDto)
    {
        var result = await _userService.CreateUserAsync(userDto);

        if (result.ResponseType == Core.Enums.ResponseType.ValidationError)
        {
            foreach (var item in result.ResponseValidationResults)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Json(new { success = false, errors });
        }

        string url = $"http://localhost:5197/auth/account/ConfirmEmail?code={result.Data.ConfirmCode}&userId={result.Data.Id}";
        var htmlMessage = HtmlTemplateGenerator.ConfirmMessage("Click button for complete email verification", url);
        await _emailService.SendEmailAsync(result.Data.Email, "Confirm password", htmlMessage);

        return View();
    }


    [HttpGet]

    public IActionResult EmailConfirmationPage()
    {
        return View();
    }


    [HttpGet]

    public async Task<IActionResult> ConfirmEmail(int code, int userId)
    {
        var result = await _userService.CheckConfirmCodeAsync(code, userId);
        if (result.Data)
        {
            await _userService.ChangeUserStatusAsync(Core.Enums.RegisterStatusEnum.Active, userId);
            return Redirect("/Auth/Account/EmailConfirmationPage");
        }

        return Redirect("/Auth/Account/Register");
    }


    [HttpGet]

    public IActionResult SignIn()
    {
        return View(new SigninUserDto());
    }


    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] SigninUserDto userDto)
    {

        var result = await _userService.CheckUserAsync(userDto);

        if (result.ResponseType == ResponseType.ValidationError)
        {
            foreach (var item in result.ResponseValidationResults)
            {
                ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
            }

            var errors = ModelState.Where(x => x.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Json(new { success = false, errors });
        }

        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                new Claim(ClaimTypes.Name, result.Data.FirstName),
                new Claim(ClaimTypes.Surname, result.Data.LastName),
                new Claim(ClaimTypes.Role, result.Data.Role.ToString()),

        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties()
        {
            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
        };

        try
        {

            Role role = await _roleRepository.GetRoleByNameAsync(result.Data.Role);
            string roleName = role?.RoleName;
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

            string redirectUrl = roleName switch
            {
                "Property owner" => "/Admin/Estate/Index",
                "Agency" => "/Admin/Estate/Index",
                "Admin" => "/Home/Index",
                _ => "/Home/Index" 
            };
            return Json(new { success = true, redirectUrl });
        }
        catch (Exception e)
        {
            var x = e;
            throw;
        }
        
    }



    [HttpGet]

    public async Task<IActionResult> ResetPassword()
    {
        return View();
    }


    [HttpPost]

    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        if (!ModelState.IsValid)
        {
            return View(resetPasswordDto);
        }

        var result = await _userService.ResetPasswordAsync(resetPasswordDto);
        if (result.ResponseType == ResponseType.NotFound)
        {
            ModelState.AddModelError(string.Empty, "Email not found.");
            return View(resetPasswordDto);
        }

        var resetToken = Guid.NewGuid().ToString();
        var resetLink = Url.Action("SignIn", "Account", new { token = resetToken, email = resetPasswordDto.Email }, Request.Scheme);
        await _emailService.SendPasswordResetEmailAsync(resetPasswordDto.Email, resetLink);
        ViewBag.Message = "Password reset link has been sent to your email.";
        return RedirectToAction("SignIn", "Account");
    }
}
