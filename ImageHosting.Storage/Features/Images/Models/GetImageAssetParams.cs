using System;
using System.Linq;
using FluentValidation;
using ImageHosting.Persistence.ValueTypes;
using Microsoft.AspNetCore.Mvc;

namespace ImageHosting.Storage.Features.Images.Models;

public class GetImageAssetParams
{
    [FromRoute]
    public required ImageId Id { get; init; }
    [FromQuery(Name = "size")]
    public required SizeValues? Size { get; init; }

    public string GetObjectName()
    {
        var imageId = Id.ToString();
        return Size is null ? imageId : $"{(short)Size}/{imageId}";
    }

    public class Validator : AbstractValidator<GetImageAssetParams>
    {
        public Validator()
        {
            RuleFor(p => p.Size)
                .Must(s => Enum.GetValues<SizeValues>().Contains(s!.Value))
                .WithMessage("Invalid value.")
                .When(p => p.Size.HasValue);
        }
    }
}