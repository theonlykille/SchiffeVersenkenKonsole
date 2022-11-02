using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.Drawing.Configuration;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace SchiffeVersenkenKonsole
{
    class Object
    {
        protected int _x;
        protected int _y;
        protected ConsoleColor _color;
        public int X { get => _x; set => _x = value; }
        public int Y { get => _y; set => _y = value; }
        public bool initialized { get; set; }   
        public ConsoleColor Color { get => _color; set => _color = value; }

        public Object(ConsoleColor color)
        {
            _x = 0;
            _y = 0;
            _color = color;
        }

        public Object(int x, int y, ConsoleColor color)
        {
            Color = color;
            _x = x;
            _y = y;
            initialized = true;
        }

        public void SetObject(int[,] koords)
        {
            initialized = true;
            PositionhasChanged();
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                string keys = key.Key.ToString();
                if (keys == "RightArrow")
                {
                    if (_x < 7)
                        _x++;
                    else
                        _x = 0;
                }
                else if (keys == "LeftArrow")
                {
                    if (_x > 0)
                        _x--;
                    else
                        _x = 7;
                }
                else if (keys == "DownArrow")
                {
                    if (_y < 7)
                        _y++;
                    else
                        _y = 0;
                }
                else if (keys == "UpArrow")
                {
                    if (_y > 0)
                        _y--;
                    else
                        _y = 7;
                }
                else if (keys == "Enter" && koords[_x, _y] != 1)
                {
                    PositionhasChanged();
                    return;
                }
                PositionhasChanged();
            }
        }


        public delegate void PositionChangedHandler(Object source);
        public event PositionChangedHandler PositionChanged;

        protected void PositionhasChanged()
        {
            if (PositionChanged != null)
            {
                PositionChanged(this);
            }
        }

    }

}
