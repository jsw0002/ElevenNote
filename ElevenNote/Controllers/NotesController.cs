using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElevenNote.Controllers
{
    // Anytime we are routed to the NotesController, authorize will require the user to be logged in
    [Authorize]
    public class NotesController : Controller
    {
        // The type Lazy - we will eventually have a note service that we will define later
        private readonly Lazy<NoteService> _svc;

        public NotesController()
        {
            _svc =
                new Lazy<NoteService>(
                    () =>
                    {
                        // Will match the user with the owner id
                        var userId = Guid.Parse(User.Identity.GetUserId());
                        return new NoteService(userId);
                    }
                );
        }

        // GET: Notes
        public ActionResult Index()
        {
            var notes = _svc.Value.GetNotes();

            return View(notes);
        }

        public ActionResult Create()
        {
            var vm = new NoteCreateViewModel();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteCreateViewModel vm)
        {
            // If it's not valid, show the errors on the view
            if (!ModelState.IsValid) return View(vm);
            // If we're not able to write the note to the database
            if (!_svc.Value.CreateNote(vm))
            {
                ModelState.AddModelError("", "Unable to create note");
                return View(vm);
            }
            // Sending us back to the list page
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            var vm = _svc.Value.GetNoteById(id);

            return View(vm);
        }

        public ActionResult Edit(int id)
        {
            var vm = _svc.Value.GetNoteById(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NoteDetailViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            if (!_svc.Value.UpdateNote(vm))
            {
                ModelState.AddModelError("", "Unable to update note");
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public ActionResult DeleteGet(int id)
        {
            var vm = _svc.Value.GetNoteById(id);

            return View(vm);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            _svc.Value.DeleteNote(id);

            return RedirectToAction("Index");
        }
    }
}