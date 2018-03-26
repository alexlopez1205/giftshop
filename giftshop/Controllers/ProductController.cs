using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using giftshop.Models;
using giftshop.Helpers;

namespace giftshop.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        claseSeguridad cs = new claseSeguridad();
        bool autorizado = false;
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult Administration() {
            autorizado = cs.Autorizacion(2);
            if (!(autorizado)) return RedirectToAction("Index", "Home");
            else return View();

        }

        public ActionResult getProducts() {
            using (GiftshopEntities gs = new GiftshopEntities()) {
                return Json(gs.gs_t_cat_products.ToList());
            }
        }

    }
}