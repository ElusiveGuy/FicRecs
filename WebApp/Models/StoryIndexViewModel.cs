using System;
using System.Collections.Generic;
using FicRecs.DatabaseLib;

namespace FicRecs.WebApp.Models
{
    public class StoryIndexViewModel
    {
        public IEnumerable<StoryDetails> Fics { get; set; }

        public bool ShowDetailed { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }
}