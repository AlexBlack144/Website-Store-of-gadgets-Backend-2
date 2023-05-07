using DataAccessEF.Data;
using DataAccessEF.Repositories;
using DataAccessEF.UnitOfWork;
using Domain.Interfaces;
using Domain.Models;
using Domain.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace WebApplicationClient.Controllers
{
    [Route("[controller]")]
    [ApiController, Authorize]
    public class UserController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        public static List<int> usersAuto = new List<int>();

        public UserController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /*[HttpPost]
        [Route("Register User")]
        public IResult Add(string name, string login, string email, string password )
        {
            try
            {
                if (!_db.Users.Any(x => x.Login.Equals(login.ToLower())))
                {
                    if (!_db.Users.Any(x => x.Email.Equals(email.ToLower())))
                    {
                        _db.Users.Add(new User() { Name = name, Login = login, Email = email, Password = password, IdGadget = null });
                        _db.SaveChanges();
                        return Results.StatusCode(StatusCodes.Status200OK);
                    }
                    else return Results.StatusCode(StatusCodes.Status400BadRequest);
                }
                else return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
            catch { return Results.StatusCode(StatusCodes.Status404NotFound); }
            
        }*/

        /*[HttpGet]
        [Route("Authorization User")]
        public IResult Authorization(string Login_or_Email, string Password)
        {
            usersAuto.Clear();
            try
            {
                if (_db.Users.Any(x => x.Login.Equals(Login_or_Email)) || _db.Users.Any(x => x.Email.Equals(Login_or_Email)))
                {
                    if (_db.Users.Any(x => x.Password.Equals(Password)))
                    {
                        var user = _db.Users.AsEnumerable().Where(x => x.Login == Login_or_Email || x.Email == Login_or_Email).Select(y => y.Id);
                        foreach (var item in user)
                        {
                            usersAuto.Add(item);
                        }
                        return Results.StatusCode(StatusCodes.Status200OK);
                    }
                    else return Results.StatusCode(StatusCodes.Status400BadRequest);
                }
                else return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
            catch { return Results.StatusCode(StatusCodes.Status404NotFound); }
        }*/

        [HttpGet]
        [Route("Buygadget")]
        public IResult BuyGadget(int id_gadget)
        {
            try
            {
                _unitOfWork.UserRepository.Buy(id_gadget, usersAuto[0]);
                return Results.StatusCode(StatusCodes.Status200OK);
            }
            catch
            {
                return Results.StatusCode(StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = UserRoles.Admin)]
        public IEnumerable<User> GetUsers()
        {
            return _unitOfWork.UserRepository.GetAll();
        }

        [HttpGet]
        [Route("GetUsersbyId")]
        [Authorize(Roles = UserRoles.Admin)]
        public User GetUserById(int id)
        {
            return _unitOfWork.UserRepository.GetId(id);
        }
    }
}
