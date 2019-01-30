using GetCms.Models;
using GetCms.Models.General;
using GetCms.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace GetCms.Services.Validation
{
    public class MenuItemValidator : ValidatorBase<MenuItem>
    {
        public override Result Validate(MenuItem m)
        {
            if (m == null) throw new ArgumentNullException($"{_name} is null");
            var result = new Result();

            if (string.IsNullOrEmpty(m.Text))
            {
                result.ValiationErrors.Add(CreateValidationError(m.Text, $"{_name}.Text.RequiredError", "Text is missing."));
            }

            if (string.IsNullOrEmpty(m.CreatedBy))
            {
                result.ValiationErrors.Add(CreateValidationError(m.CreatedBy, $"{_name}.CreatedBy.RequiredError", "Author is missing."));
            }

            if (m.MenuId == 0)
            {
                result.ValiationErrors.Add(CreateValidationError(m.MenuId, $"{_name}.MenuId.RequiredError", "MenuId is missing."));
            }

            return result;
        }
    }
}
