using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExtHardwareMaintenance.Models;
using Ext.Net;
using Ext.Net.MVC;
using Core.DomainModel;

namespace ExtHardwareMaintenance.Areas.Master.Controllers
{
    public class ItemController : Controller
    {
        //
        // GET: /Master/Item/

        public ActionResult Index()
        {
            return View(Items.GetAllItem());
        }

        public ActionResult SaveData(FormCollection isi)
        {
            String err = Items.SaveData(isi);

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
            String err = Items.SaveDataEdit(isi);

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
        public ActionResult DataDelete(int Id)
        {
            Items.DataDelete(Id);
            /*
            X.Reload();
            AjaxResult r = new AjaxResult();
            */
            String r = "Berhasil";
            return Content(r);

        }
        public ActionResult GetData()
        {
            return this.Store(Items.GetAllItem());
        }

        public ActionResult GetDataCustomer(int start, int limit, int page, string query)
        {
            //return this.Store(Customers.GetAllCustomer());
            Paging<Customer> customers = Customers.CustomersPaging(start, limit, "", "", query);

            return this.Store(customers.Data, customers.TotalRecords);
        }

        public ActionResult GetDataItemType(int start, int limit, int page, string query)
        {
            //return this.Store(ItemTypes.GetAllItemType());
            Paging<ItemType> itemTypes = ItemTypes.ItemTypesPaging(start, limit, "", "", query);

            return this.Store(itemTypes.Data, itemTypes.TotalRecords);
        }


    }
}
