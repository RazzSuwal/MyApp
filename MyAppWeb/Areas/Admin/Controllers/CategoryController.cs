using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using MyApp.Models.ViewModels;
using MyCommonHelper;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebsiteRole.Role_Admin)]
    public class CategoryController : Controller
    {
        public readonly IUnitOfwork _unitOfwork;
        public CategoryController(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        public IActionResult Index()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.categories = _unitOfwork.Category.GetAll();
            return View(categoryVM);
        }

        //[HttpGet]
        //public IActionResult Create()
        //{

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Create(Category category)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfwork.Category.Add(category);
        //        _unitOfwork.Save();
        //        TempData["success"] = "Category Create Sucessfully!";
        //        return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}

        [HttpGet]
        public IActionResult CreateUpdate(int? id)
        {
            CategoryVM vm = new CategoryVM();
            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Category = _unitOfwork.Category.GetT(x => x.Id == id);
                if (vm.Category == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(vm);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUpdate(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                var categories = _unitOfwork.Category.GetAll().ToList();

                // Sort categories by name using Bubble Sort algorithm
                SortingHelper.BubbleSortByName(categories);

                 // Check if a category with the same name already exists
                if (!SortingHelper.BinarySearchByName(categories, vm.Category.Name))
                {
                    if (vm.Category.Id == 0)
                    {
                        _unitOfwork.Category.Add(vm.Category);
                    }
                    else
                    {
                        _unitOfwork.Category.Update(vm.Category);
                    }
                    _unitOfwork.Save();
                    TempData["success"] = "Category Updated Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = "Category with the same name already exists!";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _unitOfwork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteData(int? id)
        {
            var category = _unitOfwork.Category.GetT(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfwork.Category.Delete(category);
            _unitOfwork.Save();
            TempData["error"] = "Product Deleted Done!";
            return RedirectToAction("Index");
        }
    }
}
