using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchiffeVersenkenKonsole
{
    internal class Canvas
    {
        public string Titel { get; set; }
        private int _x;
        private int _y;
        private ConsoleColor moovingColor;
        private int[,] static_objects;
        private ConsoleColor[,] static_colors;
        private int _breite;
        private int _höhe;
        private string _drawingelement;
        public ConsoleColor BackGroundColor { get; set; }

        public Canvas(int breite, int höhe)
        {
            _breite = breite;
            _höhe = höhe;
            Console.Title = "SchiffeVersenken";
            _drawingelement = "█";
            static_objects = new int[breite, höhe];
            static_colors = new ConsoleColor[breite, höhe];
            _x = -1;
            _y = -1;
        }

        public void Mooving(int x, int y, ConsoleColor color)
        {
            _x = x;
            _y = y;
            moovingColor = color;
        }

        public void AdStatic(int x, int y, ConsoleColor color)
        {
            static_objects[x, y] = 1;
            static_colors[x, y] = color;
        }


        public void Show()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Titel);
            Console.CursorVisible = false;
            for (int k = 0; k < _höhe; k++)
            {
                for (int l = 0; l < _breite; l++)
                {
                    Console.ForegroundColor = BackGroundColor;
                    if (static_objects[l, k] == 1)
                        Console.ForegroundColor = static_colors[l, k];
                    if (_x == l && _y == k)
                        Console.ForegroundColor = moovingColor;
                    Console.Write("{0,2}", _drawingelement);
                }
                Console.WriteLine();

            }
        }

        internal void NoMooving()
        {
            _x = -1;
            _y = -1;
        }
    }
}
