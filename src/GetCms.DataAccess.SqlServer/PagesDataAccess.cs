using GetCms.DataAccess.SqlServer.Constants;
using GetCms.DataAccess.SqlServer.Extentions;
using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace GetCms.DataAccess.SqlServer
{
    public class PagesDataAccess : BaseDataAccess, IPagesDataAccess
    {
        public PagesDataAccess(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory)
        {

        }
        public async Task<PagedResults<Page>> GetByAsync(int? siteId = null, int? id = null, string name = null, string slug = null, int start = 0, int to = 10)
        {
            DataTable dt = new DataTable();
            List<Page> list = new List<Page>();
            SqlParameter[] parameters = new SqlParameter[]
            {

                CreateSqlParameter("@SiteId", siteId.HasValue ? siteId.Value : new Nullable<int>()),
                CreateSqlParameter("@Name", name),
                CreateSqlParameter("@Slug", slug),
                CreateSqlParameter("@From", start),
                CreateSqlParameter("@To", to)
            };

            //total = 0;
            ExecuteDataTable(Procedures.PagesGetBy, parameters, dt);
            int total = 0;

            foreach (DataRow dr in dt.Rows)
            {
                Page w = new Page()
                {
                    IsActive = dr.Value<bool>("IsActive"),
                    Id = dr.Value<int>("PageId"),
                    Name = dr.Value<string>("Name"),
                    Slug = dr.Value<string>("Slug"),
                    SiteId = dr.Value<int>("SiteId"),
                    MasterPageId = dr["MasterPageId"] != DBNull.Value ? dr.Value<int>("MasterPageId") : new int?(),
                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"]),
                    CreatedBy = dr.Value<string>("CreatedBy"),
                    PublishedOn = dr["PublishedOn"] != DBNull.Value ? Convert.ToDateTime(dr["PublishedOn"]) : new DateTime?(),
                    PublishedBy = dr.Value<string>("PublishedBy"),
                    ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : new DateTime?(),
                    ModifiedBy = dr.Value<string>("ModifiedBy"),
                    Title = dr.Value<string>("Title"),
                };

                if (total == 0)
                    total = dr.Value<int>("Total");

                list.Add(w);
            }

            return await Task.FromResult(new PagedResults<Page>() { List = list, Total = total });
        }

        public async Task<int> SaveAsync(Page page, DataAccessActions action)
        {
            int id = 0;
            SqlParameter pageId = new SqlParameter("@PageId", DbType.Int32);
            pageId.Direction = ParameterDirection.Input;


            if (action == DataAccessActions.Insert)
            {
                pageId.Direction = ParameterDirection.Output;
            }
            else
            {
                pageId.Value = page.Id;
            }

            var parameters = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),
                CreateSqlParameter("@Name", page.Name),
                CreateSqlParameter("@SiteId", page.SiteId),
                CreateSqlParameter("@IsActive", page.IsActive),
                CreateSqlParameter("@Slug", page.Slug),
                CreateSqlParameter("@CreatedOn", page.CreatedOn),
                CreateSqlParameter("@CreatedBy", page.CreatedBy),
                CreateSqlParameter("@ModifiedOn", page.ModifiedOn),
                CreateSqlParameter("@ModifiedBy", page.ModifiedBy),
                CreateSqlParameter("@PublishedOn", page.PublishedOn),
                CreateSqlParameter("@PublishedBy", page.PublishedBy),
                CreateSqlParameter("@PageTypeId", (int)page.PageType),
                CreateSqlParameter("@ParentPageId", page.ParentId.HasValue ? page.ParentId.Value : new Nullable<int>()),
                CreateSqlParameter("@MasterPageId", page.MasterPageId.HasValue ? page.MasterPageId.Value : new Nullable<int>()),
                CreateSqlParameter("@Title", page.Title),
                pageId
            };

            await ExecuteNonQueryAsync(Procedures.PagesCreateUpdateDelete, parameters);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(pageId.Value);
            else
                id = page.Id;
            return id;
        }
    }
}
