using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchiffeVersenkenKonsole
{
    internal class Zeitgeber
    {
        private int _milisec;
        public int Milisec { get => _milisec; set => _milisec = value; }
        Thread th;
        public Zeitgeber(int milsec)
        {
            _milisec = milsec;
        }
        public Zeitgeber(double sec)
        {
            _milisec = (int)(sec * 1000);
        }
        public void Start()
        {
            th = new Thread(IStart);
            th.IsBackground = true;
            th.Start();
        }

        public void Stop()
        {
            th.Abort();
        }

        private void IStart()
        {
            while (true)
            {
                Thread.Sleep(_milisec);
                OnTick();
            }
        }

        public delegate void TickHandler();
        public event TickHandler Tick;

        private void OnTick()
        {
            if (Tick != null)
            {
                Tick();
            }
        }
    }
}
