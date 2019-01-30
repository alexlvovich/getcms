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
    public class SitesDataAccess : BaseDataAccess, ISitesDataAccess
    {
        public SitesDataAccess(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory)
        {

        }
        public async Task<List<Site>> GetByAsync(int? siteId, int? parentSiteId, bool? isActive, string name, string host, string lang, int? from, int? to)
        {
            DataTable dt = new DataTable();
            List<Site> list = new List<Site>();
            SqlParameter[] parameters = new SqlParameter[]
            {

                CreateSqlParameter("@SiteId", siteId.HasValue ? siteId.Value : new Nullable<int>()),
                CreateSqlParameter("@ParentSiteId", parentSiteId.HasValue ? parentSiteId.Value : new Nullable<int>()),
                CreateSqlParameter("@Name", name),
                CreateSqlParameter("@Host", host),
                CreateSqlParameter("@IsActive", isActive.HasValue ? isActive.Value : new Nullable<bool>()),
                CreateSqlParameter("@Lang", lang),
                CreateSqlParameter("@From", from),
                CreateSqlParameter("@To", to)
            };

            //total = 0;
            ExecuteDataTable(Procedures.SitesGetBy, parameters, dt);

            foreach (DataRow dr in dt.Rows)
            {
                Site w = new Site()
                {
                    IsActive = dr.Value<bool>("IsActive"),
                    ParentSiteId = dr["ParentSiteId"] != DBNull.Value ? dr.Value<int>("ParentSiteId") : new int?(),
                    Id = dr.Value<int>("SiteId"),
                    Name = dr.Value<string>("Name"),
                    Host = dr.Value<string>("Host"),
                    Language = Languages.English,
                    PageTitleSeparator = dr.Value<string>("PageTitleSeparator"),

                    CreatedOn = Convert.ToDateTime(dr["CreatedOn"]),
                    CreatedBy = dr.Value<string>("CreatedBy"),

                    PublishedOn = dr["PublishedOn"] != DBNull.Value ? Convert.ToDateTime(dr["PublishedOn"]) : new DateTime?(),
                    PublishedBy = dr.Value<string>("PublishedBy"),

                    ModifiedOn = dr["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(dr["ModifiedOn"]) : new DateTime?(),
                    ModifiedBy = dr.Value<string>("ModifiedBy"),
                };

                list.Add(w);
            }

            return await Task.FromResult(list);
        }

        public async Task<int> SaveAsync(Site site, DataAccessActions action)
        {
            int id = 0;
            SqlParameter siteId = new SqlParameter("@SiteId", DbType.Int32);
            siteId.Direction = ParameterDirection.Input;


            if (action == DataAccessActions.Insert)
            {
                siteId.Direction = ParameterDirection.Output;
            }
            else
            {
                siteId.Value = site.Id;
            }

            var parameters = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),
                CreateSqlParameter("@TimeZoneOffset",site.TimeZoneOffset),
                CreateSqlParameter("@PageTitleSeparator",site.PageTitleSeparator),
                CreateSqlParameter("@IsActive",site.IsActive),
                CreateSqlParameter("@Name", site.Name),
                CreateSqlParameter("@Host", site.Host),
                CreateSqlParameter("@Language", site.Language.Code),
                CreateSqlParameter("@ParentSiteId", site.ParentSiteId),
                CreateSqlParameter("@ContentType", site.ContentType),
                CreateSqlParameter("@CreatedOn", site.CreatedOn),
                CreateSqlParameter("@CreatedBy", site.CreatedBy),
                CreateSqlParameter("@ModifiedOn", site.ModifiedOn),
                CreateSqlParameter("@ModifiedBy", site.ModifiedBy),

                siteId
            };

            await ExecuteNonQueryAsync(Procedures.SitesCreateUpdate, parameters);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(siteId.Value);
            else
                id = site.Id;
            return id;
        }
    }
}
