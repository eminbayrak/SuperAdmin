using SuperAdmin.Models;
using System;
using System.DirectoryServices.AccountManagement;
using System.Web.Mvc;

namespace SuperAdmin.Controllers
{
    public class CreateUserController : Controller
    {
        [AuthorizeWithSession]
        public ActionResult Index()
        {
            var model = new CreateUser();
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(CreateUser model)
        {
            // NetBIOS name of domain. 
            var domainName = "localhost";
            // Full distinguished name of OU to create user in. E.g. OU=Users,OU=Perth,DC=Contoso,DC=com
            var userPosition = model.Building + " " + model.Position;
            var userOU = "OU" + @"\" + model.Building.ToString();
            var serviceUser = "addomainname" + @"\" + "administrator";
            var servicePassword = "password";
            // Source: http://stackoverflow.com/a/2305871
            using (var pc = new PrincipalContext(ContextType.Domain, domainName, serviceUser, servicePassword))
            {
                using (var up = new UserPrincipal(pc))
                {
                    // Create username and display name from firstname and lastname ex. lastname_f
                    var userName = (model.LastName + "_" + (model.FirstName).Remove(1)).ToLower();
                    var displayName = model.FirstName + " " + model.LastName;
                    // Temp password setup
                    var password = "password" + model.SSN.ToString();

                    // Set the values for new user account
                    up.Name = displayName;
                    up.DisplayName = displayName;
                    up.GivenName = model.FirstName;
                    up.Surname = model.LastName;
                    up.SamAccountName = userName;
                    up.UserPrincipalName = userName + "@ADDOMAINNAME.NET";
                    up.EmailAddress = userName + "@webdomainname.org";
                    up.SetPassword(password);
                    up.Enabled = true;
                    up.Description = userPosition;
                    up.ExpirePasswordNow();
                    //DirectoryEntry user = new DirectoryEntry();
                    //user.MoveTo(userOU)
                    //using (GroupPrincipal group = GroupPrincipal.FindByIdentity(up.Context, IdentityType.Name, model.Building.ToString()))                        up.IsMemberOf();

                    //using (UserPrincipal user = UserPrincipal.FindByIdentity(up.Context, IdentityType.Name, userName))
                    try
                    {
                        // Attempt to save the account to AD
                        //group.Members.Add(user);
                        up.Save();
                        ViewBag.Message = "User" + model.Name + " 's account is created";
                        ModelState.Clear();
                    }
                    catch (Exception e)
                    {
                        // Display exception(s) within validation summary
                        ModelState.AddModelError("", "Exception creating user object. " + "This user name already exist in the domain" + e);
                        return View(model);
                    }

                    // Add the building to the newly created AD user
                    // Get the directory entry object for the user
                    //DirectoryEntry de = up.GetUnderlyingObject() as DirectoryEntry;
                    //// Set the department property to the value entered by the user
                    //de.Properties["Building"].Value = model.Building;
                    //try
                    //{
                    //    // Try to commit changes
                    //    de.CommitChanges();
                    //} 
                    //catch (Exception e)
                    //{
                    //    // Display exception(s) within validation summary
                    //    ModelState.AddModelError("", "Exception adding building. " + e);

                    //    return View(model);
                    //}
                }
            }

            // Redirect to completed page if successful
            return RedirectToAction("Completed");
        }

        public ActionResult Completed()
        {
            return View();
        }
    }
}