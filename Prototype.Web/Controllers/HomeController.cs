using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Prototype.Web.ViewModels;

namespace Prototype.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public async Task<IActionResult> Index()
        {
            await WriteOutIdentityInformation();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public async Task Logout()
        {
            await HttpContext.Authentication.SignOutAsync("Cookies");
            await HttpContext.Authentication.SignOutAsync("oidc");
        }

        public async Task<IActionResult> OrderItem()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44380/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception("problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(x=>x.Type == "address")?.Value;
            return View(new OrderItemViewModel(address));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminPortal()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44380/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception("problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(x => x.Type == "address")?.Value;
            return View(new AdminPortalViewModel(address));
        }

        [Authorize(Roles = "Client, Admin")]
        public async Task<IActionResult> ClientPortal()
        {
            var discoveryClient = new DiscoveryClient("https://localhost:44380/");
            var metaDataResponse = await discoveryClient.GetAsync();
            var userInfoClient = new UserInfoClient(metaDataResponse.UserInfoEndpoint);
            var accessToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            var response = await userInfoClient.GetAsync(accessToken);

            if (response.IsError)
            {
                throw new Exception("problem accessing the UserInfo endpoint.", response.Exception);
            }

            var address = response.Claims.FirstOrDefault(x => x.Type == "address")?.Value;
            return View(new ClientPortalViewModel(address));
        }

        public async Task WriteOutIdentityInformation()
        {
            // get the saved identity token
            var identityToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            // get the saved access token
            var accessToken = await HttpContext.Authentication.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            // write it out
            Debug.WriteLine($"Identity token: {identityToken} - Access token: {accessToken}");

            // write out the user claims
            foreach (var userClaim in User.Claims)
            {
                Debug.WriteLine($"Claim type: {userClaim.Type} - Claim value: {userClaim.Value}");
            }
        }
    }
}
