using System.ComponentModel.DataAnnotations;

namespace MyPatchAPI.Models
{
    public class VerifyLoginParameter
    {
        [Required]
        public string UserID { get; set; }
        [Required]
        public string Password { get; set; }
        
        public string MacAddr { get; set; }
    }
}