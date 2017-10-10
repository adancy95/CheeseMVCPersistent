using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using Microsoft.EntityFrameworkCore;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CheeseMVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CheeseDbContext context;
        
        public CategoryController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            //retrieves a list of all categories from the CheeseDbContext
            List<CheeseCategory> categories= context.Categories.ToList();

            //passes the list into the view
            return View(categories);
        }

        
        public IActionResult Add()
        {
            AddCategoryViewModel addCategoryViewModel = new AddCategoryViewModel();
         
                
            return View(addCategoryViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
        {
            //Check if model is valid
            if (ModelState.IsValid)
            {
                //if valid create a new cheeseCategory object
                CheeseCategory newCategory = new CheeseCategory
                {
                    Name = addCategoryViewModel.Name
                };

                //Add the new category to the database
                context.Categories.Add(newCategory);
                //Save the changes made
                context.SaveChanges();

                return Redirect("/Category");
            }
            //if not valid, then reload the page
            else
            {
                return View(addCategoryViewModel);
            }
            
        }
    }
}
