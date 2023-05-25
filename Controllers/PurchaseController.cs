using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IResult AddCategorys([FromBody] Purchase purchase)
        {
            try
            {
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
        public IEnumerable<Purchase> GetPurchases()
        {
            return _unitOfWork.PurchaseRepository.GetAll();
        }
        [HttpGet]
        [Route("GetPurchaseByUserId")]
        [Authorize(Roles = UserRoles.User)]
        public async Task<IEnumerable<Purchase>> GetPurchaseByIdUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = _unitOfWork.PurchaseRepository.GetPurchaseByName(user.Id.ToString());
            foreach ( var item in result )
            {
                item.FkGadgets=_unitOfWork.GadgetRepository.GetId(item.FkGadgetsId);
            }
            return result;
        }
    }
}
