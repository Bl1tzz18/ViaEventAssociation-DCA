using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;

namespace ViaEventAssociation_DCA.Core.Domain.Common.Values
{
    public class Email : ValueObject
    {
        public string Value { get; }

        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            try
            {
                var validation = Validate(email);
                if (validation.IsSuccess)
                {
                    return Result<Email>.Success(new Email(email));
                }
                else
                {
                    return Result<Email>.Failure(validation.OperationErrors);
                }
            }
            catch (Exception exception)
            {
                return Result<Email>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, exception.Message) });
            }
        }

        private static Result Validate(string email)
        {
            var errors = new List<ExceptionModel>();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Email cannot be empty"));
            }
            else if (!Regex.IsMatch(email, @"^([a-zA-Z]{3,6}|\d{6})@via\.dk$", RegexOptions.IgnoreCase))
            {
                errors.Add(new ExceptionModel(ReasonEnum.BadRequest, "Invalid email format"));
            }

            return errors.Any() ? Result.Failure(errors) : Result.Success();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}