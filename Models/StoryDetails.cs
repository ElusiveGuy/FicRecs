using System;
using System.Collections.Generic;

namespace FicRecs.Models
{
    public partial class StoryDetails
    {
        public StoryDetails()
        {
            StoryMatrixStoryANavigation = new HashSet<StoryMatrix>();
            StoryMatrixStoryBNavigation = new HashSet<StoryMatrix>();
        }

        public int StoryId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Summary { get; set; }
        public string Characters { get; set; }
        public short? Chapters { get; set; }
        public int? Words { get; set; }
        public int? Reviews { get; set; }
        public int? Favs { get; set; }
        public int? Follows { get; set; }
        public DateTime? Published { get; set; }
        public bool? Complete { get; set; }
        public string Url { get; set; }

        public ICollection<StoryMatrix> StoryMatrixStoryANavigation { get; set; }
        public ICollection<StoryMatrix> StoryMatrixStoryBNavigation { get; set; }
    }
}
