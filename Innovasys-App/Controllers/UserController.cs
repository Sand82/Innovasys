using Microsoft.AspNetCore.Mvc;

using Innovasys_App.Models.Views;
using Innovasys_App.Services.UserService;

namespace Innovasys_App.Controllers
{
    public class UserController : Controller
    {
        private IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> LoadData()
        {
            await userService.LoadData();

            var models = userService.GetData();
            
            return View(models);
        }

        [HttpPost]
        public async Task<IActionResult> Add(List<UserViewModel> model)
        {
            var (success, message) = await userService.EditData(model);

            TempData["ResultMessage"] = message;
            TempData["IsSuccess"] = success;

            return RedirectToAction("Index", "Home");
        }
    }
}
