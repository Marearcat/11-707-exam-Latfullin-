using Exam2.Data;
using Exam2.Models;
using Exam2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Controllers
{
    public class DishController : Controller
    {
        readonly ApplicationDbContext context;

        public DishController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index(string restId)
        {
            ViewData["RestId"] = restId;
            return View(context.Dishes.Where(x => x.RestId == restId));
        }

        [HttpGet]
        public IActionResult Create(string restId)
        {
            ViewData["RestId"] = restId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Dish dish)
        {
            context.Dishes.Add(dish);
            context.SaveChanges();
            return RedirectPermanent("~/Dish/Index?restId=" + dish.RestId);
        }

        [HttpGet]
        public IActionResult Info(string id)
        {
            var dish = context.Dishes.First(x => x.Id == id);
            var model = new DishView()
            {
                Name = dish.Name,
                Cost = dish.Cost,
                Desc = dish.Description,
                Rest = context.Restaurants.First(x => x.Id == dish.RestId).Name
            };
            return View(model);
        }
    }
}
