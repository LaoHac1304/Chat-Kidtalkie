using System.ComponentModel.DataAnnotations;

namespace ChatKid.Application.Models
{
    public class UserTokenResponse
    {
        [Required(ErrorMessage = "Token Is Required")]
        public string UserProfileToken { get; set; }
    }
}
