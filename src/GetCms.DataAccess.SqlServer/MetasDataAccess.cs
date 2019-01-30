using GetCms.DataAccess.SqlServer.Constants;
using GetCms.DataAccess.SqlServer.Extentions;
using GetCms.Models;
using GetCms.Models.Cms.Enums;
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
    public class MetasDataAccess : BaseDataAccess, IMetasDataAccess
    {
        public MetasDataAccess(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory)
        {

        }
        public async Task<List<MetaData>> GetAsync(int itemId, int siteId, MetaDataTypes? types)
        {
            DataTable dt = new DataTable();

            var list = new List<MetaData>();

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@ItemId", itemId),
                CreateSqlParameter("@SiteId", siteId),
                CreateSqlParameter("@MetaDataTypeId", types.HasValue ? (short)types : new Nullable<short>())
            };

            ExecuteDataTable(Procedures.MetadataGetBy, arg, dt);

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new MetaData()
                {
                    Id = row.Value<int>("MetaDataId"),

                    ItemId = row.Value<int>("ItemId"),

                    Key = row.Value<string>("Key"),

                    Value = row.Value<string>("Value"),

                    CreatedOn = Convert.ToDateTime(row["CreatedOn"]),

                    CreatedBy = row.Value<string>("CreatedBy"),

                    PublishedOn = row["PublishedOn"] != DBNull.Value ? Convert.ToDateTime(row["PublishedOn"]) : new DateTime?(),

                    PublishedBy = row.Value<string>("PublishedBy"),
                    
                    ModifiedOn = row["ModifiedOn"] != DBNull.Value ? Convert.ToDateTime(row["ModifiedOn"]) : new DateTime?(),

                    ModifiedBy = row.Value<string>("ModifiedBy"),

                    Type = (MetaDataTypes)row.Value<byte>("MetaDataTypeId"),

                    SiteId = row.Value<int>("SiteId"),

                });
            }


            return await Task.FromResult(list);
        }

        public async Task<int> SaveAsync(MetaData data, DataAccessActions action)
        {
            int id = 0;
            SqlParameter metaDataId = new SqlParameter("@MetaDataId", DbType.Int32);
            metaDataId.Direction = ParameterDirection.Input;

            if (!data.IsNew)
                metaDataId.Value = data.Id;

            if (action == DataAccessActions.Insert)
            {
                metaDataId.Direction = ParameterDirection.Output;
            }

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),

                CreateSqlParameter("@ItemId", data.ItemId),

                CreateSqlParameter("@MetaDataTypeId", (short)data.Type),

                CreateSqlParameter("@Key", data.Key),

                CreateSqlParameter("@Value", data.Value),

                CreateSqlParameter("@CreatedOn", data.CreatedOn),

                CreateSqlParameter("@CreatedBy", data.CreatedBy),

                CreateSqlParameter("@PublishedOn", data.PublishedOn),

                CreateSqlParameter("@PublishedBy", data.PublishedBy),

                CreateSqlParameter("@ModifiedOn", data.ModifiedOn),

                CreateSqlParameter("@ModifiedBy", data.ModifiedBy),

                CreateSqlParameter("@SiteId", data.SiteId),

                metaDataId
            };

            await ExecuteNonQueryAsync(Procedures.MetadataCreateUpdateDelete, arg);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(metaDataId.Value);
            else
                id = data.Id;

            return id;
        }
    }
}
