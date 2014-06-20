using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Data.Context;
using Core.DomainModel;
using System.Data.Entity;
using Service.Service;
using Data.Repository;
using Core.Interface.Repository;
using Core.Interface.Service;
using System.Threading;
using Validation.Validation;
using System.Globalization;
using Ext.Net.MVC;
using Ext.Net;

namespace ExtHardwareMaintenance.Models
{
    public class ItemModel
    {
        public IItemService itemService;
        public ICustomerService customerService;
        public IItemTypeService itemTypeService;
        public IMaintenanceService maintenanceService;

        public ItemModel()
        {
            itemService = new ItemService(new ItemRepository(), new ItemValidator());
            customerService = new CustomerService(new CustomerRepository(), new CustomerValidator());
            itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
            maintenanceService = new MaintenanceService(new MaintenanceRepository(), new MaintenanceValidator());
        }
    }
    public class Items
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ItemTypeId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime ManufacturedAt { get; set; }
        public DateTime WarrantyExpiryDate { get; set; }

        public static IEnumerable GetAllItem()
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                ItemModel p = new ItemModel();
                IList<Item> Items = p.itemService.GetAll();
                return Items;
            }
        }
        public static Item GetItem(int id)
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                ItemModel p = new ItemModel();
                Item Item = p.itemService.GetObjectById(id);
                return Item;
            }
        }
        internal static String SaveData(FormCollection isi)
        {
            Item item = new Item();
            item.CustomerId = Int32.Parse(isi["CmbCustomer"]);
            item.ItemTypeId = Int32.Parse(isi["TxtItemTypeId"]);
            item.Description = isi["TxtDescription"];
            item.WarrantyExpiryDate = Convert.ToDateTime(isi["TxtWarrantyExpiryDate"]);
            item.ManufacturedAt = Convert.ToDateTime(isi["TxtManufacturedAt"]);
            
            ItemModel p = new ItemModel();
            Item ItemTypeNew = p.itemService.CreateObject(item, p.customerService, p.itemTypeService);

            String err = (ItemTypeNew.Errors.Any()) ? p.itemService.GetValidator().PrintError(ItemTypeNew) : "";
            return err;
        }

        internal static string SaveDataEdit(FormCollection isi)
        {
            int id = Convert.ToInt16(isi["TxtId"]);
            ItemModel p = new ItemModel();
            Item item = p.itemService.GetObjectById(id);

            item.ItemTypeId = Int32.Parse(isi["TxtItemTypeId"]);
            item.Description = isi["TxtDescription"];
            item.WarrantyExpiryDate = Convert.ToDateTime(isi["TxtWarrantyExpiryDate"]);
            item.ManufacturedAt = Convert.ToDateTime(isi["TxtManufacturedAt"]);

            Item ItemTypeNew = p.itemService.UpdateObject(item, p.itemService, p.maintenanceService);
            String err = (ItemTypeNew.Errors.Any()) ? p.itemService.GetValidator().PrintError(ItemTypeNew) : "";
            return err;
        }
        internal static void DataDelete(int id)
        {
            ItemModel p = new ItemModel();
            Item item = p.itemService.GetObjectById(id);

            bool userDeleted = p.itemService.DeleteObject(item.Id);
        }

        public static Paging<Item> ItemPaging(int start, int limit, string sort, string dir, string filter)
        {
            ItemModel p = new ItemModel();
            List<Item> items = p.itemService.GetAll().ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                items.RemoveAll(item => !item.Description.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                items.Sort(delegate(Item x, Item y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > items.Count)
            {
                limit = items.Count - start;
            }

            List<Item> rangeItems = (start < 0 || limit < 0) ? items : items.GetRange(start, limit);

            return new Paging<Item>(rangeItems, items.Count);
        }
    }
}