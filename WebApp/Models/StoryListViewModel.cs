using System;
using System.Collections.Generic;
using FicRecs.DatabaseLib;

namespace FicRecs.WebApp.Models
{
    public class StoryListViewModel
    {
        public IEnumerable<StoryDetails> Fics { get; set; }

        public bool ShowDetailed { get; set; }

        public bool ShowSimilarButton { get; set; }
    }
}