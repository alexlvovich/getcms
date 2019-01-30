using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Services.Validation
{
    public class ContentValidator : ValidatorBase<Content>
    {
        public override Result Validate(Content content)
        {
            if (content == null) throw new ArgumentNullException("Content is null");
            var result = new Result();

            if (string.IsNullOrEmpty(content.Name))
            {
                result.ValiationErrors.Add(CreateValidationError(content.Name, "Content.Name.RequiredError", "Name is missing."));
            }

        
            if (string.IsNullOrEmpty(content.CreatedBy))
            {
                result.ValiationErrors.Add(CreateValidationError(content.CreatedBy, "Content.CreatedBy.RequiredError", "Author is missing."));
            }

            if (content.SiteId == 0)
            {
                result.ValiationErrors.Add(CreateValidationError(content.SiteId, "Content.SiteId.RequiredError", "SiteId is missing."));
            }

            return result;
        }
    }
}
