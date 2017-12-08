using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication8.Data;
using WebApplication8.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication8.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: Announcements
        public async Task<IActionResult> Index()
        {
            return View(await _context.Announcements.ToListAsync());
        }

        // GET: Announcements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Announcements announcements = await _context.Announcements
                .SingleOrDefaultAsync(m => m.Id == id);
            if (announcements == null)
            {
                return NotFound();
            }

            AnnouncementsDetailsViewModel viewModel = await GetAnnouncementsDetailsViewModelFromAnnouncements(announcements);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details([Bind("AnnouncementsId,FirstName,LastName")]
        AnnouncementsDetailsViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                Comments comments = new Comments();

                
                comments.FirstName = viewModel.FirstName;
                comments.LastName = viewModel.LastName;

                Announcements announcements = await _context.Announcements
                .SingleOrDefaultAsync(m => m.Id == viewModel.AnnouncementsId);

                if (announcements == null)
                {
                    return NotFound();
                }

                comments.MyAnnouncements = announcements;
                _context.Comments.Add(comments);
                await _context.SaveChangesAsync();

                viewModel = await GetAnnouncementsDetailsViewModelFromAnnouncements(announcements);


            }

            return View(viewModel);
        }

        private async Task<AnnouncementsDetailsViewModel> GetAnnouncementsDetailsViewModelFromAnnouncements(Announcements announcements)
        {
            AnnouncementsDetailsViewModel viewModel = new AnnouncementsDetailsViewModel();

            viewModel.Announcements = announcements;

            List<Comments> comments = await _context.Comments
                .Where(m => m.MyAnnouncements == announcements).ToListAsync();

            viewModel.Comments = comments;
            return viewModel;
        }

        // GET: Announcements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Announcements announcements)
        {
            if (ModelState.IsValid)
            {
                _context.Add(announcements);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(announcements);
        }

        // GET: Announcements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcements = await _context.Announcements.SingleOrDefaultAsync(m => m.Id == id);
            if (announcements == null)
            {
                return NotFound();
            }
            return View(announcements);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Announcements announcements)
        {
            if (id != announcements.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(announcements);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnnouncementsExists(announcements.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(announcements);
        }

        // GET: Announcements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcements = await _context.Announcements
                .SingleOrDefaultAsync(m => m.Id == id);
            if (announcements == null)
            {
                return NotFound();
            }

            return View(announcements);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var announcements = await _context.Announcements.SingleOrDefaultAsync(m => m.Id == id);
            _context.Announcements.Remove(announcements);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool AnnouncementsExists(int id)
        {
            return _context.Announcements.Any(e => e.Id == id);
        }
    }
}
