using CheeseMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CheeseMVC.ViewModels
{
    public class AddMenuItemViewModel
    {
        public int CheeseID { get; set; }
        public int MenuID { get; set; }
                
        public Menu Menu { get; set; }
        public List<SelectListItem> Cheese { get; set; }

        public AddMenuItemViewModel() { }

        public AddMenuItemViewModel(IEnumerable<Cheese> cheeses, Menu menu)
        {
            this.Menu = menu;

            Cheese = new List<SelectListItem>();

            foreach (var cheese in cheeses)
            {
                Cheese.Add(new SelectListItem
                {
                    Value = cheese.ID.ToString(),
                    Text = cheese.Name
                });
            };

           
           

        }


    }
}
