using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Services.Validation
{
    public class MenuValidator : ValidatorBase<Menu>
    {
        public override Result Validate(Menu m)
        {
            if (m == null) throw new ArgumentNullException($"{_name} is null");
            var result = new Result();

            if (string.IsNullOrEmpty(m.Name))
            {
                result.ValiationErrors.Add(CreateValidationError(m.Name, $"{_name}.Name.RequiredError", "Name is missing."));
            }

            if (string.IsNullOrEmpty(m.CreatedBy))
            {
                result.ValiationErrors.Add(CreateValidationError(m.CreatedBy, $"{_name}.CreatedBy.RequiredError", "Author is missing."));
            }

            if (m.SiteId == 0)
            {
                result.ValiationErrors.Add(CreateValidationError(m.SiteId, $"{_name}.SiteId.RequiredError", "SiteId is missing."));
            }

            return result;
        }
    }
}
