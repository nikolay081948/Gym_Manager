using System.ComponentModel.DataAnnotations;

namespace Gym_Manager.ViewModels.Gym
{
    public class GymCreateViewModel
    {
        [Required(ErrorMessage = "Името е задължително")]
        [StringLength(50, ErrorMessage = "Максимум 50 символа")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Адресът е задължителен")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Имейлът е задължителен")]
        [EmailAddress(ErrorMessage = "Невалиден имейл")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Телефонът е задължителен")]
        [Phone(ErrorMessage = "Невалиден телефон")]
        public string PhoneNumber { get; set; }

        [Range(1, 5000, ErrorMessage = "Капацитетът трябва да е между 1 и 5000")]
        public int MembersCapacity { get; set; }
    }
}
