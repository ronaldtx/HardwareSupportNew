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
    public class ItemTypeController : Controller
    {
        //
        // GET: /Master/ItemType/

        public ActionResult Index()
        {
            return View(ItemTypes.GetAllItemType());
        }

        public ActionResult SaveData(FormCollection isi)
        {
            String err = ItemTypes.SaveData(isi);

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
            String err = ItemTypes.SaveDataEdit(isi);

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
            ItemTypes.DataDelete(Id);

            X.Reload();
            AjaxResult r = new AjaxResult();
            return r;

        }
        public ActionResult GetData()
        {
            return this.Store(ItemTypes.GetAllItemType());
        }

    }
}
