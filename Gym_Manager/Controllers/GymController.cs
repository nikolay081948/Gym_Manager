using Data_Gym_Manager;
using Data_Gym_Manager.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gym_Manager.ViewModels.Gym;

namespace Gym_Manager.Controllers
{

    public class GymController : Controller
    {

        private readonly GymDbContext context;

        public GymController(GymDbContext context)
        {
            this.context = context;
        }

        
            
        public async Task<IActionResult> Index()
        {
            var gyms = await context.Gyms//зала БД
                .Where(h => !h.IsDeleted)//Вземи само залите, при които IsDeleted е false
                .ToListAsync();

            var model = new List<GymIndexViewModel>();//празен списък за ViewModel-ите

            foreach (var gym in gyms)
            {
                model.Add(new GymIndexViewModel
                {
                    Id = gym.Id,
                    Name = gym.Name,
                    Address = gym.Address,
                    Email = gym.Email,
                    PhoneNumber = gym.PhoneNumber,
                    MembersCapacity = gym.MembersCapacity
                });
            }

            return View(model);//Изпращаме списъка към View
        }
        

        public async Task<IActionResult> Details(int id)
        {
            var gym = await context.Gyms.FindAsync(id);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new GymDetailsViewModel
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Email = gym.Email,
                PhoneNumber = gym.PhoneNumber,
                MembersCapacity = gym.MembersCapacity,
                CreatedOn = gym.CreatedOn
            };

            return View(model);
        }

        public IActionResult Create()//метод  който показва празната форма.
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GymCreateViewModel model)//обработва POST заявка
        {
            if (ModelState.IsValid)//Проверява валидацията
            {
                var gym = new Gym
                {
                    Name = model.Name,
                    Address = model.Address,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    MembersCapacity = model.MembersCapacity,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false
                };

                context.Gyms.Add(gym);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var gym = await context.Gyms.FindAsync(id);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new GymEditViewModel
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Email = gym.Email,
                PhoneNumber = gym.PhoneNumber,
                MembersCapacity = gym.MembersCapacity
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, GymEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var gym = await context.Gyms.FindAsync(id);

                if (gym == null)
                {
                    return NotFound();
                }

                gym.Name = model.Name;
                gym.Address = model.Address;
                gym.Email = model.Email;
                gym.PhoneNumber = model.PhoneNumber;
                gym.MembersCapacity = model.MembersCapacity;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)//Сигурни ли сте, че искате да изтриете тази зала?
        {
            var gym = await context.Gyms.FindAsync(id);

            if (gym == null)
            {
                return NotFound();
            }

            var model = new GymDeleteViewModel
            {
                Id = gym.Id,
                Name = gym.Name
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gym = await context.Gyms.FindAsync(id);

            if (gym != null)
            {
                gym.IsDeleted = true;

                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}


