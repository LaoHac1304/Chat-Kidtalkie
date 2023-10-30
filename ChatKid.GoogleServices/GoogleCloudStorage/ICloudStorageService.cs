using ChatKid.GoogleServices.GoogleCloudStorage.ViewModel;
using Microsoft.AspNetCore.Http;

namespace ChatKid.GoogleServices.GoogleCloudStorage
{
    public interface ICloudStorageService
    {
        Task<FileViewModel> UploadFile(IFormFile file);
    }
}
