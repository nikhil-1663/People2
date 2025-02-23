using System.ComponentModel.DataAnnotations;

namespace People2.Models
{
    public class LoginMod
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
