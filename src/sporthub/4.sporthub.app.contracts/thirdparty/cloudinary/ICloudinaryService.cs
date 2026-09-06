using Microsoft.AspNetCore.Http;

namespace sporthub.app.contracts
{
    public interface ICloudinaryService
    {
        Task<CloudinaryResponse> UploadImageAsync(IFormFile file, string folderName);
        Task<List<CloudinaryResponse>> UploadImagesAsync(List<IFormFile> files, string folderName);
        Task<bool> DeleteImageAsync(string publicId);
        Task<List<CloudinaryResponse>> DeleteImagesAsync(List<string> publicIds);
    }
}