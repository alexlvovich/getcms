using GetCms.Models.Validation;
using System.Collections.Generic;

namespace GetCms.Models.General
{
    /// <summary>
    /// Represents base result of service method call 
    /// </summary>
    public class Result
    {
        public Result()
        {
            Errors = new List<ErrorItem>();
            ValiationErrors = new List<ValidationError>();
        }

        /// <summary>
        /// Returns a flag indication whether the call was successful.
        /// </summary>
        /// <value>True if the call was successful, otherwise false.</value>
        public bool Succeeded
        {
            get
            {
                // if no validation errors and no execution errors occured and id generated the call is succesful 
                return ValiationErrors.Count == 0 && Errors.Count == 0;
            }
        }

        /// <summary>
        /// Returns newly created object Id
        /// </summary>
        public int NewId { get; set; }

        /// <summary>
        /// Returns errors in case of failure
        /// </summary>
        public List<ErrorItem> Errors { get; set; }

        /// <summary>
        /// Return validation errors
        /// </summary>
        public List<ValidationError> ValiationErrors { get; set; }

        /// <summary>
        /// Returns if there are validation errors
        /// </summary>
        public bool IsValid
        {
            get
            {
                return ValiationErrors.Count == 0; 
            }
        } 
    }
}
