using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApplicationClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public PurchaseController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }
        //Post
        [HttpPost]
        [Route("AddPurchases")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IResult> AddCategorys([FromBody] Purchase purchase)
        {
            try
            {
               
                 purchase.FkAspNetUsers = await _userManager.FindByIdAsync(purchase.FkAspNetUsersId);
                 purchase.FkGadgets = _unitOfWork.GadgetRepository.GetId(purchase.FkGadgetsId);
                _unitOfWork.PurchaseRepository.Create(purchase);
                var gadgetsSql = _unitOfWork.PurchaseRepository.GetAll();
                //_cacheService.SetData("Category", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        //Get

        [HttpGet]
        [Route("GetPurchases")]
        public async Task<IEnumerable<Purchase>> GetPurchases()
        {
            var result = _unitOfWork.PurchaseRepository.GetAll();
            foreach (var item in result)
            {
                item.FkAspNetUsers = await _userManager.FindByIdAsync(item.FkAspNetUsersId);
            }
            return result;
        }
        [HttpGet]
        [Route("GetPurchaseByUserId")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IEnumerable<Purchase>> GetPurchaseByIdUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return _unitOfWork.PurchaseRepository.GetPurchaseByName(user.Id.ToString()); 
        }
        [HttpGet]
        [Route("GetPurchaseByIdCategory")]
        public async Task<IEnumerable<Purchase>> GetPurchaseByIdCategory(int id)
        {
            var result = _unitOfWork.PurchaseRepository.GetAll();
            foreach (var item in result)
            {
                item.FkAspNetUsers = await _userManager.FindByIdAsync(item.FkAspNetUsersId);
            }
            result = result.AsEnumerable().Where(x => x.FkGadgets.IdCategory == id);
            return result;
        }
        [HttpGet]
        [Route("GetPurchaseByBrandModelUser")]
        public async Task<IEnumerable<Purchase>> GetPurchaseByBrandModelUser(string name)
        {
            var result = _unitOfWork.PurchaseRepository.GetPurchaseByBrandModel(name);
            if (result.AsEnumerable().Count() != 0)
            {
                foreach (var item in result)
                {
                    item.FkAspNetUsers = await _userManager.FindByIdAsync(item.FkAspNetUsersId);
                }
                
            }
            else 
            {
                result = _unitOfWork.PurchaseRepository.GetAll();
                foreach (var item in result)
                {
                    item.FkAspNetUsers = await _userManager.FindByIdAsync(item.FkAspNetUsersId);
                }
                result = result.AsEnumerable().Where(x => x.FkAspNetUsers.UserName.ToLower().StartsWith(name));
            }
            return result;
        }

        [HttpPost]
        [Route("GetPurchaseFilter")]
        public async  Task<IEnumerable<Purchase>> GetGadgetFilter([FromBody] FilterPurchase filter)
        {
            List<Purchase> purchase = new List<Purchase>();
            var result = _unitOfWork.PurchaseRepository.GetPurchaseFilter(filter.nameModels, filter.min, filter.max);
            foreach (var item in result)
            {
                item.FkAspNetUsers = await _userManager.FindByIdAsync(item.FkAspNetUsersId);
            }
            if (filter.nameUsers != null) 
            {
                foreach (var user in filter.nameUsers)
                {
                    if (result.AsEnumerable().Any(x => x.FkAspNetUsers.UserName == user) == true)
                    {
                        purchase.AddRange(result.AsEnumerable().Where(x => x.FkAspNetUsers.UserName == user));
                    }
                }
                if (purchase.Count > 0)
                {
                    result = purchase;
                    return result;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                return result;
            }
        }
    }
}
