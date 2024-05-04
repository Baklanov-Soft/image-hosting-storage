using ImageHosting.Storage.Domain.ValueTypes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ImageHosting.Storage.Infrastructure.Converters;

public class UserIdConverter() : ValueConverter<UserId, Guid>(userId => userId.Id, guid => new UserId(guid));