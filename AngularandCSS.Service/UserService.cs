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
        private EmailFactory _emailFactory = new EmailFactory();

        public UserService( DataContext dataContext) : base(dataContext)
        {
            _userManager = new UserManager<User>(new UserStore<User>(dataContext));
            _userManager.UserValidator = new UserValidator<User>(_userManager){
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = true
            };
            _userManager.PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6
            };
            _userManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(10);
            _userManager.MaxFailedAccessAttemptsBeforeLockout = 5;
            _userManager.UserLockoutEnabledByDefault = true;
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
                IdentityResult passwordResult = await CheckPasswordValidity(model.Password);
                if(!passwordResult.Succeeded)
                {
                    return new RegistrationResultViewModel()
                    {
                        Result = passwordResult,
                        User = newUser
                    };
                }
                IdentityResult result = await _userManager.CreateAsync(newUser, model.Password);
                if(result.Succeeded)
                {
                    await SendConfirmationEmail(newUser);
                }
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

        //public async Task<bool> DeleteUser(User user)
        //{
        //    try
        //    {
        //        IdentityResult result = await _userManager.DeleteAsync(user);
        //        if (result.Succeeded)
        //        {
        //            EmailConfirmation confirmation = _dataContext.EmailConfirmations.Where(ec => ec.UserID == user.Id).FirstOrDefault();
        //            if (confirmation != null)
        //            {
        //                _dataContext.EmailConfirmations.Remove(confirmation);
        //                _dataContext.SaveChanges();
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch
        //    {
        //        //log here.
        //        return false;
        //    }
        //}


        public async Task<bool> SignIn(User user, bool isPersistant, IAuthenticationManager authenticationManager)
        {
            try
            {
                if (_userManager.FindByIdAsync(user.Id).Result.CustomEmailConfirmation)
                {
                    if (!await IsUserLockedOut(user.UserName))
                    {
                        authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                        var identity = await _userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistant }, identity);
                        await _userManager.ResetAccessFailedCountAsync(user.Id);
                        return true;
                    }
                    return false;
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

        #region user confirmation / user password  actions

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
                _emailFactory.SendConfirmationEmail(user, confirmationToken);
            }
            catch (Exception ex)
            {
                //Log here.
            }
        }

        public async Task SendPasswordRecoveryEmail(User user)
        {
            try
            {
                string recoveryToken = BCryptHelper.GenerateSalt();
                PasswordRecover recovery = _dataContext.PasswordRecovers.Where(pr => pr.UserID == user.Id).FirstOrDefault();
                if (recovery == null)
                {
                    PasswordRecover newRecovery = new PasswordRecover()
                    {
                        RecoveryToken = recoveryToken,
                        UserID = user.Id,
                        ValidUntil = DateTime.UtcNow.AddDays(1)
                    };
                    _dataContext.PasswordRecovers.Add(newRecovery);
                    _dataContext.SaveChanges();
                }
                else
                {
                    recovery.ValidUntil = DateTime.UtcNow.AddDays(1);
                    recovery.RecoveryToken = recoveryToken;
                    _dataContext.Entry(recovery).State = System.Data.Entity.EntityState.Modified;
                    _dataContext.SaveChanges();
                }
                _emailFactory.SendPasswordRecoveryEmail(user, recoveryToken);
            }
            catch (Exception ex)
            {
                //Log here.
            }
        }


        //Resets a user's password to a new password and returns whether this was a successful action or not.  If a password recovery request exists in the database,
        //then it is removed.  This is only ever used for password recovery requests.
        public async Task<bool> SetUserPassword(string userID, string newPassword, string recoveryToken)
        {
            try
            {
                PasswordRecover recover = _dataContext.PasswordRecovers.Where(pr => pr.UserID == userID && pr.RecoveryToken == recoveryToken).FirstOrDefault();
                if (recover != null)
                {
                    IdentityResult result = await _userManager.RemovePasswordAsync(userID);
                    if (result.Succeeded)
                    {
                        IdentityResult additionResult = await _userManager.AddPasswordAsync(userID, newPassword);
                        if (additionResult.Succeeded)
                        {
                            _dataContext.PasswordRecovers.Remove(recover);
                            _dataContext.SaveChanges();
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                //Log here.
                return false;
            }
        }


        //Takes in a user's ID and a recovery token and checks the recovery table to make sure that such a request by a user exists, that the token
        //is correct, and that the request has not expired.
        public async Task<bool> CheckRecoveryValidity(string userID, string recoveryToken)
        {
            try
            {
                PasswordRecover recovery = _dataContext.PasswordRecovers.Where(pr => pr.UserID == userID && pr.RecoveryToken == recoveryToken && DateTime.UtcNow <= pr.ValidUntil).FirstOrDefault();
                if (recovery != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //Log here.
                return false;
            }
        }

        //Determines whether a proposed password is valid for a given user and returns true or false.
        public async Task<IdentityResult> CheckPasswordValidity(string password)
        {
            try
            {
                return await _userManager.PasswordValidator.ValidateAsync(password);
            }
            catch (Exception ex)
            {
                //Log here.
                return new IdentityResult("There was an issue checking the validity of the password");
            }
        }

        //Activates a user account if the confirmation token given is correct and removes the confirmation request from the database.
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
            catch (Exception ex)
            {
                //Log here.
                return false;
            }
        }

        #endregion

        #region user database functions

        public async Task<User> GetUserFromViewModelForLogin(LoginViewModel model)
        {
            try
            {
                User user = await _userManager.FindAsync(model.UserName, model.Password);
                if(user == null)
                {
                    User userToLockout = await GetUserFromUsername(model.UserName);
                    if (userToLockout != null)
                    {
                        await _userManager.AccessFailedAsync(userToLockout.Id);
                    }
                }
                return user;
            }
            catch (Exception ex)
            {
                //Log here.
                return null;
            }
        }

        public async Task<bool> IsUserLockedOut(string username)
        {
            try
            {
                User user = await GetUserFromUsername(username);
                if (user != null)
                {
                    return await _userManager.IsLockedOutAsync(user.Id);
                }
                return false;
            }
            catch(Exception ex)
            {
                //Log here.
                return false;
            }
        }

        public async Task<User> GetUserFromEmail(string email)
        {
            try
            {
                return await _userManager.FindByEmailAsync(email);
            }
            catch (Exception ex)
            {
                //Log here.
                return null;
            }
        }

        public async Task<User> GetUserFromUsername(string username)
        {
            try
            {
                return await _userManager.FindByNameAsync(username);
            }
            catch (Exception ex)
            {
                //Log here.
                return null;
            }
        }

        public async Task<User> GetUserFromID(string userID)
        {
            try
            {
                User user = await _userManager.FindByIdAsync(userID);
                return user;
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
