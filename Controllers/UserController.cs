using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApplicationClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }
        [HttpPost]
        [Route("GetUserIdLikeOrDis")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<string> GetUserIdLikeOrDis([FromBody] int itemId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var like_or_disSql = _unitOfWork.GadgetCommentsLikesDislikesRepository.GetAll();
            string found = "none";
            foreach (var item in like_or_disSql)
            {
                if (item.IsLiked == true && item.FkAspNetUsersId == user.Id && item.FkGadgetsId== itemId)
                {
                    found = "Like";
                }
                else if (item.IsDisliked == true && item.FkAspNetUsersId == user.Id && item.FkGadgetsId == itemId)
                {
                    found =  "Dislike";
                } 
            }
            return found;
                
        }
        ///GET
        [HttpGet]
        [Route("GetUserId")]
        [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Manager}")]
        public async Task<string> GetUserId()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var userRole = _userManager.GetRolesAsync(user);
            string id = "";
            if (userRole.Result[0] == "Manager")
            {
                id = "Manager";
            }
            else
            {
                id = user.Id.ToString();
            }
            return id;

        }

    }
}
