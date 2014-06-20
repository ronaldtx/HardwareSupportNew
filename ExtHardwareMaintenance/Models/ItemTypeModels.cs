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
using Ext.Net;

namespace ExtHardwareMaintenance.Models
{
    public class ItemTypeModel
    {
        public IItemTypeService itemTypeService;

        public ItemTypeModel()
        {
            itemTypeService = new ItemTypeService(new ItemTypeRepository(), new ItemTypeValidator());
        }
    }
    public class ItemTypes
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static IEnumerable GetAllItemType()
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                ItemTypeModel p = new ItemTypeModel();
                IList<ItemType> ItemTypes = p.itemTypeService.GetAll();
                return ItemTypes;
            }
        }
        public static ItemType GetItemType(int id)
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                ItemTypeModel p = new ItemTypeModel();
                ItemType ItemType = p.itemTypeService.GetObjectById(id);
                return ItemType;
            }
        }

        internal static String SaveData(FormCollection isi)
        {
            ItemType ItemType = new ItemType();
            ItemType.Name = isi["TxtName"];
            ItemType.Description = isi["TxtDescription"];
            ItemTypeModel p = new ItemTypeModel();
            ItemType ItemTypeNew = p.itemTypeService.CreateObject(ItemType);
            String err = (ItemTypeNew.Errors.Any()) ? p.itemTypeService.GetValidator().PrintError(ItemTypeNew) : "";
            return err;
        }
        internal static String SaveDataEdit(FormCollection isi)
        {
            int id = Convert.ToInt16(isi["TxtId"]);
            ItemTypeModel p = new ItemTypeModel();
            ItemType ItemType = p.itemTypeService.GetObjectById(id);

            ItemType.Name = isi["TxtName"];
            ItemType.Description = isi["TxtDescription"];
            ItemType ItemTypeNew = p.itemTypeService.UpdateObject(ItemType);
            String err = (ItemTypeNew.Errors.Any()) ? p.itemTypeService.GetValidator().PrintError(ItemTypeNew) : "";
            return err;
        }

        internal static void DataDelete(int id)
        {
            ItemTypeModel p = new ItemTypeModel();
            ItemType ItemType = p.itemTypeService.GetObjectById(id);

            bool ItemTypeDeleted = p.itemTypeService.DeleteObject(ItemType.Id);
        }

        public static Paging<ItemType> ItemTypesPaging(int start, int limit, string sort, string dir, string filter)
        {
            ItemTypeModel p = new ItemTypeModel();
            List<ItemType> itemTypes = p.itemTypeService.GetAll().ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                itemTypes.RemoveAll(itemType => !itemType.Name.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                itemTypes.Sort(delegate(ItemType x, ItemType y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > itemTypes.Count)
            {
                limit = itemTypes.Count - start;
            }

            List<ItemType> rangePlants = (start < 0 || limit < 0) ? itemTypes : itemTypes.GetRange(start, limit);

            return new Paging<ItemType>(rangePlants, itemTypes.Count);
        }
    }
}