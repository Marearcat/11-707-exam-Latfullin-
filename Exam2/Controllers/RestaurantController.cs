using Exam2.Data;
using Exam2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Controllers
{
    public class RestaurantController : Controller
    {
        readonly ApplicationDbContext context;

        public RestaurantController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(context.Restaurants);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Restaurant restaurant)
        {
            context.Restaurants.Add(restaurant);
            context.SaveChanges();
            return RedirectToAction("Index", "Restaurant");
        }


    }
}
