using Microsoft.AspNetCore.Components;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelVillaClient.Service.IService;

namespace TravelVillaClient.Pages.Authentication
{
    public partial class Register
    {
        private UserRequestDTO UserForRegisteration = new UserRequestDTO();
        public bool IsProcessing { get; set; } = false;
        public bool ShowRegistrationErrors { get; set; }
        public IEnumerable<string> Errors { get; set; }

        [Inject]
        public IAuthenticationService authenticationService { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        private async Task RegisterUser()
        {
            ShowRegistrationErrors = false;
            IsProcessing = true;
            var result = await authenticationService.RegisterUser(UserForRegisteration);
            if (result.IsRegSuccess)
            {
                IsProcessing = false;
                navigationManager.NavigateTo("/login");
            }
            else
            {
                IsProcessing = false;
                Errors = result.Errors;
                ShowRegistrationErrors = true;
            }
        }
    }
}
