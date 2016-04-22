using ElevenNote.Models;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.Controllers.WebApi
{
    [Authorize]
    [RoutePrefix("api/Notes")]
    public class NotesController : ApiController
    {
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

        [Route]
        public IEnumerable<NoteListItemViewModel> Get()
        {
            return _svc.Value.GetNotes();
        }

        // When we get a get request for api/Notes/1, we will execute this method
        [Route("{id}")]
        public NoteDetailViewModel Get(int id)
        {
            return _svc.Value.GetNoteById(id);
        }

        [Route]
        public bool Post(NoteCreateViewModel vm)
        {
            return _svc.Value.CreateNote(vm);
        }

        [Route("{id}")]
        public bool Put(int id, NoteDetailViewModel vm)
        {
            return _svc.Value.UpdateNote(vm);
        }

        [Route("{id}")]
        public bool Delete(int id)
        {
            return _svc.Value.DeleteNote(id);
        }

        private bool SetStarState(int noteId, bool state)
        {
            // Retrieve the note that we're identified.
            var note =
                _svc.Value.GetNoteById(noteId);

            // CHange the value of IsStarred and write.
            note.IsStarred = state;

            return _svc.Value.UpdateNote(note);
        } 

        // Could also put these (star off and on) in a separate controller, but we can wrap them here for simplicity.
        [Route("{id}/Star")]
        [HttpPost]
        public bool ToggleStarOn(int id) => SetStarState(id, true);

        [Route("{id}/Star")]
        [HttpDelete]
        public bool ToggleStarOff(int id) => SetStarState(id, false);
    }
}
