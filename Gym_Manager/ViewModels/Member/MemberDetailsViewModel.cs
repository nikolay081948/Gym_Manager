using System.ComponentModel.DataAnnotations;

namespace Gym_Manager.ViewModels.Member
{
    public class MemberDetailsViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string TrainerNames {  get; set; }
    }
}
