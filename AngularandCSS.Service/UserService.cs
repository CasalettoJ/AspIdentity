using AngularandCSS.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using AngularandCSS.Service.ViewModels;
using DevOne.Security.Cryptography.BCrypt;
using System.Net.Mail;

namespace AngularandCSS.Service
{
    public class UserService : ServiceBase
    {
        private static UserManager<User> _userManager { get; set; }
        private static RoleManager<IdentityRole> _roleManager { get; set; }

        public UserService( DataContext dataContext) : base(dataContext)
        {
            _userManager = new UserManager<User>(new UserStore<User>(dataContext));
            _userManager.UserValidator = new UserValidator<User>(_userManager){
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dataContext));
        }

        #region Signin / signout / register / delete

        public async Task<RegistrationResultViewModel> Register(RegisterViewModel model)
        {
            try
            {
                User newUser = new User()
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    CustomEmailConfirmation = false
                };
                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
                await SendConfirmationEmail(newUser);
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

        public async Task<bool> DeleteUser(User user)
        {
            try
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    EmailConfirmation confirmation = _dataContext.EmailConfirmations.Where(ec => ec.UserID == user.Id).FirstOrDefault();
                    if (confirmation != null)
                    {
                        _dataContext.EmailConfirmations.Remove(confirmation);
                        _dataContext.SaveChanges();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                //log here.
                return false;
            }
        }


        public async Task<bool> SignIn(User user, bool isPersistant, IAuthenticationManager authenticationManager)
        {
            try
            {
                if (_userManager.FindByIdAsync(user.Id).Result.CustomEmailConfirmation)
                {
                    authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                    var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistant }, identity);
                    return true;
                }
                else
                {
                    await SendConfirmationEmail(user);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Log here.
                return false;
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

        #region user confirmation / user password actions

        public async Task SendConfirmationEmail(User user)
        {
            try
            {
                string confirmationToken = BCryptHelper.GenerateSalt();
                EmailConfirmation confirmation = _dataContext.EmailConfirmations.Where(ec => ec.UserID == user.Id).FirstOrDefault();
                if (confirmation == null)
                {
                    EmailConfirmation newConfirmation = new EmailConfirmation()
                    {
                        ConfirmationToken = confirmationToken,
                        UserID = user.Id
                    };
                    _dataContext.EmailConfirmations.Add(newConfirmation);
                    _dataContext.SaveChanges();
                }
                else
                {
                    confirmationToken = confirmation.ConfirmationToken;
                }
                EmailFactory.SendConfirmationEmail(user.Email, user.Id, confirmationToken);
            }
            catch
            {
                //Log here.
            }
        }

        public async Task<bool> ConfirmEmail(string userID, string confirmationToken)
        {
            try
            {
                EmailConfirmation confirmation = _dataContext.EmailConfirmations.Where(ec => ec.ConfirmationToken == confirmationToken && ec.UserID == userID).FirstOrDefault();
                if(confirmation != null)
                {
                    _dataContext.EmailConfirmations.Remove(confirmation);
                    User user = _dataContext.Users.Where(u => u.Id == userID).FirstOrDefault();
                    if(user != null)
                    {
                        user.CustomEmailConfirmation = true;
                        _dataContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    }
                    _dataContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                //Log here.
                return false;
            }
        }

        #endregion

        #region user database functions

        public async Task<User> GetUserFromViewModel(LoginViewModel model)
        {
            try
            {
                User user = await _userManager.FindByEmailAsync("ossia.gaming@gmail.com");
                return await _userManager.FindAsync(model.UserName, model.Password);
            }
            catch (Exception ex)
            {
                //Log here.
                return null;
            }
        }


        #endregion
    }
}
