using BadUI.Data.Dtos;
using BadUI.Data.ViewModels;
using BadUI.Services;
using Microsoft.AspNetCore.Components;

namespace BadUI.Pages
{
    public partial class Register
    {
        [Inject]
        private IBadUIService BadUIService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public RegisterVm Vm { get; set; } = new RegisterVm();

        public async Task RegisterIn()
        {
            StatusDto statusDto = await BadUIService.CreateUser(Vm.Username, Vm.Password, Vm.Gender);

            if (!statusDto.IsSuccess)
            {
                Vm.ErrorMessage = statusDto.Message;
            }
            else
            {
                NavigationManager.NavigateTo("/login");
            }
        }
    }
}
