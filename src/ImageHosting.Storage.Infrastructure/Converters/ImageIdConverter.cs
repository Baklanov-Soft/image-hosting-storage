using ImageHosting.Storage.Domain.ValueTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageHosting.Storage.Infrastructure.Converters;


public class ImageIdConverter() : ValueConverter<ImageId, Guid>(userId => userId.Id, guid => new ImageId(guid));