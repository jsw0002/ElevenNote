﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Models
{
    public class NoteDetailViewModel
    {
        [Display(Name = "Note Id")]
        public int NoteId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        [UIHint("Starred")]
        [Display(Name = "Favorite")]
        public bool IsStarred { get; set; }

        [Display(Name = "Created")]
        public DateTimeOffset CreatedUtc { get; set; }

        [Display(Name = "Modified")]
        public DateTimeOffset? ModifiedUtc { get; set; }
    }
}
