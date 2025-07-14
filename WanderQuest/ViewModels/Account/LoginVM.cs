using System;
using System.ComponentModel.DataAnnotations;

namespace WanderQuest.ViewModels.Account
{
    public class LoginVM
    {
        [Required, MaxLength(100), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }

        public bool IsPersistent { get; set; }
    }
}
