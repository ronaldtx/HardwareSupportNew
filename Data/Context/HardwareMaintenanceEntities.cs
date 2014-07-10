using Core.DomainModel;
using Data.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class HardwareMaintenanceEntities : DbContext
    {
        public HardwareMaintenanceEntities() : base("HardwareDb") 
        {
            //Database.SetInitializer<HardwareMaintenanceEntities>(new DropCreateDatabaseAlways<HardwareMaintenanceEntities>());
            Database.SetInitializer<HardwareMaintenanceEntities>(new DropCreateDatabaseIfModelChanges<HardwareMaintenanceEntities>());
        }

        public void DeleteAllTables()
        {
            IList<String> tableNames = new List<String>() { "Customer", "ItemType", "Item", "DbUser", "Maintenance" };

            foreach (var tableName in tableNames)
            {
                Database.ExecuteSqlCommand(string.Format("DELETE FROM {0}", tableName));
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();  

            modelBuilder.Configurations.Add(new CustomerMapping());
            modelBuilder.Configurations.Add(new ItemMapping());
            modelBuilder.Configurations.Add(new ItemTypeMapping());
            modelBuilder.Configurations.Add(new MaintenanceMapping());
            modelBuilder.Configurations.Add(new UserMapping());
            modelBuilder.Configurations.Add(new UserLoginMapping());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }
        public DbSet<DbUser> DbUsers { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
    }
}