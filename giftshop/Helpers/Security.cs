using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data;
using System.Text;

using giftshop.Models;


namespace giftshop.Helpers
{
    public class claseSeguridad : IDisposable
    {
        private bool disposed = false;

        public claseSeguridad()
        {
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                //en caso de utilizar variables globales limpialas aquí...
            }
            disposed = true;
        }

        ~claseSeguridad()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool Autorizacion(int id_menu)
        {

            JsonResult result = new JsonResult();
            ArrayList data = new ArrayList();
            int _id_rol = Convert.ToInt16(SessionVar.idrol);
            claseDBSqlServer oDB = new claseDBSqlServer();
            DataRow row;

            try
            {
                oDB.ClearDataListArray();
                oDB.Procedure = "gs_sp_obt_aut_menu";
                oDB.AddParameter("@idrol", _id_rol);
                oDB.AddParameter("@idmenu", id_menu);
                row = oDB.ExecuteProcedureDataRow();
                oDB.Dispose();

                if (!(row == null))
                {
                    if (row["coincidencias"].ToString() == "0")
                    {
                        return false;
                    }
                    else if (row["coincidencias"].ToString() == "1")
                    {
                        return true;
                    }
                }
                oDB.Dispose();
                return false;
            }
            catch (Exception e)
            {
                oDB.Dispose();
                return false;
            }


        }
    }
}