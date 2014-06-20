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
    public class MaintenanceController : Controller
    {
        //
        // GET: /Master/Maintenance/

        public ActionResult Index()
        {
            return View(Maintenances.GetAllMaintenance());
        }

        public ActionResult SaveData(FormCollection isi)
        {
            String err = Maintenances.SaveData(isi);

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

        public ActionResult GetDataItem(int start, int limit, int page, string query)
        {
            //return this.Store(ItemTypes.GetAllItemType());
            Paging<Item> items = Items.ItemPaging(start, limit, "", "", query);

            return this.Store(items.Data, items.TotalRecords);
        }

        public ActionResult GetDataUser(int start, int limit, int page, string query)
        {
            //return this.Store(ItemTypes.GetAllItemType());
            Paging<DbUser> users = Users.ItemPaging(start, limit, "", "", query);

            return this.Store(users.Data, users.TotalRecords);
        }
    }
}
