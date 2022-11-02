using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchiffeVersenkenKonsole
{
    internal class Program
    {
        private static readonly ushort maxSchiffe = 4;
        private static Spiel sp = new Spiel(8, 8);
        private static readonly ushort maxSchüsse = 4;
        static void Main(string[] args)
        {
            sp.InputSpielID();
            sp.WaitForEnemyID();
            sp.Setzeschiffe(maxSchiffe);
            sp.WaitForReady();
            sp.SetzteSchüsse(maxSchüsse);
            Console.Read();
        }

        
    }
}
