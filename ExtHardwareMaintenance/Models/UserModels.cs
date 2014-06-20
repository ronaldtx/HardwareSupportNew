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
    public class UserModel
    {
        public IUserService userService;

        public UserModel()
        {
            userService = new UserService(new UserRepository(), new UserValidator());
        }
    }
    public class Users
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static IEnumerable GetAllUser()
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                UserModel p = new UserModel();
                IList<DbUser> users = p.userService.GetAll();
                return users;
            }
        }
        public static Paging<DbUser> UsersPaging(int start, int limit, string sort, string dir, string filter)
        {
            UserModel p = new UserModel();
            List<DbUser> users = p.userService.GetAll().ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                users.RemoveAll(user => !user.Name.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                users.Sort(delegate(DbUser x, DbUser y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > users.Count)
            {
                limit = users.Count - start;
            }

            List<DbUser> rangePlants = (start < 0 || limit < 0) ? users : users.GetRange(start, limit);

            return new Paging<DbUser>(rangePlants, users.Count);
        }
        public static DbUser GetUser(int id)
        {
            var db = new HardwareMaintenanceEntities();
            using (db)
            {
                UserModel p = new UserModel();
                DbUser user = p.userService.GetObjectById(id);
                return user;
            }
        }
        /*
        public static void SaveData(Dictionary<string, string> isi)
        {
            DbUser user = new DbUser();
            user.Name = isi["TxtName"];
            user.Description = isi["TxtDescription"];
            ExtNetModel p = new ExtNetModel();
            DbUser userNew = p.userService.CreateObject(user);
        }
        */
        internal static Dictionary<String, String> SaveData(FormCollection isi)
        {
            DbUser user = new DbUser();
            user.Name = isi["Name"];
            user.Description = isi["TxtDescription"];
            UserModel p = new UserModel();
            DbUser userNew = p.userService.CreateObject(user);
            Dictionary<String, String> err = (userNew.Errors.Any()) ? userNew.Errors : null;
            return err;
        }
        internal static String SaveDataEdit(FormCollection isi)
        {
            int id = Convert.ToInt16(isi["TxtId"]);
            UserModel p = new UserModel();
            DbUser user = p.userService.GetObjectById(id);

            user.Name = isi["Name"];
            user.Description = isi["TxtDescription"];

            DbUser userNew = p.userService.UpdateObject(user);
            String err = (userNew.Errors.Any()) ? p.userService.GetValidator().PrintError(userNew) : "";
            return err;

        }

        internal static void DataDelete(int id)
        {
            UserModel p = new UserModel();
            DbUser user = p.userService.GetObjectById(id);

            bool userDeleted = p.userService.DeleteObject(user.Id);
        }

        public static Paging<DbUser> ItemPaging(int start, int limit, string sort, string dir, string filter)
        {
            UserModel p = new UserModel();
            List<DbUser> users = p.userService.GetAll().ToList();

            if (!string.IsNullOrEmpty(filter) && filter != "*")
            {
                users.RemoveAll(user => !user.Name.ToLower().StartsWith(filter.ToLower()));
            }

            if (!string.IsNullOrEmpty(sort))
            {
                users.Sort(delegate(DbUser x, DbUser y)
                {
                    object a;
                    object b;

                    int direction = dir == "DESC" ? -1 : 1;

                    a = x.GetType().GetProperty(sort).GetValue(x, null);
                    b = y.GetType().GetProperty(sort).GetValue(y, null);

                    return CaseInsensitiveComparer.Default.Compare(a, b) * direction;
                });
            }

            if ((start + limit) > users.Count)
            {
                limit = users.Count - start;
            }

            List<DbUser> rangeUsers = (start < 0 || limit < 0) ? users : users.GetRange(start, limit);

            return new Paging<DbUser>(rangeUsers, users.Count);
        }
    }
}