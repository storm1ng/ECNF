﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib.Util
{
    public static class TextReaderExtension
    {
        public static IEnumerable<string[]> GetSplittedLines(this TextReader tr, char splitter)
        {
            string[] lines = tr.ReadToEnd().Split('\n');

            foreach (var s in lines)
                yield return s.Split(splitter);
        }
    }
}
