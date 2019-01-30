using GetCms.DataAccess.SqlServer.Constants;
using GetCms.DataAccess.SqlServer.Extentions;
using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace GetCms.DataAccess.SqlServer
{
    public class MenusDataAccess : BaseDataAccess, IMenusDataAccess
    {
        public MenusDataAccess(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory)
        {

        }
        public async Task<List<Menu>> GetByAsync(int? siteId = null, int? id = null, bool? isActive = null, string name = null, int from = 0, int to = 10)
        {
            DataTable dt = new DataTable();

            var list = new List<Menu>();

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@MenuId", id.HasValue ? id.Value : new Nullable<int>()),
                CreateSqlParameter("@SiteId", siteId.HasValue ? siteId.Value : new Nullable<int>()),
                CreateSqlParameter("@IsActive", isActive.HasValue ? isActive.Value : new Nullable<bool>()),
                CreateSqlParameter("@Name", name),
                CreateSqlParameter("@From", from),
                CreateSqlParameter("@To", to)
            };

            ExecuteDataTable(Procedures.MenuGetBy, arg, dt);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Menu()
                {
                    Id = row.Value<int>("MenuId"),

                    CreatedOn = Convert.ToDateTime(row["CreatedOn"]),

                    CreatedBy = row.Value<string>("CreatedBy"),

                    PublishedOn = row["PublishedOn"] != DBNull.Value ? Convert.ToDateTime(row["PublishedOn"]) : new DateTime?(),

                    PublishedBy = row.Value<string>("PublishedBy"),

                    ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : new DateTime?(),

                    ModifiedBy = row.Value<string>("ModifiedBy"),

                    SiteId = row.Value<int>("SiteId"),

                    Name = row.Value<string>("Name"),

                    IsActive = row.Value<bool>("IsActive")
                });
            }


            return await Task.FromResult(list);
        }

        public async Task<List<MenuItem>> GetItemsByAsync(int? menuId, int? id)
        {
            DataTable dt = new DataTable();

            var list = new List<MenuItem>();

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@ItemId", id.HasValue ? id.Value : new Nullable<int>()),
                CreateSqlParameter("@MenuId", menuId.HasValue ? menuId.Value : new Nullable<int>()),
                CreateSqlParameter("@From", 0),
                CreateSqlParameter("@To", 100)
            };

            ExecuteDataTable(Procedures.MenuItemsGetBy, arg, dt);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MenuItem()
                {
                    Id = row.Value<int>("MenuId"),

                    CreatedOn = Convert.ToDateTime(row["CreatedOn"]),

                    CreatedBy = row.Value<string>("CreatedBy"),

                    PublishedOn = row["PublishedOn"] != DBNull.Value ? Convert.ToDateTime(row["PublishedOn"]) : new DateTime?(),

                    PublishedBy = row.Value<string>("PublishedBy"),

                    ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : new DateTime?(),

                    ModifiedBy = row.Value<string>("ModifiedBy"),

                    MenuId = row.Value<int>("MenuId"),

                    Text = row.Value<string>("Name"),

                    Alt = row.Value<string>("Title"),
                    
                    Link = row.Value<string>("Url"),
                    
                    Order = row.Value<byte>("Order")
                });
            }


            return await Task.FromResult(list);
        }

        public async Task<int> SaveAsync(Menu menu, DataAccessActions action)
        {
            int id = 0;
            SqlParameter metaDataId = new SqlParameter("@MenuId", DbType.Int32);
            metaDataId.Direction = ParameterDirection.Input;

            if (!menu.IsNew)
                metaDataId.Value = menu.Id;

            if (action == DataAccessActions.Insert)
            {
                metaDataId.Direction = ParameterDirection.Output;
            }

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),

                CreateSqlParameter("@Name", menu.Name),

                CreateSqlParameter("@IsActive", menu.IsActive),

                CreateSqlParameter("@SiteId", menu.SiteId),

                CreateSqlParameter("@CreatedOn", menu.CreatedOn),

                CreateSqlParameter("@CreatedBy", menu.CreatedBy),

                CreateSqlParameter("@ModifiedOn", menu.ModifiedOn),

                CreateSqlParameter("@ModifiedBy", menu.ModifiedBy),

                CreateSqlParameter("@PublishedOn", menu.PublishedOn),

                CreateSqlParameter("@PublishedBy", menu.PublishedBy),
                
                metaDataId
            };

            await ExecuteNonQueryAsync(Procedures.MenuCreateUpdateDelete, arg);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(metaDataId.Value);
            else
                id = menu.Id;

            return id;
        }

        public async Task<int> SaveItemAsync(MenuItem menuItem, DataAccessActions action)
        {
            int id = 0;
            SqlParameter menuItemId = new SqlParameter("@ItemId", DbType.Int32);
            menuItemId.Direction = ParameterDirection.Input;

            if (!menuItem.IsNew)
                menuItemId.Value = menuItem.Id;

            if (action == DataAccessActions.Insert)
            {
                menuItemId.Direction = ParameterDirection.Output;
            }

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),

                CreateSqlParameter("@MenuId", menuItem.MenuId),
                
                CreateSqlParameter("@Name", menuItem.Text),

                CreateSqlParameter("@Title", menuItem.Alt),

                CreateSqlParameter("@Url", menuItem.Link),

                CreateSqlParameter("@CreatedOn", menuItem.CreatedOn),

                CreateSqlParameter("@CreatedBy", menuItem.CreatedBy),

                CreateSqlParameter("@PublishedOn", menuItem.PublishedOn),

                CreateSqlParameter("@PublishedBy", menuItem.PublishedBy),

                CreateSqlParameter("@ModifiedOn", menuItem.ModifiedOn),

                CreateSqlParameter("@ModifiedBy", menuItem.ModifiedBy),

                CreateSqlParameter("@IsActive", menuItem.IsActive),

                CreateSqlParameter("@ImagePath", menuItem.ImagePath),

                CreateSqlParameter("@ImagePathAct", menuItem.ImagePathAlt),

                CreateSqlParameter("@Order", menuItem.Order),

                menuItemId
            };

            await ExecuteNonQueryAsync(Procedures.MenuItemsCreateUpdateDelete, arg);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(menuItemId.Value);
            else
                id = menuItem.Id;

            return id;
        }
    }
}
