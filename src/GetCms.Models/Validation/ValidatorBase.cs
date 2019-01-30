using GetCms.Models.General;
using System;

namespace GetCms.Models.Validation
{
    public abstract class ValidatorBase<T> : IValidator<T>
    {
        public string _name => typeof(T).Name;
        protected ValidatorBase()
        {
        }

        #region IValidator<T> Members

        public abstract Result Validate(T entity);

        #endregion

        protected ValidationError CreateValidationError(object attemtedValue, string validationKey, string validationMessage, params object[] validationMessageParameters)
        {
            if (validationMessageParameters != null && validationMessageParameters.Length > 0)
            {
                validationMessage = string.Format(validationMessage, validationMessageParameters);
            }

            return new ValidationError(
                validationKey,
                attemtedValue,
                new InvalidOperationException(validationMessage)
                );
        }


    }
}
