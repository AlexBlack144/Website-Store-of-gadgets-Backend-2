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
namespace WebApplicationClient.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class GadgetController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        //CacheService _cacheService = new CacheService();
        static string accessKey = "AKIAXTVX5PM2WVUPY72A";
        static string secretKey = "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";
        static string bucket = "web-imgs-kursak";
        string UserName = "";

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

        public GadgetController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        ///POST

        [HttpPost]
        [Route("UploadImg")]
        [Authorize(Roles = UserRoles.Manager)]
        public async Task<string> AddFile([FromForm]IFormFile file)
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
        [Route("AddGadget")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult Add([FromBody]Gadget gadget)
        {   
            try
            {
                _unitOfWork.GadgetRepository.Create(gadget);
                var gadgetsSql = _unitOfWork.GadgetRepository.GetAll();
                //_cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost]
        [Route("UpdateGadgetbyId")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult Update([FromBody]Gadget gadget)
        {
            try
            {
                _unitOfWork.GadgetRepository.Update(gadget);
                var gadgetsSql = _unitOfWork.GadgetRepository.GetAll();
                //_cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]
        [Route("RemoveGadgetbyId")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult RemoveById([FromBody]int id)
        {
            try
            {
                _unitOfWork.GadgetRepository.Delete(id);
                var gadgetsSql = _unitOfWork.GadgetRepository.GetAll();
                //_cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost]
        [Route("BuyGadgets")]
       // [Authorize(Roles = UserRoles.User)]
        public IResult BuyGadgets([FromBody]List<BasketGadget> basketGadgets)
        {
            try
            {
                List<Gadget> gadgets = new List<Gadget>();
                foreach (var item in basketGadgets)
                {
                    if (item.Sold == null)
                    {
                        item.Sold = 0;
                    }
                    if ((item.Quantity - item.Count) == 0)
                    {
                        gadgets.Add(new Gadget(item.Id, item.Image, item.Name, item.Model, item.Price, (item.Quantity - item.Count), (item.Sold + item.Count), false, item.IdCategory));
                    }
                    else
                    {
                        gadgets.Add(new Gadget(item.Id, item.Image, item.Name, item.Model, item.Price, (item.Quantity - item.Count), (item.Sold + item.Count), item.Status, item.IdCategory));
                    }
                }
                foreach (var item in gadgets)
                {
                    _unitOfWork.GadgetRepository.Update(item);
                }
                var gadgetsSql = _unitOfWork.GadgetRepository.GetAll();
                //_cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        ///GET

        [HttpGet]
        [Route("GetGadgets")]
        public IEnumerable<Gadget> GetGadgets()
        {
           /* List<Gadget> gadgets = _cacheService.GetData<List<Gadget>>("Gadget");
            if (gadgets == null)
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetAll();
                if (gadgetsSql.Count() > 0)
                {
                    _cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                    gadgets = gadgetsSql.ToList();
                }
            }
            return gadgets;*/
            return _unitOfWork.GadgetRepository.GetAll();
        }

        [HttpGet]
        [Route("GetGadgetbyId")]
        public Gadget GetGadgetById(int id)
        {
          /*  Gadget gadgets = _cacheService.GetData<Gadget>("Gadget");
            if (gadgets == null)
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetId(id);
                if (gadgetsSql != null)
                {
                    _cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                    gadgets = gadgetsSql;
                }
            }
            else
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetId(id);
                gadgets = gadgetsSql;
            }
            return gadgets;*/
            return _unitOfWork.GadgetRepository.GetId(id);
        }

        [HttpGet]
        [Route("GetGadgetbyId_Category")]
        public IEnumerable<Gadget> GetGadgetByIdCategory(int id)
        {
           /* List<Gadget> gadgets = _cacheService.GetData<List<Gadget>>("Gadget");
            if (gadgets == null)
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetbyIdCategory(id);
                if (gadgetsSql.Count() > 0)
                {
                    _cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                    gadgets = gadgetsSql.ToList();
                }
            }
            else {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetbyIdCategory(id);
                gadgets = gadgetsSql.ToList();
            }
            return gadgets;*/
            return _unitOfWork.GadgetRepository.GetbyIdCategory(id);
        }

        [HttpGet]
        [Route("GetGadgetbyName")]
        public IEnumerable<Gadget> GetGadgetByName(string name)
        {
           /* List<Gadget> gadgets = _cacheService.GetData<List<Gadget>>("Gadget");
            if (gadgets == null)
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetGadgetByName(name);
                if (gadgetsSql.Count() > 0)
                {
                    _cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                    gadgets = gadgetsSql.ToList();
                }
                else { gadgets = _unitOfWork.GadgetRepository.GetAll().ToList(); }
            }
            else
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetGadgetByName(name);
                gadgets = gadgetsSql.ToList();
            }
            return gadgets;*/
            return _unitOfWork.GadgetRepository.GetGadgetByName(name);
        }

        [HttpPost]
        [Route("GetGadgetFilter")]
        public IEnumerable<Gadget> GetGadgetFilter([FromBody]FilterGadgets filter)
        {
            /*List<Gadget> gadgets = _cacheService.GetData<List<Gadget>>("Gadget");
            if (gadgets == null)
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetGadgetFilter(filter.nameModels, filter.min, filter.max);
                if (gadgetsSql.Count() > 0)
                {
                    _cacheService.SetData("Gadget", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                    gadgets = gadgetsSql.ToList();
                }
            }
            else
            {
                var gadgetsSql = _unitOfWork.GadgetRepository.GetGadgetFilter(filter.nameModels, filter.min, filter.max);
                gadgets = gadgetsSql.ToList();
            }
            return gadgets;*/
            return _unitOfWork.GadgetRepository.GetGadgetFilter(filter.nameModels, filter.min, filter.max);
        }
    }
}