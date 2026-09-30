using Data_Gym_Manager;
using Data_Gym_Manager.Entities;
using Gym_Manager.ViewModels.Trainer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Gym_Manager.Controllers
{
    public class TrainerController : Controller
    {
        private readonly GymDbContext context;

        public TrainerController(GymDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var trainers = await context.Trainers
                .Include(d => d.Gym)
                .ToListAsync();

            var model = new List<TrainerIndexViewModel>();

            foreach (var trainer in trainers)
            {
                model.Add(new TrainerIndexViewModel
                {
                    Id = trainer.Id,
                    FirstName = trainer.FirstName,
                    LastName = trainer.LastName,
                    Specialty = trainer.Specialty,
                    GymName = trainer.Gym.Name
                });
            }

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var trainer = await context.Trainers
                .Include(d => d.Gym)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (trainer == null)
            {
                return NotFound();
            }

            var model = new TrainerDetailsViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Specialty = trainer.Specialty,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                GymName = trainer.Gym.Name
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Gyms = new SelectList(
            await context.Gyms
         .Where(h => !h.IsDeleted)
         .ToListAsync(),
             "Id",
            "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TrainerCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var trainer = new Trainer
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Specialty = model.Specialty,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    GymId = model.GymId
                };

                context.Trainers.Add(trainer);
                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            //Вземи залите от БД и ги сложи във ViewBag.Gyms.
                        ViewBag.Gyms = new SelectList(
                await context.Gyms
                    .Where(h => !h.IsDeleted)
                    .ToListAsync(),
                "Id",
                "Name");

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var trainer = await context.Trainers.FindAsync(id);

            if (trainer == null)
            {
                return NotFound();
            }

            var model = new TrainerEditViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Specialty = trainer.Specialty,
                Email = trainer.Email,
                PhoneNumber = trainer.PhoneNumber,
                GymId = trainer.GymId
            };

            ViewBag.Gyms = new SelectList(
                    await context.Gyms
                        .Where(h => !h.IsDeleted)
                        .ToListAsync(),
                    "Id",
                    "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TrainerEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var trainer = await context.Trainers.FindAsync(id);

                if (trainer == null)
                {
                    return NotFound();
                }

                trainer.FirstName = model.FirstName;
                trainer.LastName = model.LastName;
                trainer.Specialty = model.Specialty;
                trainer.Email = model.Email;
                trainer.PhoneNumber = model.PhoneNumber;
                trainer.GymId = model.GymId;

                await context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Gyms = new SelectList(
                  await context.Gyms
                      .Where(h => !h.IsDeleted)
                      .ToListAsync(),
                  "Id",
                  "Name");

            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var trainer = await context.Trainers.FindAsync(id);

            if (trainer == null)
            {
                return NotFound();
            }

            var model = new TrainerDeleteViewModel
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName
            };

            return View(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainer = await context.Trainers.FindAsync(id);

            if (trainer != null)
            {
                context.Trainers.Remove(trainer);
                await context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

