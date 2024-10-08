using FluentValidation;

namespace ImageHosting.Storage.WebApi.Features.Images.Models;

public class UpdateNameCommand
{
    public required string NewName { get; init; }

    public class Validator : AbstractValidator<UpdateNameCommand>
    {
        public Validator()
        {
            RuleFor(p => p.NewName).NotEmpty();
        }
    }
}