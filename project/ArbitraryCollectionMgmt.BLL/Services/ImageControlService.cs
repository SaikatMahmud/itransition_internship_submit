using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace ArbitraryCollectionMgmt.BLL.Services
{
    public class ImageControlService
    {
        private static readonly Cloudinary cloudinary;
        static ImageControlService()
        {
            cloudinary = new Cloudinary("cloudinary://168489166584648:pge21Opm5-yS8GSPRxfy6saJaOM@dxnpfy9oy");
            cloudinary.Api.Secure = true;
        }

        public static string UploadCollectionImage(IFormFile collectionImage, string? existingUrl = null)
        {
            string publicId;
            if (string.IsNullOrEmpty(existingUrl))
            {
                publicId = "collections/" + Guid.NewGuid().ToString();
            }
            else
            {
                var urlParts = existingUrl.Split("/");
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(urlParts.Last());
                publicId = urlParts[urlParts.Length - 2] + "/" + fileNameWithoutExtension;
            }
            using (var imageStream = collectionImage.OpenReadStream())
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(collectionImage.FileName, imageStream),
                    PublicId = publicId,
                    UseFilename = false,
                    //UniqueFilename = true,
                    Overwrite = true
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                if (uploadResult.Error != null) return string.Empty;
                return uploadResult.Url.ToString();
            }
        }
        public static bool DeleteCollectionImage(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return false;
            var urlParts = imageUrl.Split("/");
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(urlParts.Last());
            var publicId = urlParts[urlParts.Length - 2] + "/" + fileNameWithoutExtension;
            var deleteParams = new DeletionParams(publicId)
            {
                //Type = "upload",
                ResourceType = ResourceType.Image,
                PublicId = publicId,
            };
            var deleteResult = cloudinary.Destroy(deleteParams);
            return deleteResult.Result == "ok";
        }
    }
}
