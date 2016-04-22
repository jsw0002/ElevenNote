using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        // Can only set a Guid here when initializing or in a constructor (below).
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        // We return IEnumerable because the application doesn't care what kind it is (as opposed to list or something else)
        // Also called coding to the interface (gives you greater flexibility in the long run)
        public IEnumerable<NoteListItemViewModel> GetNotes()
        {
            // ctx means context, txn is an abbreviation for transaction
            using (var ctx = new ElevenNoteDbContext())
            {
                //constructing a query
                return
                    ctx
                        .Notes
                        // Filtering the notes in the database for the below OwnerId
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new NoteListItemViewModel
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    IsStarred = e.IsStarred,
                                    CreatedUtc = e.CreatedUtc
                                })
                            .ToArray();
            }
        }

        public bool CreateNote(NoteCreateViewModel vm)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    new NoteEntity
                    {
                        OwnerId = _userId,
                        Title = vm.Title,
                        Content = vm.Content,
                        CreatedUtc = DateTimeOffset.UtcNow
                    };
                // Gets added into the notes table
                ctx.Notes.Add(entity);
                // Validate that we only added one row
                return ctx.SaveChanges() == 1;
            }
        }

        public NoteDetailViewModel GetNoteById(int noteId)
        {
            NoteEntity entity;

            using (var ctx = new ElevenNoteDbContext())
            {
                entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == noteId);
            }

            return
                new NoteDetailViewModel
                {
                    NoteId = entity.NoteId,
                    Title = entity.Title,
                    Content = entity.Content,
                    IsStarred = entity.IsStarred,
                    CreatedUtc = entity.CreatedUtc,
                    ModifiedUtc = entity.ModifiedUtc
                };
        }

        public bool UpdateNote(NoteDetailViewModel vm)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == vm.NoteId);

                entity.Title = vm.Title;
                entity.Content = vm.Content;
                entity.IsStarred = vm.IsStarred;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int NoteId)
        {
            using (var ctx = new ElevenNoteDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.OwnerId == _userId && e.NoteId == NoteId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
