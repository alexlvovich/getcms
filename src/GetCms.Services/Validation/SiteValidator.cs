using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Validation;
using System;

namespace GetCms.Services.Validation
{
    public class SiteValidator : ValidatorBase<Site>
    {
        public override Result Validate(Site site)
        {
            if (site == null) throw new ArgumentNullException("site is null");
            var result = new Result();

            if (string.IsNullOrEmpty(site.Name))
            {
                result.ValiationErrors.Add(CreateValidationError(site.Name, "Site.Name.RequiredError", "Name is missing."));
            }

            if (string.IsNullOrEmpty(site.Host))
            {
                result.ValiationErrors.Add(CreateValidationError(site.Host, "Site.Host.RequiredError", "Host is missing."));
            }


            if (string.IsNullOrEmpty(site.CreatedBy))
            {
                result.ValiationErrors.Add(CreateValidationError(site.CreatedBy, "Site.CreatedBy.RequiredError", "Author is missing."));
            }

            return result;
        }
    }
}
