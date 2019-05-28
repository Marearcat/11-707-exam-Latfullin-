using Exam2.Data;
using Exam2.Models;
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
            return RedirectPermanent("~/Dish/Index");
        }
    }
}
