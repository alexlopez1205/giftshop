using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using giftshop.Models;
using giftshop.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Web.Security;
using System.Collections;

namespace giftshop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        

        public ActionResult Login() {
            FormsAuthentication.SignOut();
            SessionVar.reset();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateInput(true)]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(LoginPageViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ErrorMessage = "Please type your username";
                return PartialView(viewModel);
            }
            if (string.IsNullOrEmpty(viewModel.Username)
                || string.IsNullOrEmpty(viewModel.Password))
            {
                viewModel.ErrorMessage = "Please type your password";
                return PartialView(viewModel);
            }

            
            claseDBSqlServer oDB = new claseDBSqlServer();
            oDB.Procedure = "gs_sp_login";
            oDB.AddParameter("@username", viewModel.Username, ParameterDirection.Input);
            oDB.AddParameter("@password", viewModel.Password, ParameterDirection.Input);
            oDB.AddParameter("@idrol", 0, ParameterDirection.InputOutput);
            oDB.AddParameter("@iduser", 0, ParameterDirection.InputOutput);
            oDB.ExecuteProcedureNonQuery();

            int idrol = Convert.ToInt16(oDB.GetParameter("@idrol"));
            int iduser = Convert.ToInt16(oDB.GetParameter("@iduser"));

            oDB.Dispose();




            if (iduser != 0)
            {
                SessionVar.Username = viewModel.Username;
                SessionVar.idrol = idrol;
                SessionVar.iduser = iduser;
                FormsAuthentication.SetAuthCookie(SessionVar.Username, false);

                return RedirectToAction("Index", "Home");

            }
            viewModel.ErrorMessage = "Login o contraseña incorrecta";

            return PartialView(viewModel);
        }

        public JsonResult ObtenerMenu()
        {
            int _id_rol = Convert.ToInt16(SessionVar.idrol);
            ArrayList data = new ArrayList();
            claseDBSqlServer oDB = new claseDBSqlServer();
            JsonResult result = new JsonResult();

            try
            {
                oDB.ClearDataListArray();
                oDB.Procedure = "gs_sp_obt_menu";
                oDB.AddParameter("@IDROL", _id_rol);
                oDB.AddDataListArray(oDB.ExecuteProcedureDataList(), "opciones_menu");
                data.AddRange(oDB.DataListArray);
                result = Json(data);
                oDB.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                return Json("Error: " + ex);
            }
        }
    }
}