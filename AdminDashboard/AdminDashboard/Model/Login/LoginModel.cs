using System.ComponentModel.DataAnnotations;

namespace DSELN.Models.Login
{
    // Student 저장/처리용 Model 
    public class LoginModel : BaseModel 
    {
        [Required(ErrorMessage = "Please enter User Id")]
        public string? USER_ID { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string? PWD { get; set; }
        public string? ROLE { get; set; }

    }
}
