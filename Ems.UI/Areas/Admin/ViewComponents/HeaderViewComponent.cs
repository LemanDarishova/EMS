using Microsoft.AspNetCore.Mvc;

namespace Ems.UI.Areas.Admin.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    }
}
