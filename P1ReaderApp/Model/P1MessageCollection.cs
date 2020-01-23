using System;
using System.Collections.Generic;

namespace P1ReaderApp.Model
{
    public class P1MessageCollection
    {
        public List<string> Messages { get; set; }

        public DateTime ReceivedUtc { get; set; }
    }
}