using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ImageHosting.Storage.Http;

public class ValidationFilter<T>(IValidator<T> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var argument = context.Arguments.OfType<T>().FirstOrDefault();
        if (argument is not null)
        {
            var validationResult = await validator.ValidateAsync(argument);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary(),
                    statusCode: StatusCodes.Status422UnprocessableEntity);
            }
        }

        return await next(context);
    }
}