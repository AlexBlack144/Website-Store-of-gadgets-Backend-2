using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DataAccessEF.Data;
using DataAccessEF.Repositories;
using Domain.Models;
using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using WebApplicationClient.Cach;
using Domain.Roles;

namespace WebApplicationClient.Controllers
{
    
    [Route("[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
       
        private IUnitOfWork _unitOfWork;
        //CacheService _cacheService = new CacheService();

        public CategoryController(IUnitOfWork unitOfWork)
        {
           this._unitOfWork = unitOfWork;
        }
        //Post

        [HttpPost]
        [Route("AddCategorys")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult AddCategorys([FromBody]Category category)
        {
            try
            {
                _unitOfWork.CategoryRepository.Create(category);
                var gadgetsSql = _unitOfWork.CategoryRepository.GetAll();
                //_cacheService.SetData("Category", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
            return Results.StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost]
        [Route("RemoveCategorybyId")]
        [Authorize(Roles = UserRoles.Manager)]
        public IResult RemoveById([FromBody] int id)
        {
            try
            {
                var resGadget = _unitOfWork.GadgetRepository.GetbyIdCategory(id);
                if (resGadget.Count()==0)
                {

                    _unitOfWork.CategoryRepository.Delete(id);
                    //var gadgetsSql = _unitOfWork.CategoryRepository.GetAll();
                    //_cacheService.SetData("Category", gadgetsSql, DateTimeOffset.Now.AddDays(1));
                    return Results.StatusCode(StatusCodes.Status200OK);
                }
                else {
                    return Results.StatusCode(StatusCodes.Status400BadRequest);
                }
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        //Get

        [HttpGet]
        [Route("GetCategorys")]
        public IEnumerable<Category> GetCategorys()
        {
            /*List<Category> category = _cacheService.GetData<List<Category>>("Category");
            if (category == null)
            {
                var categorysSql = _unitOfWork.CategoryRepository.GetAll();
                if (categorysSql.Count() > 0)
                {
                    _cacheService.SetData("Category", categorysSql, DateTimeOffset.Now.AddDays(1));
                    category = categorysSql.ToList();
                }
            }
            return category;*/
            return _unitOfWork.CategoryRepository.GetAll();
        }
        [HttpGet]
        [Route("GetCategorysbyId")]
        public Category GetCategoryById(int id)
        {
            /*Category category = _cacheService.GetData<Category>("Category");
            if (category == null)
            {
                var categorysSql = _unitOfWork.CategoryRepository.GetId(id);
                if (categorysSql != null)
                {
                    _cacheService.SetData("Category", categorysSql, DateTimeOffset.Now.AddDays(1));
                    category = categorysSql;
                }
            }
            else
            {
                var categorysSql = _unitOfWork.CategoryRepository.GetId(id);
                category = categorysSql;
            }
            return category;*/
            return _unitOfWork.CategoryRepository.GetId(id);
        }
    
    }
}
