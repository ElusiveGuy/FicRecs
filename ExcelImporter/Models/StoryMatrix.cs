using System;
using System.Collections.Generic;

namespace FicRecs_ExcelImporter.Models
{
    public partial class StoryMatrix
    {
        public int StoryA { get; set; }
        public int StoryB { get; set; }
        public float? Similarity { get; set; }

        public StoryDetails StoryANavigation { get; set; }
        public StoryDetails StoryBNavigation { get; set; }
    }
}
