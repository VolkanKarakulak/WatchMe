﻿using WatchMe.Models;

namespace WatchMe.ViewModels
{
    public class MediaPostViewModel
    {
        public Media? Media { get; set; }
        public List<int>? StarIds { get; set; }
        public List<short>? CategoryIds { get; set; }
        public List<int>? DirectorIds { get; set; }
    }
}
