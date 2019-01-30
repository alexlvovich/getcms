using GetCms.DataAccess.SqlServer.Constants;
using GetCms.DataAccess.SqlServer.Extentions;
using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.Enums.Messaging;
using GetCms.Models.General;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace GetCms.DataAccess.SqlServer
{
    public class ContentsDataAccess : BaseDataAccess, IContentsDataAccess
    {
        public ContentsDataAccess(string connectionString, ILoggerFactory loggerFactory) : base(connectionString, loggerFactory)
        {

        }
        public async Task<PagedResults<Content>> GetByAsync(int? siteId = null, int? id = null, int? pageId = null, int from = 0, int to = 10)
        {
            var list = new List<Content>();
            DataTable dt = new DataTable();
            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@Id", id),
                CreateSqlParameter("@SiteId", siteId),
                CreateSqlParameter("@PageId", pageId),
                CreateSqlParameter("@From", from),
                CreateSqlParameter("@To", to)
            };

            ExecuteDataTable(Procedures.ContentGetBy, arg, dt);
            int total = 0;

            foreach (DataRow row in dt.Rows)
            {
                // check if Order column included
                list.Add(PopulateContent(row));


                // get total rows
                if (total == 0)
                    total = row.Value<int>("Total");
            }


            return await Task.FromResult(new PagedResults<Content>() { List = list, Total = total });
        }

        public async Task<PagedResults<MessagingTemplate>> GetEmailTemplatesByAsync(int? siteId = null, int? id = null, byte? templateType = null, byte? target = null, bool? isActive = null, int from = 0, int to = 10)
        {
            int total = 0;
            var list = new List<MessagingTemplate>();
            DataTable dt = new DataTable();
            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@ContentId", id.HasValue ? id.Value : new Nullable<int>()),
                CreateSqlParameter("@Target", templateType.HasValue ? target.Value : new Nullable<byte>()),
                CreateSqlParameter("@TemplateTypeId", templateType.HasValue ? templateType.Value : new Nullable<byte>()),
                CreateSqlParameter("@IsActive", isActive.HasValue ? isActive.Value : new Nullable<bool>()),
                CreateSqlParameter("@SiteId", siteId.HasValue ? siteId.Value : new Nullable<int>()),
                CreateSqlParameter("@From", from),
                CreateSqlParameter("@To", to)
            };


            ExecuteDataTable(Procedures.MessagingTemplateGetBy, arg, dt);

            foreach (DataRow row in dt.Rows)
            {
                var c = PopulateContent(row) as MessagingTemplate;

                c.TemplateType = (TemplateTypes)row.Value<byte>("TemplateTypeId");
                c.Target = (TargetTypes)row.Value<byte>("Target");
                c.Subject = row.Value<string>("Subject");
                c.ContentId = row.Value<int>("ContentId");

                if (total == 0)
                    total = row.Value<int>("Total");

                // get total rows
                list.Add(c);
            }


            return await Task.FromResult(new PagedResults<MessagingTemplate>() { List = list, Total = total });
        }

        public async Task<int> SaveAsync(Content content, DataAccessActions action)
        {
            int id = 0; 
            SqlParameter contentId = new SqlParameter("@contentId", DbType.Int32);
            contentId.Direction = ParameterDirection.Input;

            if (!content.IsNew)
                contentId.Value = content.Id;

            if (action == DataAccessActions.Insert)
            {
                contentId.Direction = ParameterDirection.Output;
            }

            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),

                CreateSqlParameter("@Name", content.Name),

                CreateSqlParameter("@Title", content.Title),

                CreateSqlParameter("@Body", content.Body),

                CreateSqlParameter("@IsActive", content.IsActive),

                CreateSqlParameter("@ContentTypeId", (int)content.Type),

                CreateSqlParameter("@CreatedOn", content.CreatedOn),

                CreateSqlParameter("@CreatedBy", content.CreatedBy),

                CreateSqlParameter("@PublishedOn", content.PublishedOn),

                CreateSqlParameter("@PublishedBy", content.PublishedBy),

                CreateSqlParameter("@SiteId", content.SiteId),

                CreateSqlParameter("@ModifiedOn", content.ModifiedOn),
            
                CreateSqlParameter("@ModifiedBy", content.ModifiedBy),

                contentId
            };

            await ExecuteNonQueryAsync(Procedures.ContentCreateUpdateDelete, arg);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(contentId.Value);
            else
                id = content.Id;

            return id;
        }

        public async Task<int> SaveEmailTemplateAsync(MessagingTemplate t, DataAccessActions action)
        {
            int id = 0;
            SqlParameter contentId = new SqlParameter("@Id", DbType.Int32);
            contentId.Direction = ParameterDirection.Input;

            if (!t.IsNew)
                contentId.Value = t.Id;

            if (action == DataAccessActions.Insert)
            {
                contentId.Direction = ParameterDirection.Output;
            }
            var arg = new SqlParameter[]
            {
                CreateSqlParameter("@Action", (int)action),

                CreateSqlParameter("@Subject", t.Subject),

                CreateSqlParameter("@TemplateTypeId", (int)t.TemplateType),

                CreateSqlParameter("@Target", (int)t.Target),

                CreateSqlParameter("@ContentId", t.ContentId),

                CreateSqlParameter("@Parameters", t.Parameters),

                CreateSqlParameter("@EmailPriorityId", 1),

                CreateSqlParameter("@EmailBodyFormatId", 1),

                contentId
            };

            await ExecuteNonQueryAsync(Procedures.MessagingTemplateCreateUpdateDelete, arg);
            if (action == DataAccessActions.Insert)
                id = Convert.ToInt32(contentId.Value);
            else
                id = t.Id;

            return id;
        }

        public async Task SaveMapping(int contentId, int pageId, DataAccessActions action)
        {
            var arg = new SqlParameter[]
           {
                CreateSqlParameter("@Action", (int)action),

                CreateSqlParameter("@ContentId", contentId),

                CreateSqlParameter("@PageId", pageId)
           };

            await ExecuteNonQueryAsync(Procedures.ContentToPageMap, arg);

        }

        private Content PopulateContent(DataRow dataRow)
        {
            var content = new Content();

            content.Id = dataRow.Value<int>("ContentId");
            content.Type = (ContentTypes)dataRow.Value<int>("ContentTypeId");
            content.Name = dataRow.Value<string>("Name");
            content.Title = dataRow.Value<string>("Title");
            content.Body = dataRow.Value<string>("Body");
            content.SiteId = dataRow.Value<int>("SiteId");

            content.CreatedOn = Convert.ToDateTime(dataRow["CreatedOn"]);
            content.CreatedBy = dataRow.Value<string>("CreatedBy");
            if (DBNull.Value != dataRow["ModifiedOn"])
                content.ModifiedOn = Convert.ToDateTime(dataRow["ModifiedOn"]);
            if (DBNull.Value != dataRow["ModifiedBy"])
                content.ModifiedBy = dataRow.Value<string>("ModifiedBy");

            content.IsActive = dataRow.Value<bool>("IsActive");
            
            if (DBNull.Value != dataRow["Order"])
                content.Order = dataRow.Value<int>("Order");
            
            /*
            if (DBNull.Value != dataRow["RefContentId"])
                content.ContentRefId = DB.GetDBValue<int>(dataRow["RefContentId"]);

            if (DBNull.Value != dataRow["ExternalLink"])
                content.ExternalLink = DB.GetDBValue<string>(dataRow["ExternalLink"]);
            */
            return content;
        }
    }
}
