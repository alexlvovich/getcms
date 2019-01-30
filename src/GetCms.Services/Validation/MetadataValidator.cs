using GetCms.Models;
using GetCms.Models.Cms.Enums;
using GetCms.Models.General;
using GetCms.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Services.Validation
{
    public class MetadataValidator : ValidatorBase<MetaData>
    {
        public override Result Validate(MetaData m)
        {
            if (m == null) throw new ArgumentNullException($"{_name} is null");
            var result = new Result();

            if (string.IsNullOrEmpty(m.Key))
            {
                result.ValiationErrors.Add(CreateValidationError(m.Key, $"{_name}.Key.RequiredError", "Key is missing."));
            }

            if (string.IsNullOrEmpty(m.Value))
            {
                result.ValiationErrors.Add(CreateValidationError(m.Value, $"{_name}.Value.RequiredError", "Value is missing."));
            }

            if (string.IsNullOrEmpty(m.CreatedBy))
            {
                result.ValiationErrors.Add(CreateValidationError(m.CreatedBy, $"{_name}.CreatedBy.RequiredError", "Author is missing."));
            }

            if (m.SiteId == 0)
            {
                result.ValiationErrors.Add(CreateValidationError(m.SiteId, $"{_name}.SiteId.RequiredError", "SiteId is missing."));
            }

            if (m.ItemId == 0)
            {
                result.ValiationErrors.Add(CreateValidationError(m.SiteId, $"{_name}.ItemId.RequiredError", "ItemId is missing."));
            }

            if (m.Type == MetaDataTypes.NotSet)
            {
                result.ValiationErrors.Add(CreateValidationError(m.SiteId, $"{_name}.Type.RequiredError", "Type not set."));
            }

            return result;
        }
    }
}
