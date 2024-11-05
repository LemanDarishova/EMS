
using Ems.BusinessLogic.Abstract;
using Ems.BusinessLogic.Dtos;
using Ems.Core.Helpers;
using Ems.ExternalServices.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Ems.Core.Enums;

namespace Ems.UI.Areas.Account.Controllers;
[Area("Auth")]

public class AccountController : Controller
{
    private readonly IUserService _userService;
    public readonly IEmailService _emailService;

    public AccountController(IUserService userService,
                             IEmailService emailService)
    {
        _userService = userService;
        _emailService = emailService;
    }

    public async Task<IActionResult> Register()
    {
        CreateUserDto userDto = new();
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

        var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                new Claim(ClaimTypes.Name, result.Data.FirstName),
                new Claim(ClaimTypes.Surname, result.Data.LastName),
                //new Claim(ClaimTypes.Role, "Admin"),
                //new Claim(ClaimTypes.Role, "Agency"),
                new Claim(ClaimTypes.Role, "Property owner")

        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties()
        {
            ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
        };

        try
        {
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

        }
        catch (Exception e)
        {
            var x = e;
            throw;
        }

        return Json(new { success = true });
    }

    [HttpGet]

    public async Task<IActionResult> ResetPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(IdentifyNewPassDto identifyNewPassDto)
    {
        var result = await _userService.UpdatePasswordAsync(identifyNewPassDto);
        if (result.ResponseType == ResponseType.SuccessResult)
        {
            ViewBag.Message = "Password successfully updated.";
            return Redirect("/Auth/Account/SignIn");
        }
        else
        {
            ModelState.AddModelError("", "Kod və ya şifrə yanlışdır.");
            return Redirect("/Auth/Account/ResetPassword");
        }
    }

}
