using System.ComponentModel.DataAnnotations;

namespace Gym_Manager.ViewModels.Gym
{
    public class GymDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int MembersCapacity { get; set; }
        [Display(Name = "Създадена на")]
        public DateTime CreatedOn { get; set; }
    }
}
