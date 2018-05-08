using System.Threading.Tasks;

namespace BinaryCook.Core.Services
{
    public interface IServiceInitializer
    {
        Task InitializeAsync();
    }
}