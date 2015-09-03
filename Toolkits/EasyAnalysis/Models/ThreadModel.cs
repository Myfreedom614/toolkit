﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyAnalysis.Models
{
    public class ThreadModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime CreateOn { get; set; }

        public int? TypeId { get; set; }
        
        public string ForumId { get; set; }

        public string AuthorId { get; set; }
    }

    public class ThreadViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string Type { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}