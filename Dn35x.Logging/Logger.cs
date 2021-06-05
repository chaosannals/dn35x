using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Dn35x.Logging
{
    public class Logger
    {
        public string Folder { get; private set; }
        public Logger(string folder)
        {
            Folder = Path.GetFullPath(folder);
        }
    }
}
