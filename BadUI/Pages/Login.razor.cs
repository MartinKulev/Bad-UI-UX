using BadUI.Data.Dtos;
using BadUI.Data.ViewModels;
using BadUI.Services;
using Microsoft.AspNetCore.Components;

namespace BadUI.Pages
{
    public partial class Login
    {
        [Inject]
        private IBadUIService BadUIService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Parameter]
        public LoginVm Vm { get; set; } = new LoginVm();


        public async Task LogIn()
        {
            StatusDto statusDto = await BadUIService.Login(Vm.Username, Vm.Password);

            if (!statusDto.IsSuccess)
            {
                Vm.ErrorMessage = statusDto.Message;

                if(!string.IsNullOrEmpty(statusDto.Hangman))
                {
                    Vm.Hangman = statusDto.Hangman;
                    Vm.HangmanProgress = $"{statusDto.Hangman[0] + new string('_', statusDto.Hangman.Length - 2) + statusDto.Hangman[statusDto.Hangman.Length - 1]}";
                }
            }
            else
            {
                NavigationManager.NavigateTo($"/profile?username={Vm.Username}");
            }
        }

        public void Guess()
        {
            char letter = Vm.Letter[0];
            string word = Vm.Hangman;

            if(!word.Contains(letter))
            {
                Vm.HangmanFails++;
            }
            else
            {
                char[] progressArray = Vm.HangmanProgress.ToCharArray();

                for (int i = 0; i < word.Length; i++)
                {
                    if (word[i] == letter)
                    {
                        progressArray[i] = letter;
                    }
                }

                Vm.HangmanProgress = new string(progressArray);
            }

            if (Vm.HangmanFails >= 6)
            {
                BadUIService.DeleteUser(Vm.Username);
            }
        }

        private string DrawHangman(int errors)
        {
            string[] stages = new string[]
            {
            @"
                -----
                |   |
                    |
                    |
                    |
                    |
            =========",
            @"
                -----
                |   |
                O   |
                    |
                    |
                    |
            =========",
            @"
                -----
                |   |
                O   |
                |   |
                    |
                    |
            =========",
            @"
                -----
                |   |
                O   |
               /|   |
                    |
                    |
            =========",
            @"
                -----
                |   |
                O   |
               /|\  |
                    |
                    |
            =========",
            @"
                -----
                |   |
                O   |
               /|\  |
               /    |
                    |
            =========",
            @"
                -----
                |   |
                O   |
               /|\  |
               / \  |
                    |
            ========="
            };

            return errors >= stages.Length ? stages[^1] : stages[errors];
        }
    }
}
