using BadUI.Data.Entities;
using BadUI.Data.ViewModels;
using BadUI.Services;
using Microsoft.AspNetCore.Components;

namespace BadUI.Pages
{
    public partial class Profile
    {
        [Inject]
        private IBadUIService BadUIService { get; set; }

        [Parameter]
        public ProfileVm Vm { get; set; } = new ProfileVm();


        protected override async Task OnInitializedAsync()
        {
            var uri = new Uri(Navigation.Uri);
            var queryParams = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            if (queryParams.TryGetValue("username", out var usernameValue))
            {
                Vm.Username = usernameValue.ToString();
            }

            CustomUser customUser = await BadUIService.GetCustomUser(Vm.Username);
            Vm.Gender = customUser.Gender;

            await base.OnInitializedAsync();
        }
    }
}
