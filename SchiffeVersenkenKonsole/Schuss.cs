using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchiffeVersenkenKonsole
{
    internal class Schuss : Object
    {
        public Schuss() : base(ConsoleColor.Red)
        {

        }
        public Schuss(int x, int y) : base(x,y,ConsoleColor.Red)
        {

        }
    }
}
