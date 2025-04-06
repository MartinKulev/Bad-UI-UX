namespace BadUI.Data.ViewModels
{
    public class LoginVm : BaseVm
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Letter { get; set; }

        public string Hangman { get; set; }

        public string HangmanProgress { get; set; }

        public int HangmanFails { get; set; }
    }
}
