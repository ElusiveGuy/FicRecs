using System;
using System.Collections.Generic;

namespace FicRecs.foo
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
