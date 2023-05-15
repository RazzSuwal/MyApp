using Microsoft.AspNetCore.Mvc;
using MyApp.DataAccessLayer;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.Models.ViewModels;
using MyCommonHelper;
using Microsoft.AspNetCore.Authorization;

namespace MyAppWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = WebsiteRole.Role_Admin)]
    public class ProductController : Controller
    {
        public readonly IUnitOfwork _unitOfwork;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public object Product { get; private set; }
        public IEnumerable<SelectListItem> Categories { get; private set; }

        public ProductController(IUnitOfwork unitOfwork, IWebHostEnvironment hostingEnvironment)
        {
            _unitOfwork = unitOfwork;
            _hostingEnvironment = hostingEnvironment;
        }

        #region ApiiCALL
        public IActionResult AllProducts()
        {
            var products = _unitOfwork.Product.GetAll(includeProperties:"Category");
            return Json(new { data = products }); //data transfer through JASON into web page
        }
        #endregion
        public IActionResult Index()
        {
            //ProductVM productVM = new ProductVM();
            //productVM.Products = _unitOfwork.Product.GetAll();
            return View();
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
            ProductVM vm = new ProductVM()
            {
                Product = new(),
                Categories = _unitOfwork.Category.GetAll().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null || id == 0)
            {
                return View(vm);
            }
            else
            {
                vm.Product = _unitOfwork.Product.GetT(x => x.Id == id);
                if (vm.Product == null)
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
        public IActionResult CreateUpdate(ProductVM vm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                //for file upload
                string fileName = String.Empty;
                if (file != null)
                {
                    string uploadDir = Path.Combine(_hostingEnvironment.WebRootPath, "ProductImage");
                    fileName = Guid.NewGuid().ToString()+"-"+file.FileName;
                    string filePath = Path.Combine(uploadDir, fileName);

                    if (vm.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, vm.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(filePath,FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    vm.Product.ImageUrl = @"\ProductImage\" + fileName;
                }
                if(vm.Product.Id == 0) 
                { 
                    _unitOfwork.Product.Add(vm.Product);
                    TempData["success"] = "Product Added!";
                }
                else
                {
                    _unitOfwork.Product.Update(vm.Product);
                    TempData["success"] = "Product Update Done!";
                }
                _unitOfwork.Save();
                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        //Dlete done throght jason and api which we create

        #region DeleteAPICall
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var product = _unitOfwork.Product.GetT(x => x.Id == id);
            if (product == null)
            {
                return Json(new {success=false,message="Error in Fetching Data" });
            }
            else
            {
                var oldImagePath = Path.Combine(_hostingEnvironment.WebRootPath, product.ImageUrl.TrimStart('\\'));
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
                _unitOfwork.Product.Delete(product);
                _unitOfwork.Save();
                return Json(new { success = true, message = "Product Deleted" });
            }
        }
        #endregion
    }
}
