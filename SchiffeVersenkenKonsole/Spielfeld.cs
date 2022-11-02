using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SchiffeVersenkenKonsole
{
    class Spiel
    {
        private uint breite;
        private uint höhe;
        static private List<Schiff> schiffe = new List<Schiff>();
        static private List<Schuss> schüsse = new List<Schuss>();
        static private List<Schuss> enemyschüsse = new List<Schuss>();
        private int[,] koords;
        private int[,] koords2;
        Canvas canvas;
        Canvas canvas2;
        private MQTT mqtt;
        private int ID = new Random().Next(int.MaxValue);
        private int enemyID = 0;
        private bool enemyready = false;
        private int GameID;
        private string sendeprefix;
        private bool testmode = false;

        public Spiel(uint w, uint h)
        {
            breite = w;
            höhe = h;
            canvas = new Canvas((int)breite, (int)höhe);
            canvas2 = new Canvas((int)breite, (int)höhe);
            canvas.Titel = "Dein Spielfeld";
            canvas2.Titel = "Gegnerisches Spielfeld";
            canvas.BackGroundColor = ConsoleColor.Blue;
            canvas2.BackGroundColor = ConsoleColor.Blue;
            mqtt = new MQTT();
            mqtt.MsgRecieved += MesageRecieved;
        }

        public void InputSpielID()
        {
            Console.Write("Enter Game ID: ");
            string temp = Console.ReadLine();
            try
            {
                GameID = Convert.ToInt32(temp);
            }
            catch (FormatException)
            {
                GameID = -1;
                testmode = true;
            }
        }
        public void Setzeschiffe(int anzahl)
        {
            koords = new int[(int)breite, (int)höhe];
            for (int i = 0; i < anzahl; i++)
            {
                Schiff s = new Schiff();
                s.PositionChanged += OnPositionChanged;
                s.SetObject(koords);
                s.PositionChanged -= OnPositionChanged;
                schiffe.Add(s);
                koords[s.X, s.Y] = 1;
                canvas.AdStatic(s.X, s.Y, ConsoleColor.DarkGray);
            }
            canvas.NoMooving();
            Update();
            mqtt.Senden(sendeprefix + "Bereit", "1");
        }

        public void SetzteSchüsse(int anzahl)
        {
            koords2 = new int[(int)breite, (int)höhe];
            for (int i = 0; i < anzahl; i++)
            {
                Schuss s = new Schuss();
                s.PositionChanged += OnPositionChanged2;
                s.SetObject(koords);
                mqtt.Senden(sendeprefix + "Schuss", s.X + "|" + s.Y);
                s.PositionChanged -= OnPositionChanged2;
                schüsse.Add(s);
                koords2[s.X, s.Y] = 1;
                canvas2.AdStatic(s.X, s.Y, ConsoleColor.Yellow);
            }
            canvas2.NoMooving();
            Update();
        }

        private void OnPositionChanged(Object source)
        {
            Console.Clear();
            canvas.Mooving(source.X, source.Y, source.Color);
            canvas.Show();
            canvas2.Show();
        }

        private void Update()
        {
            Console.Clear();
            canvas.Show();
            canvas2.Show();
        }

        private void OnPositionChanged2(Object source)
        {
            Console.Clear();
            canvas.Show();
            canvas2.Mooving(source.X, source.Y, source.Color);
            canvas2.Show();
        }

        public void WaitForEnemyID()
        {
            mqtt.subscribe(GameID + "/getID", 2);
            if (testmode)
                enemyID = -1;
            int n = 0;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            do
            {
                n++;
                Console.Clear();
                Console.Write("Searching Opponent");
                switch (n)
                {
                    case 0:
                        Console.Write('|');
                        break;
                    case 1:
                        Console.Write('/');
                        break;
                    case 2:
                        Console.Write('-');
                        break;
                    case 3:
                        Console.Write("\\");
                        break;
                    case 4:
                        Console.Write('|');
                        break;
                    case 5:
                        Console.Write('/');
                        break;
                    case 6:
                        Console.Write('-');
                        break;
                    case 7:
                        Console.Write('\\');
                        n = 0;
                        break;
                }
                Thread.Sleep(300);
                mqtt.Senden(GameID + "/getID", Convert.ToString(ID));
            } while (enemyID == 0);
            Console.Clear();
            Console.WriteLine("Opponent Found!");
            Console.CursorVisible = true;
            mqtt.unsubscribe(GameID + "/getID");
            sendeprefix = GameID + "/" + ID + "/";
        }
        public void WaitForReady()
        {
            int n = 0;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            do
            {
                n++;
                Console.Clear();
                Console.Write("Waiting for Opponent");
                switch (n)
                {
                    case 0:
                        Console.Write('|');
                        break;
                    case 1:
                        Console.Write('/');
                        break;
                    case 2:
                        Console.Write('-');
                        break;
                    case 3:
                        Console.Write("\\");
                        break;
                    case 4:
                        Console.Write('|');
                        break;
                    case 5:
                        Console.Write('/');
                        break;
                    case 6:
                        Console.Write('-');
                        break;
                    case 7:
                        Console.Write('\\');
                        n = 0;
                        break;
                }
                enemyready = testmode;
                Thread.Sleep(100);
            } while (enemyready == false);
            Console.Clear();
            Console.WriteLine("Opponent Ready!");
            Thread.Sleep(500);
            Console.Clear();
            Console.CursorVisible = true;
            mqtt.subscribe(GameID + "/" + enemyID + "/#", 2);
            //mqtt.subscribe(new string[] { GameID + "/" + enemyID + "/#" }, new byte[] { 2 });
        }


        private void MesageRecieved(object sender, string topic, string message)
        {
            //Console.WriteLine(topic + ": " + message);
            if (topic == GameID + "/getID")
            {
                if (Convert.ToInt32(message) != ID)
                {
                    enemyID = Convert.ToInt32(message);
                }
            }
            else if (topic == GameID + "/" + enemyID + "/Bereit")
            {
                enemyready = true;
            }
            else if (topic == GameID + "/" + enemyID + "/Schuss")
            {
                string[] xy = message.Split('|');
                int _x = Convert.ToInt32(xy[0]);
                int _y = Convert.ToInt32(xy[1]);
                enemyschüsse.Add(new Schuss(_x, _y));
                foreach (Schiff sch in schiffe)
                {
                    if (sch.X == _x && sch.Y == _y)
                    {
                        sch.Color = ConsoleColor.Red;
                        sch.Getroffen = true;
                        schiffe[schiffe.IndexOf(sch)] = sch;
                        break;
                    }
                }
            }
        }

    }
}
