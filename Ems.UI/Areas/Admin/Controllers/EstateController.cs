using Ems.BusinessLogic.Abstract;
using Ems.BusinessLogic.Dtos;
using Ems.DataAccessLayer.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Ems.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EstateController : Controller
    {
        
        private readonly IEstateService _estateService;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EstateController(IEstateService stateService, ICategoryRepository categoryRepository, IWebHostEnvironment webHostEnvironment)
        {
            _estateService = stateService;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Property owner")]
        public async Task<IActionResult> Index()
        {
            var categoires = await _categoryRepository.GetCategoryDictionaryAsync();
            TempData["Categories"] = new SelectList(categoires, "Key", "Value");
            AddEstateDto addEstateDto = new();
            return View(addEstateDto);
        }

        [HttpPost]
        [Authorize(Roles = "Property owner")]
        public async Task<IActionResult> AddEstate(AddEstateDto estateDto, IList<IFormFile> imageFile)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            estateDto.UserId = userId;

            
            estateDto.UploadedFilesDtos = new List<UploadedFileDto>();

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (Directory.Exists(uploadsFolder) is false)
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            foreach (var img in imageFile)
            {
                if (img.Length > 0)
                {
                    string uniqueImageName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(img.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueImageName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await img.CopyToAsync(fileStream);
                    }

                    estateDto.UploadedFilesDtos.Add(new UploadedFileDto
                    {
                        FileName = uniqueImageName,
                        ContentType = img.ContentType,
                        RelativePath = "/uploads/" + uniqueImageName,
                    });
                }
            }

            var result = await _estateService.AddAsync(estateDto);

            if (result.ResponseType == Core.Enums.ResponseType.ValidationError)
            {
                foreach (var item in result.ResponseValidationResults)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }

                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, errors });
            }

            return RedirectToAction("Index", "Estate");
        }



        [HttpGet]
        [Authorize(Roles = "Property owner")]
        public async Task<IActionResult> GetEstate()
        {
            var result = await _estateService.GetAllAsync();
            foreach (var estate in result.Data)
            {
                Console.WriteLine($"Image Path: {estate.Image}");
            }
            return Json(result.Data);
        }


        [HttpDelete]
        [Authorize(Roles = "Property owner")]
         public async Task<IActionResult> RemoveEstate(int id)
         {
            var result = await _estateService.RemoveAsync(id);
            if (result.ResponseType == Core.Enums.ResponseType.NotFound)
            {
                return Json(new { success = false, message = result.Message });
            }
         
            return Json(new { success = true, message = "Estate was deleted successfully" });
         }



        [HttpPost]
        [Authorize(Roles = "Property owner")]
        public async Task<IActionResult> UpdateEstate(int id)
        {
            var result = await _estateService.UpdateAsync(id);
            if (result.ResponseType == Core.Enums.ResponseType.NotFound)
            {
                return Json(new { success = false, message = result.Message });
            }

            return Json(new { success = true, message = "Estate was updated successfully" });
        }

    }
}
