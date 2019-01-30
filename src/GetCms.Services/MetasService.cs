using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GetCms.Models;
using GetCms.Models.Cms.Extensions;
using GetCms.Models.DataAccess;
using GetCms.Models.Enums;
using GetCms.Models.General;
using GetCms.Models.Services;
using GetCms.Models.Validation;
using GetCms.Services;
using Microsoft.Extensions.Logging;

namespace GetCms.Services.Cms
{
    public class MetasService : BaseService,  IMetasService
    {
        private readonly IMetasDataAccess _metaDataAccess;
        private readonly IValidator<MetaData> _validator;
        public MetasService(ILoggerFactory loggerFactory, 
            IMetasDataAccess metaDataAccess,
            IValidator<MetaData> validator)
            : base(loggerFactory)
        {
            this._metaDataAccess = metaDataAccess;
            this._validator = validator;
        }

        public async Task<Result> SaveAsync(MetaData metaData, string username)
        {
            metaData.Audit(username);

            var result = _validator.Validate(metaData);

            if (result.IsValid)
            {
                try
                {
                    metaData.Id = result.NewId = await _metaDataAccess.SaveAsync(metaData, metaData.IsNew ? DataAccessActions.Insert : DataAccessActions.Update);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SaveAsync error: {ex.Message}, stack: {ex.StackTrace}");
                    result.Errors.Add(new ErrorItem(ex.Message));
                }
            }

            return result;
        }


        public async Task<Dictionary<string, MetaData>> GetAsync(int itemId, int siteId)
        {
            Dictionary<string, MetaData> data = new Dictionary<string, MetaData>();
            List<MetaData> list = await _metaDataAccess.GetAsync(itemId, siteId, null);

            foreach (var m in list)
            {
                if(!data.ContainsKey(m.Key))
                    data.Add(m.Key, m);
            }

            return data;
        }
    }
}
