using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExtHardwareMaintenance.Models;
using Core.DomainModel;
using Ext.Net;
using Ext.Net.MVC;

namespace ExtHardwareMaintenance.Areas.Master.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /Master/User/

        public ActionResult Index()
        {
            return View(Users.GetAllUser());
        }

        public ActionResult SaveData(FormCollection isi)
        {
            Dictionary<String, String> err = Users.SaveData(isi);

            if (err == null)
            {
                FormPanelResult r = new FormPanelResult();
                r.Script = string.Format("UpdateGrid();");

                return r;
            }
            else
            {
                /*
                X.Msg.Notify(new NotificationConfig
                {
                    Icon = Icon.Error,
                    Title = "Error",
                    Html = err
                }).Show();
                return this.Direct();
                */
                this.GetCmp<FormPanel>("myFormPanel").MarkInvalid(err);
                return this.Direct();
            }
        }

        public ActionResult SaveDataEdit(FormCollection isi)
        {
            String err = Users.SaveDataEdit(isi);

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
            Users.DataDelete(Id);

            String r = "Berhasil";
            return Content(r);

        }
        public ActionResult GetData()
        {
            return this.Store(Users.GetAllUser());
        }
        public ActionResult GetPlants(int start, int limit, int page, string query)
        {
            Paging<DbUser> users = Users.UsersPaging(start, limit, "", "", query);

            return this.Store(users.Data, users.TotalRecords);
        }

    }
}
