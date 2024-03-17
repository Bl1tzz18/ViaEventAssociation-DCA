using ViaEventAssociation_DCA.Core.Domain.Common.Values;
using ViaEventAssociation.Core.Tools.OperationResult;
using ViaEventAssociation.Core.Tools.OperationResult.Errors;
using System;
using System.Collections.Generic;
using ViaEventAssociation_DCA.Core.Domain.Common.Bases;

namespace ViaEventAssociation_DCA.Core.Domain.Aggregates.Guests
{
    public class Guest : AggregateRoot<GuestId>
    {
        // Properties
        public GuestName FirstName { get; }
        public GuestName LastName { get; }
        public Email Email { get; }

        // Constructor
        private Guest(GuestId id, GuestName firstName, GuestName lastName, Email email) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        // Create method
        public static Result<Guest> Create(string firstName, string lastName, string email)
        {
            try
            {
                var firstNameResult = GuestName.Create(firstName);
                if (firstNameResult.IsFailure)
                    return Result<Guest>.Failure(firstNameResult.OperationErrors);

                var lastNameResult = GuestName.Create(lastName);
                if (lastNameResult.IsFailure)
                    return Result<Guest>.Failure(lastNameResult.OperationErrors);

                var emailResult = Email.Create(email);
                if (emailResult.IsFailure)
                    return Result<Guest>.Failure(emailResult.OperationErrors);

                var guestIdResult = GuestId.GenerateId();
                if (guestIdResult.IsFailure)
                    return Result<Guest>.Failure(guestIdResult.OperationErrors);

                var guest = new Guest(guestIdResult.Payload, firstNameResult.Payload, lastNameResult.Payload, emailResult.Payload);
                return Result<Guest>.Success(guest);
            }
            catch (Exception ex)
            {
                return Result<Guest>.Failure(new List<ExceptionModel> { new ExceptionModel(ReasonEnum.BadRequest, ex.Message) });
            }
        }

        }
    }

