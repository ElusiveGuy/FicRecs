using System;
using System.Collections.Generic;
using FicRecs.DatabaseLib;

namespace FicRecs.WebApp.Models
{
    public class StorySimilarViewModel
    {
        public IEnumerable<StoryDetails> SimilarFics { get; set; }

        public StoryDetails SelectedFic { get; set; }

        public IEnumerable<StoryDetails> SelectedFicAsEnumerable => new StoryDetails[] { SelectedFic };

        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }
    }
}