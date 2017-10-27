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
    public class MenuController : Controller
    {
        private readonly CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            //retrieves a list of all Menus from the CheeseDbContext
            IList<Menu> menus = context.Menus.ToList();


            // sends the list of all menus to the view page
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel addMenuViewModel = new AddMenuViewModel();

            return View(addMenuViewModel);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                // Create a new menu object in the database
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };

                // Add that new menu object to the database
                context.Menus.Add(newMenu);

                //save the changes that you made to the database
                context.SaveChanges();

                Menu lastmenu = context.Menus.Last();

                return Redirect("/ViewMenu/" + lastmenu.ID);
            }
            else
            {
                return View(addMenuViewModel);
            }

        }

        public IActionResult ViewMenu(int id)
        {
            // get the menu of the given id
            Menu theMenu = context.Menus.Find(id);
            
            //get the items associated with a particular menu
            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Where(cm => cm.MenuID == id)
                .ToList();

            // build a viewmenuviewmodel and pass it into the view.
            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = theMenu,
                Items = items
            };
            return View(viewMenuViewModel);
        }

        public IActionResult AddMenuItem(int id)
        {
            Menu theMenu = context.Menus.Single(m => m.ID == id);
            List<Cheese> cheeses = context.Cheeses.ToList();


            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(cheeses, theMenu);

            ViewBag.ID = id;


            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddMenuItem(AddMenuItemViewModel addMenuItemViewModel)
        {
            if (ModelState.IsValid)
            {
                var cheeseID = addMenuItemViewModel.CheeseID;
                var menuID = addMenuItemViewModel.MenuID;

                IList<CheeseMenu> existingItems = context.CheeseMenus
                                                    .Where(cm => cm.CheeseID == cheeseID)
                                                     .Where(cm => cm.MenuID == menuID).ToList();

                if ( existingItems.Count == 0)
                {
                    CheeseMenu menuItem = new CheeseMenu
                    {
                        Cheese = context.Cheeses.Single(c => c.ID == cheeseID),
                        Menu = context.Menus.Single(m => m.ID == menuID)

                    };

                    context.CheeseMenus.Add(menuItem);
                    context.SaveChanges();
                     
                }

                return Redirect(string.Format("/Menu/ViewMenu/{0}", addMenuItemViewModel.MenuID));
            }
            return View(addMenuItemViewModel);
        }
    }
}

