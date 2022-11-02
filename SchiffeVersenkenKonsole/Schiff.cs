using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SchiffeVersenkenKonsole
{
    internal class Schiff : Object
    {
        public bool Getroffen { get; set; }
        public Schiff():base(ConsoleColor.Cyan)
        {
            Getroffen = false;
        }
    }
}
