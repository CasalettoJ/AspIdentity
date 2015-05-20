using AngularandCSS.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using AngularandCSS.Service.ViewModels;
using DevOne.Security.Cryptography.BCrypt;

namespace AngularandCSS.Service
{
    public class UserService : ServiceBase
    {
        private UserManager<User> _userManager { get; set; }
        private RoleManager<IdentityRole> _roleManager { get; set; }

        public UserService( DataContext dataContext) : base(dataContext)
        {
            _userManager = new UserManager<User>(new UserStore<User>(dataContext));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dataContext));
        }

        #region Signin / signout / register

        public async Task<RegistrationResultViewModel> Register(RegisterViewModel model)
        {
            try
            {
                User newUser = new User()
                {
                    Email = model.Email,
                    UserName = model.UserName
                };
                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
                return new RegistrationResultViewModel()
                {
                    Result = result,
                    User = newUser
                };
            }
            catch (Exception ex)
            {
                //Log here.
                return new RegistrationResultViewModel()
                {
                    Result = new IdentityResult(ex.Message),
                    User = null
                };
            }
        }

        public async Task DeleteUser(User user)
        {
            await _userManager.DeleteAsync(user);
        }


        public async Task SignIn(User user, bool isPersistant, IAuthenticationManager authenticationManager)
        {
            try
            {
                authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistant }, identity);
            }
            catch (Exception ex)
            {
                //Log here.
            }
        }

        public async Task<User> GetUserFromViewModel(LoginViewModel model)
        {
            try
            {
                return await _userManager.FindAsync(model.UserName, model.Password);
            }
            catch(Exception ex)
            {
                //Log here.
                return null;
            }
        }

        public async Task SignOut(IAuthenticationManager authenticationManager)
        {
            try
            {
                await Task.Run(() =>
                {
                    authenticationManager.SignOut();
                });
            }
            catch (Exception ex)
            {
                //Log here.
            }
        }


        #endregion
    }
}
