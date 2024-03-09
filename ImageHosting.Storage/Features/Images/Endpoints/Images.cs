using System;
using System.Security.Claims;
using System.Threading;
using ImageHosting.Persistence.ValueTypes;
using ImageHosting.Storage.Features.Images.Handlers;
using ImageHosting.Storage.Features.Images.Models;
using ImageHosting.Storage.Http;
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

                return TypedResults.CreatedAtRoute(response, "GetImage", new { id = response.Id });
            })
            .DisableAntiforgery()
            .WithName("PostImage")
            .MapToApiVersion(1);

        images.MapGet(pattern: "{id}/asset", handler: async ([AsParameters] GetImageAssetParams @params,
                [FromServices] IGetImageAssetHandler getImageAssetHandler, ClaimsPrincipal user,
                CancellationToken cancellationToken) =>
            {
                var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) switch
                {
                    { } uid => UserId.ParseExact(uid, "D"),
                    _ => UserId.Empty
                };
                var objectName = @params.GetObjectName();
                var result = await getImageAssetHandler.GetImageAsync(userId, objectName, cancellationToken);

                return Results.File(result.Stream, result.ContentType);
            })
            .AddEndpointFilter<ValidationFilter<GetImageAssetParams>>()
            .WithName("GetImageAsset")
            .MapToApiVersion(1)
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity);

        images.MapGet(pattern: "{id}", handler: async ([FromRoute] ImageId id,
                [FromServices] IGetImageHandler getImageHandler, CancellationToken cancellationToken) =>
            {
                var readImage = await getImageHandler.GetAsync(id, cancellationToken);

                return TypedResults.Ok(readImage);
            })
            .WithName("GetImage")
            .MapToApiVersion(1);

        images.MapPut(pattern: "{id}/name", async ([FromRoute] ImageId id, [FromBody] UpdateNameCommand command,
            [FromServices] IUpdateNameHandler updateNameHandler, CancellationToken cancellationToken) =>
        {
            var readImage = await updateNameHandler.UpdateAsync(id, command.NewName, cancellationToken);

            return TypedResults.Ok(readImage);
        })
        .AddEndpointFilter<ValidationFilter<UpdateNameCommand>>()
        .WithName("UpdateName")
        .MapToApiVersion(1);
        
        return images;
    }
}