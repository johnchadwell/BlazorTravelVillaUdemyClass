using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TravelVillaClient.Service.IService;

namespace TravelVillaClient.Pages.Authentication
{
    public partial class Login
    {
        private AuthDTO UserForAuthentication = new AuthDTO();
        public bool IsProcessing { get; set; } = false;
        public bool ShowAuthenticationErrors { get; set; }
        public string Errors { get; set; }
        public string ReturnUrl { get; set; }
        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        private async Task LoginUser()
        {
            ShowAuthenticationErrors = false;
            IsProcessing = true;
            var result = await authenticationService.Login(UserForAuthentication);
            if (result.IsAuthSuccess)
            {
                IsProcessing = false;

                var absoluteUri = new Uri(navigationManager.Uri);
                var queryParam = HttpUtility.ParseQueryString(absoluteUri.Query);
                ReturnUrl = queryParam["returnUrl"];
                if (string.IsNullOrEmpty(ReturnUrl))
                {
                    navigationManager.NavigateTo("/");
                }
                else
                {
                    navigationManager.NavigateTo("/" + ReturnUrl);
                }
            }
            else
            {
                IsProcessing = false;
                Errors = result.ErrorMsg;
                ShowAuthenticationErrors = true;
            }
        }
    }
}