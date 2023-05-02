using Microsoft.AspNetCore.Mvc;
using MovieStoreApp.Models.BLL;

using MovieStoreApp.Models.DTO;

namespace MovieStoreApp.Controllers
{
    public class CategoriesController : BaseController
    {
        private static List<Category> _categories=new List<Category>();
        public IActionResult Index()
        {
            return View(CategoriesManager.GetCategories());
        }
        [HttpGet]
        public IActionResult Create() {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            return View(new Category());
        }
        [HttpPost]  
        public IActionResult Create(Category category) {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            try { 
            if (ModelState.IsValid)
            {
                CategoriesManager.AddNewCategory(category);
                return RedirectToAction(nameof(Index));
            }
        }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"{ex.Message}");
                
            }
        return View(category);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            Category categoryToEdit = CategoriesManager.GetCategorById(id);

            if (categoryToEdit != null)
            {
                return View(categoryToEdit);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Edit(int id ,Category model)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();
         
            try
            {
            var  editedModel = CategoriesManager.EditCategory(id, model);
                if (editedModel != null)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Data did not saved");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"{ex.Message}");
                
            }
          
            return View(model);
        }
       
        [HttpGet]
        public IActionResult Details(int id)
        {
            var category = CategoriesManager.GetCategorById(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            var category = CategoriesManager.GetCategorById(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            if (!IsAdmin)
                return this.RedirectToHomePage();
            var category = CategoriesManager.GetCategorById(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoriesManager.DeleteCategory(id);
            return RedirectToAction("Index");
        }

     

    }
}
