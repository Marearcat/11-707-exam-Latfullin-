using Exam2.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Exam2.Services
{
    public class CartService
    {
        ApplicationDbContext context;
        public CartService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void Clean(string userId)
        {
            if(context.Carts.Any(x => x.UserId == userId))
            {
                var cart = context.Carts.First(x => x.UserId == userId);
                cart.Cost = 0;
                context.Carts.Update(cart);
                context.DishesToCart.RemoveRange(context.DishesToCart.Where(x => x.CartId == cart.Id));
                context.SaveChanges();
            }
        }
    }
}
