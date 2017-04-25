﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.api
{
    public class FileResult
    {
        public IEnumerable<string> FileNames { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedTimestamp { get; set; }
        public DateTimeOffset UpdatedTimestamp { get; set; }
        public string DownloadLink { get; set; }
        public IEnumerable<string> ContentTypes { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}