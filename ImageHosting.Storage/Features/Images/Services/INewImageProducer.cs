using System;
using System.Threading;
using System.Threading.Tasks;
using ImageHosting.Storage.Features.Images.Models;

namespace ImageHosting.Storage.Features.Images.Services;

public interface INewImageProducer : IDisposable
{
    Task SendAsync(NewImage newImage, CancellationToken cancellationToken = default);
}