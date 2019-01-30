using GetCms.Models;
using GetCms.Models.DataAccess;
using GetCms.Models.General;
using GetCms.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Services.Validation
{
    public class PageValidator : ValidatorBase<Page>
    {
        private readonly IPagesDataAccess _pagesDataAccess;
        public PageValidator(IPagesDataAccess pagesDataAccess)
        {
            _pagesDataAccess = pagesDataAccess;
        }
        public override Result Validate(Page page)
        {
            if (page == null) throw new ArgumentNullException("page is null");
            var result = new Result();

            if (string.IsNullOrEmpty(page.Name))
            {
                result.ValiationErrors.Add(CreateValidationError(page.Name, "Page.Name.RequiredError", "Name is missing."));
            }
           

            if (string.IsNullOrEmpty(page.Slug))
            {
                result.ValiationErrors.Add(CreateValidationError(page.Slug, "Page.Slug.RequiredError", "Slug is missing."));
            }
            else
            {
                // check for duplicate
                var pages = _pagesDataAccess.GetByAsync(page.SiteId, null, null, page.Slug, 0, 1).Result;
                if(pages.Total > 0)
                    result.ValiationErrors.Add(CreateValidationError(page.Slug, "Page.Slug.Duplicate", "Slug duplicate."));
            }

            if (string.IsNullOrEmpty(page.CreatedBy))
            {
                result.ValiationErrors.Add(CreateValidationError(page.CreatedBy, "Page.CreatedBy.RequiredError", "Author is missing."));
            }

            if (page.SiteId == 0)
            {
                result.ValiationErrors.Add(CreateValidationError(page.SiteId, "Page.SiteId.RequiredError", "SiteId is missing."));
            }
            

            return result;
        }
    }
}
