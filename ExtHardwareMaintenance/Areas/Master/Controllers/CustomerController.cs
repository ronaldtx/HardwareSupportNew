using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExtHardwareMaintenance.Models;
using Ext.Net;
using Ext.Net.MVC;

namespace ExtHardwareMaintenance.Areas.Master.Controllers
{
    public class CustomerController : Controller
    {
        //
        // GET: /Master/Customer/

        public ActionResult Index()
        {
            return View(Customers.GetAllCustomer());
        }

        public ActionResult SaveData(FormCollection isi)
        {
            String err = Customers.SaveData(isi);

            if (err == "")
            {
                FormPanelResult r = new FormPanelResult();
                r.Script = string.Format("UpdateGrid();");
                return r;
            }
            else
            {
                X.Msg.Notify(new NotificationConfig
                {
                    Icon = Icon.Error,
                    Title = "Error",
                    Html = err
                }).Show();
                return this.Direct();
            }

        }

        public ActionResult SaveDataEdit(FormCollection isi)
        {
            String err = Customers.SaveDataEdit(isi);

            if (err == "")
            {
                FormPanelResult r = new FormPanelResult();
                r.Script = string.Format("UpdateGrid();");
                return r;
            }
            else
            {
                X.Msg.Notify(new NotificationConfig
                {
                    Icon = Icon.Error,
                    Title = "Error",
                    Html = err
                }).Show();
                return this.Direct();
            }

        }

        public ActionResult SaveDataDelete(int Id)
        {
            Customers.DataDelete(Id);

            X.Reload();
            AjaxResult r = new AjaxResult();
            return r;

        }
        public ActionResult GetData()
        {
            return this.Store(Customers.GetAllCustomer());
        }

    }
}
