using System.Threading.Tasks;

namespace ImageHosting.Storage.Services;

public interface IInitializeKafka
{
    Task CreateNewImageTopic();
}