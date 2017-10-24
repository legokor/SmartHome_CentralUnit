﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    internal class Unit
    {
        public string Id { get; set; }
        public string Location { get; set; } = "";
        public string Type { get; set; } = "";
        public bool Auto { get; set; } = true;
    }
}
