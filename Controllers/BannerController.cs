using Amazon.S3.Model;
using Amazon.S3;
using DataAccessEF.Data;
using DataAccessEF.Repositories;
using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Blob;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using WebApplicationClient.Cach;
using static Azure.Core.HttpHeader;
using Microsoft.AspNetCore.Identity;

namespace WebApplicationClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        static string accessKey = "AKIAXTVX5PM2WVUPY72A";
        static string secretKey = "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";
        static string bucket = "baner-kursak";
        //static string bucket = "web-design-kursak";

        AmazonS3Client s3Client = new AmazonS3Client(accessKey, secretKey, Amazon.RegionEndpoint.EUWest2);

        static async Task<bool> UploadFileAsync(IAmazonS3 client, string bucketName, string objectName, Stream source)
        {
            var request = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = source,
                CannedACL = S3CannedACL.PublicRead,
            };
            var response = await client.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"Successfully uploaded {objectName} to {bucketName}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Could not upload {objectName} to {bucketName}.");
                return false;
            }
        }
        public static string GetFileUrlAsync(IAmazonS3 client, string bucketName, string objectName)
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = bucketName;
            request.Key = objectName;
            request.Expires = DateTime.Now.AddHours(1);
            request.Protocol = Protocol.HTTP;
            string url = client.GetPreSignedURL(request);
            Console.WriteLine(url);
            return url;
        }

        public BannerController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        //POST
        [HttpPost]
        [Route("UploadImg")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<string> AddFile([FromForm] IFormFile file)
        {
            if (file != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    memoryStream.ToArray();
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    await UploadFileAsync(s3Client, bucket, file.FileName, memoryStream);
                }
                return GetFileUrlAsync(s3Client, bucket, file.FileName); ;
                ////////////////////////////Asure
                /*                string blobName = "";
                                blobName = await _unitOfWork.GadgetRepository.UploadImg(file, blobName);
                                return _unitOfWork.GadgetRepository.GetImgUrl(blobName);*/
            }
            else { return "400 code"; }
        }

        [HttpPost]
        [Route("AddBanner")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult Add([FromBody] BannerFromFront banner_from_front)
        {
            try
            {
                Banner banner = new Banner(banner_from_front.Id, banner_from_front.FkGadgetsId, banner_from_front.ImgUrl);
                _unitOfWork.BannerRepository.Create(banner);
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost]
        [Route("UpdateBannerbyId")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult Update([FromBody] BannerFromFront banner_from_front)
        {
            try
            {
                Banner banner = new Banner(banner_from_front.Id, banner_from_front.FkGadgetsId, banner_from_front.ImgUrl);
                _unitOfWork.BannerRepository.Update(banner);
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]
        [Route("RemoveBannerbyId")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult RemoveById([FromBody] int id)
        {
            try
            {
                _unitOfWork.BannerRepository.Delete(id);
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }
        
        //GET
        [HttpGet]
        [Route("GetBanners")]
        public IEnumerable<Banner> GetBanners()
        {
            return _unitOfWork.BannerRepository.GetAll();
        }
    }
}
