using System.ComponentModel.DataAnnotations;

namespace Gym_Manager.ViewModels.Member
{
    public class MemberCreateViewModel
    {
        [Required(ErrorMessage = "Първото име е задължително.")]
        [StringLength(30, ErrorMessage = "Първото име не може да е повече от 30 букви.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Фамилното име е задължително.")]
        [StringLength(30, ErrorMessage = "Фамилното име не може да е повече от 30 букви.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Имейлът е задължителен.")]
        [EmailAddress(ErrorMessage = "Въведи валиден имейл.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Телефонният номер е задължителен.")]
        [Phone(ErrorMessage = "Въведи валиден телефонен номер.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Моля, изберете поне един треньор.")]
        [MinLength(1, ErrorMessage = "Моля, изберете поне един треньор.")]
        public List<int> TrainerIds {  get; set; }= new List<int>();
    }
}
