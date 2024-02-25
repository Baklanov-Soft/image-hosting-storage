using System;
using System.Security.Claims;
using System.Threading;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Handlers;
using ImageHosting.Storage.Features.Images.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ImageHosting.Storage.Features.Images.Endpoints;

public static class Images
{
    public static RouteGroupBuilder MapImagesEndpoints(this IEndpointRouteBuilder routes)
    {
        var images = routes.MapGroup("images");

        images.MapPost(pattern: "", handler: async ([FromForm] IFormFile file,
                [FromServices] IUploadFileHandler uploadFileHandler, ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) switch
                {
                    { } id => UserId.ParseExact(id, "D"),
                    _ => UserId.Empty
                };
                var imageId = new ImageId(Guid.NewGuid());
                var uploadedAt = DateTime.UtcNow;

                var response = await uploadFileHandler.UploadAsync(userId, imageId, file, hidden: false,
                    uploadedAt, cancellationToken);

                return TypedResults.Ok(response);
            })
            .DisableAntiforgery()
            .WithName("PostImage")
            .MapToApiVersion(1);

        images.MapGet(pattern: "{id:guid}/asset", handler: async ([FromRoute] Guid id, [FromQuery] SizeParam? size,
                [FromServices] IGetImageAssetHandler getImageAssetHandler, CancellationToken cancellationToken) =>
            {
                var stream = await getImageAssetHandler.GetImageAsync(id, cancellationToken);

                return TypedResults.File(stream);
            })
            .WithName("GetImageAsset")
            .MapToApiVersion(1);

        return images;
    }
}