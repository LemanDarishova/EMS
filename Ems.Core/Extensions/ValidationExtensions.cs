using Ems.Core.Wrappers.Concrete;
using FluentValidation.Results;
using System.Collections.Generic;

namespace Ems.Core.Extensions
{
    public static class ValidationExtensions
    {
        public static List<ResponseValidationResult> ToResponseValidationResults(this ValidationResult validationResult)
        {
            List<ResponseValidationResult> responseValidationResults = new List<ResponseValidationResult>();

            foreach (var item in validationResult.Errors)
            {
                responseValidationResults.Add(new ResponseValidationResult
                {
                    ErrorMessage = item.ErrorMessage,
                    PropertyName = item.PropertyName
                });
            }
            return responseValidationResults;
        }
    }
}
