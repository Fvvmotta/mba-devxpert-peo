using System.ComponentModel.DataAnnotations;

namespace MBA_DevXpert_PEO.Api.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}
