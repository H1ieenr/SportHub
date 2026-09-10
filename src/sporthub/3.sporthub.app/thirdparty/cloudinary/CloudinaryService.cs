using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet.Actions;
using sporthub.app.contracts;
namespace sporthub.app
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly string _rootFolder;

        public CloudinaryService(IConfiguration config)
        {
            var acc = new Account(
                config["Cloudinary:CloudName"],
                config["Cloudinary:ApiKey"],
                config["Cloudinary:ApiSecret"]
            );
            _cloudinary = new Cloudinary(acc);
            _rootFolder = config["Cloudinary:RootFolder"] ?? "webdev_uploads";
        }

        public async Task<CloudinaryResponse> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                return new CloudinaryResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "File không hợp lệ hoặc trống."
                };
            }

            try
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = $"{_rootFolder}{folderName}",
                    Transformation = new Transformation().Quality("auto").FetchFormat("auto")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    return new CloudinaryResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = uploadResult.Error.Message
                    };
                }

                return new CloudinaryResponse
                {
                    IsSuccess = true,
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl?.ToString()
                };
            }
            catch (Exception ex)
            {
                return new CloudinaryResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Lỗi hệ thống: {ex.Message}"
                };
            }
        }

        public async Task<List<CloudinaryResponse>> UploadImagesAsync(List<IFormFile> files, string folderName)
        {
            var responses = new List<CloudinaryResponse>();

            foreach (var file in files)
            {
                var response = await UploadImageAsync(file, folderName);
                responses.Add(response);
            }
            return responses;
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
            if (string.IsNullOrEmpty(publicId)) return false;

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            return result.Result == "ok";
        }

        public async Task<List<CloudinaryResponse>> DeleteImagesAsync(List<string> publicIds)
        {
            var responses = new List<CloudinaryResponse>();

            foreach (var publicId in publicIds)
            {
                var isSuccess = await DeleteImageAsync(publicId);
                responses.Add(new CloudinaryResponse { IsSuccess = isSuccess });
            }

            return responses;
        }
    }
}