
using Data_Gym_Manager;
using Data_Gym_Manager.Entities;
using Gym_Manager.ViewModels.Member;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gym_Manager.Controllers
{
    public class MemberController : Controller
    {
        private readonly GymDbContext context;
        public MemberController(GymDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var members=await context.Members
                .Include(p=>p.TrainerMembers)
                .ThenInclude(dp=>dp.Trainer)
                .ToListAsync();
            var model=new List<MemberIndexViewModel>();
            foreach (var pat in members)
            {
                model.Add(new MemberIndexViewModel
                {
                    Id = pat.Id,
                    FirstName = pat.FirstName,
                    LastName = pat.LastName,
                    Email = pat.Email,
                    PhoneNumber = pat.PhoneNumber,
                    TrainerNames = string.Join(", ", pat.TrainerMembers.Select(dp => dp.Trainer.FirstName + " " + dp.Trainer.LastName))
                });
            }
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            var member = await context.Members
                .Include(p => p.TrainerMembers)
                .ThenInclude(dp => dp.Trainer)
                .FirstOrDefaultAsync(p=>p.Id == id);
            if(member == null)
            {
                return NotFound();
            }
            var model = new MemberDetailsViewModel
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                TrainerNames = string.Join(", ", member.TrainerMembers.Select(dp => dp.Trainer.FirstName + " " + dp.Trainer.LastName))
            };
            return View(model);
        }
        public async Task<IActionResult> Create()
        {
            await LoadTrainers();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(MemberCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var member = new Member
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber

                };
                foreach (var item in model.TrainerIds)
                {
                    member.TrainerMembers.Add(new TrainerMember
                    {
                        TrainerId = item
                    });
                    
                }
                context.Members.Add(member);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadTrainers(model.TrainerIds);
            return View(model);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var member=await context.Members
                .Include(p=>p.TrainerMembers)
                .FirstOrDefaultAsync(p=>p.Id == id);
            if(member == null)
            {
                return NotFound();
            }
            var model = new MemberEditViewModel
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName,
                Email = member.Email,
                PhoneNumber = member.PhoneNumber,
                TrainerIds = member.TrainerMembers
                .Select(dp => dp.TrainerId).ToList()

            };
            await LoadTrainers(model.TrainerIds);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MemberEditViewModel model)
        {

            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var member = await context.Members.Include(p => p.TrainerMembers).FirstOrDefaultAsync(p => p.Id == id);
                if (member == null)
                {
                    return NotFound();
                }

                member.FirstName = model.FirstName;
                member.LastName = model.LastName;
                member.Email = model.Email;
                member.PhoneNumber = model.PhoneNumber;

                var existingTrainerIds = member.TrainerMembers.Select(dp => dp.TrainerId).ToList();
                var newTrainerIds = model.TrainerIds ?? new List<int>();

                foreach (var trainerId in existingTrainerIds.Except(newTrainerIds))
                {
                    var trainerMember = member.TrainerMembers.FirstOrDefault(dp => dp.TrainerId == trainerId);
                    if (trainerMember != null)
                    {
                        context.Remove(trainerMember);
                    }
                }

                foreach (var trainerId in newTrainerIds.Except(existingTrainerIds))
                {
                    context.Add(new Data_Gym_Manager.Entities.TrainerMember { TrainerId = trainerId, MemberId = member.Id });
                }

                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            await LoadTrainers(model.TrainerIds);
            return View(model);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var member=await context.Members.FindAsync(id);
            if(member == null)
            {
                return NotFound();

            }
            var model = new MemberDeleteViewModel
            {
                Id = member.Id,
                FirstName = member.FirstName,
                LastName = member.LastName

            };
            return View(model);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await context.Members.FindAsync(id);
            if(member != null)
            {
                context.Members.Remove(member);
                await context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }




        public async Task LoadTrainers(List<int> selectedTrainers = null)
        {
            var trainers = await context.Trainers
                .Select(d => new
                {
                    d.Id,
                    FullName = d.FirstName + " " + d.LastName
                }).ToListAsync();
            ViewBag.Trainers = new MultiSelectList(trainers, "Id", "FullName", selectedTrainers);

        }
    }
}
