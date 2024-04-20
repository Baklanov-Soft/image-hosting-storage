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
            .WithTags("Images")
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
            .WithTags("Images")
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
            .WithTags("Images")
            .MapToApiVersion(1);

        images.MapPut(pattern: "{id}/name", async ([FromRoute] ImageId id, [FromBody] UpdateNameCommand command,
                [FromServices] IUpdateNameHandler updateNameHandler, CancellationToken cancellationToken) =>
            {
                var readImage = await updateNameHandler.UpdateAsync(id, command.NewName, cancellationToken);
                return Results.Ok(readImage);
            })
            .AddEndpointFilter<ValidationFilter<UpdateNameCommand>>()
            .WithName("UpdateName")
            .WithTags("Images")
            .Produces<ReadImage>()
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .MapToApiVersion(1);

        var tags = images.MapGroup(prefix: "{id}/tags");

        tags.MapPost(pattern: "", handler: async ([FromRoute] ImageId id, [FromBody] AddTagsCommand addTagsCommand,
                [FromServices] IAddTagsHandler addTagsHandler, CancellationToken cancellationToken) =>
            {
                var response = await addTagsHandler.AddAsync(id, addTagsCommand.Tags, cancellationToken);
                return TypedResults.Ok(response);
            })
            .WithName("AddTag")
            .WithTags("Tags")
            .MapToApiVersion(1);

        tags.MapDelete(pattern: "",
                handler: async ([FromRoute] ImageId id, [FromQuery] string[] tag,
                    [FromServices] IDeleteTagsHandler deleteTagsHandler, CancellationToken cancellationToken) =>
                {
                    await deleteTagsHandler.DeleteAsync(id, tag, cancellationToken);
                    return TypedResults.Ok();
                })
            .WithName("DeleteTags")
            .WithTags("Tags")
            .MapToApiVersion(1);

        return images;
    }
}