using Microsoft.AspNet.Identity;
using OwinFormsAuthenticationTutorial.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace OwinFormsAuthenticationTutorial.Controllers
{
    public class AuthenticationController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!ValidateUser(model.Login, model.Password))
            {
                ModelState.AddModelError(string.Empty, "Le nom d'utilisateur ou le mot de passe est incorrect.");
                return View(model);
            }

            // L'authentification est réussie, 
            // injecter les informations utilisateur dans le cookie d'authentification :
            var userClaims = new List<Claim>();
            // Identifiant utilisateur :
            userClaims.Add(new Claim(ClaimTypes.NameIdentifier, model.Login));
            // Rôles utilisateur :
            userClaims.AddRange(LoadRoles(model.Login));
            var claimsIdentity = new ClaimsIdentity(userClaims, DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(claimsIdentity);

            // Rediriger vers l'url d'origine :
            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                return Redirect(ViewBag.ReturnUrl);
            // Par défaut, rediriger vers la page d'accueil :
            return RedirectToAction("Index", "Home");
        }

        private bool ValidateUser(string login, string password)
        {
            // TODO : Insérer ici la validation des identifiant et mot de passe de l'utilisateur...

            // Pour ce tutoriel, j'utilise une validation extrêmement sécurisée...
            return login == password;
        }

        private IEnumerable<Claim> LoadRoles(string login)
        {
            // TODO : Charger ici les rôles de l'utilisateur...

            // Pour ce tutoriel, je considère que l'utilisateur a les rôles "Contributeur" et "Modérateur" :
            yield return new Claim(ClaimTypes.Role, "Contributeur");
            yield return new Claim(ClaimTypes.Role, "Modérateur");
        }

        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();

            // Rediriger vers la page d'accueil :
            return RedirectToAction("Index", "Home");
        }
    }
}