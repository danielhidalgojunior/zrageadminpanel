﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticResources
{
    public static class Variables
    {
        public static bool CommandAddedOrUpdated { get; set; }
        public static object LoggedUser { get; set; }
        public static string NextMap { get; set; }
        public static string Map { get; set; }
        public static int MapLevel { get; set; }

        public static Settings Settings { get; set; }
    }
}
