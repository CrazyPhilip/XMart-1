using System.IO;
using System.Threading.Tasks;

namespace XMart.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
