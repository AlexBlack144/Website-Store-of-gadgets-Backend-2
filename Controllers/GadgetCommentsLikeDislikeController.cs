using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApplicationClient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GadgetCommentsLikeDislikeController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public GadgetCommentsLikeDislikeController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
        }

        ///GET
        
        [HttpGet]
        [Route("GetAllGadgetsCommentsLikesDis")]
        public IEnumerable<GadgetCommentsLikeDislike> GetAllGadgetsCommentsLikesDis()
        {
            return _unitOfWork.GadgetCommentsLikesDislikesRepository.GetAll();
        }

        [HttpGet]
        [Route("GetCommentsLikesDisByIdGadget")]
        public IEnumerable<GadgetCommentsLikeDislike> GetCommentsLikesDisByIdGadget(int id)
        {
            return _unitOfWork.GadgetCommentsLikesDislikesRepository.GetbyIdGadget(id);
        }

        [HttpGet]
        [Route("GetCommentsLikesDisByIdUser")]
        public IEnumerable<GadgetCommentsLikeDislike> GetCommentsLikesDisByIdUser(string id)
        {
            return _unitOfWork.GadgetCommentsLikesDislikesRepository.GetbyIdUser(id);
        }

        [HttpGet]
        [Route("GetCommentsLikesDisById")]
        public GadgetCommentsLikeDislike GetCommentsLikesDisById(int id)
        {
            return _unitOfWork.GadgetCommentsLikesDislikesRepository.GetId(id);
        }

        ///POST

        [HttpPost]
        [Route("AddComment")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IResult> AddComment([FromBody] Comments comment)
        {
            try
            {
                DateTime dateNow = DateTime.Today;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                GadgetCommentsLikeDislike comments = new GadgetCommentsLikeDislike(comment.GadgetId, user.Id, user.UserName, comment.Comment, false, false, DateTime.Now.ToString());
                _unitOfWork.GadgetCommentsLikesDislikesRepository.Create(comments);
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost]
        [Route("DeleteComment")]
        [Authorize(Roles = $"{UserRoles.User}, {UserRoles.Manager}")]
        public async Task<IResult> DeleteComment([FromBody] Comments comment)
        {
            try
            {
                
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                var userRole = _userManager.GetRolesAsync(user);
                var commentsSql = _unitOfWork.GadgetCommentsLikesDislikesRepository.GetAll();
                foreach (var item in commentsSql)
                {
                    if (comment.Id == item.Id && user.Id == item.FkAspNetUsersId)
                    {
                        _unitOfWork.GadgetCommentsLikesDislikesRepository.Delete(item.Id);
                    }
                    else if (userRole.Result[0] =="Manager")
                    {
                        _unitOfWork.GadgetCommentsLikesDislikesRepository.Delete(item.Id);
                    }
                }
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

        }

        [HttpPost]
        [Route("AddLikeOrDis")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IResult> AddLikeOrDis([FromBody] LikeOrDis likeOrDis)
        {
            try
            {
                
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                GadgetCommentsLikeDislike like_or_dis = new GadgetCommentsLikeDislike(likeOrDis.GadgetId, user.Id, user.UserName, "",likeOrDis.Like,likeOrDis.Dis,null);
                bool found = false;
                var like_or_disSql = _unitOfWork.GadgetCommentsLikesDislikesRepository.GetAll();
                foreach (var item in like_or_disSql)
                {
                    if (item.FkGadgetsId == like_or_dis.FkGadgetsId && item.FkAspNetUsersId == like_or_dis.FkAspNetUsersId)
                    {
                        if (item.IsLiked == like_or_dis.IsLiked && item.IsDisliked == like_or_dis.IsDisliked)
                        {
                            item.IsLiked = like_or_dis.IsLiked;
                            item.IsDisliked = like_or_dis.IsDisliked;
                            _unitOfWork.GadgetCommentsLikesDislikesRepository.Delete(item.Id);  
                        }
                        else 
                        {
                            item.IsLiked = like_or_dis.IsLiked;
                            item.IsDisliked = like_or_dis.IsDisliked;
                            _unitOfWork.GadgetCommentsLikesDislikesRepository.Update(item);
                        }
                        found = true;
                        break;
                    }
                    else
                    {
                        found = false;
                    }
                }
                if (found == false)
                {
                    _unitOfWork.GadgetCommentsLikesDislikesRepository.Create(like_or_dis);
                    }
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }

        }


    }
}
