using System;
using System.ComponentModel.DataAnnotations;

namespace travel_service.Dtos
{
    public class LoginDto
    {
        public LoginDto()
        {
        }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
