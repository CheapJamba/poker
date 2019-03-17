using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Покер_с_костями
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }


        #region Global Variables
        int widthTo16 = 0;
        int heightTo9 = 0;
        Player Player1 = new Player();
        Player Player2 = new Player();
        Player currentplayer;
        int turn = 1;
        int time = 0;
        int stake = 0;
        bool shift = false;
        string[] help = { "Игра поделена на этапы. В начале раздачи оба игрока бросают все пять костей.",
        "Затем вам предоставляется возможность оценить ситуацию и сделать ставку -",
        "ваш противник будет обязан ответить на нее, добавив соответствующую сумму в банк",
        "или отказаться и отдать вам текущий банк. После ставок начинается стадия вторых бросков.",
        "Игроки по очереди выбирают кости, которые хотят перебросить и перебрасывают выбранные.",
        "Выбирайте кости, кликая на них мышью. Выбранные кости будут обведены кружочками.",
        "По результатам второго броска определяется победитель.",
        "Следуйте подсказкам в тексте кнопки в нижней части игрового поля и вы не потеряетесь.",
        "Комбинации костей по возрастанию старшинства:",
        "Пара - 2 одинаковых значения => 2 пары разных значений => Сет - 3 одинаковых значения =>",
        "Фулл хаус - пара и сет => Малый стрит - комбинация костей 1,2,3,4,5 (порядок не важен) =>",
        "Большой стрит - комбинация костей 2,3,4,5,6 (порядок не важен) => Каре - 4 одинаковых =>",
        "Идеально - 5 одинаковых."
        };
        LeaderBoard board = new LeaderBoard();
        Random rand = new Random();
        enum Scenes { Menu, Game, Records, Enter_Name, Help };
        Scenes scene = Scenes.Menu;
        enum Stages { Draw, Opponent_draw, Stakes, Redraw, Opponent_redraw, End }
        Stages stage = Stages.Draw;
        SolidBrush Menu_Start_brush = new SolidBrush(Color.Snow);
        SolidBrush Menu_Record_brush = new SolidBrush(Color.Snow);
        SolidBrush Menu_Quit_brush = new SolidBrush(Color.Snow);
        SolidBrush Records_Return_brush = new SolidBrush(Color.Snow);
        SolidBrush Game_Stakes_brush = new SolidBrush(Color.Gold);
        SolidBrush Game_GoOn_brush = new SolidBrush(Color.Snow);
        SolidBrush Menu_help_brush = new SolidBrush(Color.Snow);
        string[] scores = new string[10];
        StringBuilder name = new StringBuilder();
        MyButton Menu_Start = new MyButton();
        MyButton Menu_Record = new MyButton();
        MyButton Menu_Close = new MyButton();
        MyButton Game_Redraw_Circles = new MyButton();
        MyButton Records_return = new MyButton();
        MyButton Game_Stakes = new MyButton();
        MyButton Enter_name_Key = new MyButton();
        MyButton Enter_name_Bspace = new MyButton();
        MyButton Enter_name_Space = new MyButton();
        MyButton Game_GoOn = new MyButton();
        MyButton Menu_Help = new MyButton();
        #endregion

        #region Drawing

        string[] alphabet = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

        Pen circle_pen = new Pen(Color.Red, 5);

        public void Keyboard(Graphics g, Font f)
        {
            for (int i = 0; i < 10; i++)
            {
                g.FillRectangle(Brushes.SeaShell, ClientSize.Width / 2 - Convert.ToInt32((3.0625 - 0.625 * i) * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.875 * heightTo9), Convert.ToInt32(widthTo16 * 0.5), Convert.ToInt32(heightTo9 * 0.5));
                g.DrawString(alphabet[i].ToString(), f, Brushes.Black, ClientSize.Width / 2 - Convert.ToInt32((2.9375 - 0.625 * i) * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.8125 * heightTo9));
            }
            for (int i = 0; i < 10; i++)
            {
                g.FillRectangle(Brushes.SeaShell, ClientSize.Width / 2 - Convert.ToInt32((3.0625 - 0.625 * i) * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.25 * heightTo9), Convert.ToInt32(widthTo16 * 0.5), Convert.ToInt32(heightTo9 * 0.5));
                g.DrawString(alphabet[10 + i].ToString(), f, Brushes.Black, ClientSize.Width / 2 - Convert.ToInt32((2.9375 - 0.625 * i) * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.1875 * heightTo9));
            }
            for (int i = 0; i < 6; i++)
            {
                g.FillRectangle(Brushes.SeaShell, ClientSize.Width / 2 - Convert.ToInt32((3.0625 - 0.625 * i) * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.375 * heightTo9), Convert.ToInt32(0.5 * widthTo16), Convert.ToInt32(0.5 * heightTo9));
                g.DrawString(alphabet[20 + i].ToString(), f, Brushes.Black, ClientSize.Width / 2 - Convert.ToInt32((2.9375 - 0.625 * i) * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.4375 * heightTo9));
            }

            g.FillRectangle(Brushes.SeaShell, ClientSize.Width / 2 + Convert.ToInt32(0.6875 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.375 * heightTo9), Convert.ToInt32(1.125 * widthTo16), Convert.ToInt32(0.5 * heightTo9));
            g.FillRectangle(Brushes.SeaShell, ClientSize.Width / 2 + Convert.ToInt32(1.9375 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.375 * heightTo9), Convert.ToInt32(1.125 * widthTo16), Convert.ToInt32(0.5 * heightTo9));
            g.DrawString("Shift", f, Brushes.Black, ClientSize.Width / 2 + Convert.ToInt32(2.0625 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.4375 * heightTo9));
            g.DrawString("Delete", f, Brushes.Black, ClientSize.Width / 2 + Convert.ToInt32(0.725 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.4375 * heightTo9));
            g.FillRectangle(Brushes.SeaShell, ClientSize.Width / 2 - Convert.ToInt32(3.0625 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(1 * heightTo9), Convert.ToInt32(6.125 * widthTo16), Convert.ToInt32(0.5 * heightTo9));
        }

        public void Circles(Graphics g, Pen p, int x, int y, int[] do_circle)
        {
            for (int i = 0; i < 5; i++)
            {
                if (do_circle[i] == 1)
                    g.DrawEllipse(p, x + Convert.ToInt32(3.35 * widthTo16) * i, y, Convert.ToInt32(widthTo16 * 2), Convert.ToInt32(heightTo9 * 2));
            }
        }

        public void rectangle(Graphics g, SolidBrush b, int x, int y)
        {
            Point[] points = {
                new Point (Convert.ToInt32(x), Convert.ToInt32(y - Convert.ToInt32(1 * heightTo9))),
                new Point (Convert.ToInt32(x - Convert.ToInt32(1 * widthTo16)), Convert.ToInt32(y)),
                new Point (Convert.ToInt32(x), Convert.ToInt32(y + Convert.ToInt32(1 * heightTo9))),
                new Point (Convert.ToInt32(x + Convert.ToInt32(1 * widthTo16)), Convert.ToInt32(y))
            };
            g.FillPolygon(b, points);
        }

        public void One_Dot(Graphics g, SolidBrush b, int x, int y)
        {
            g.FillEllipse(b, Convert.ToInt32(x - 0.125 * widthTo16), Convert.ToInt32(y - 0.125 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
            
        }

        public void Two_Dots(Graphics g, SolidBrush b, int x, int y)
        {
            g.FillEllipse(b, Convert.ToInt32(x - 0.125 * widthTo16), Convert.ToInt32(y + 0.375 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
            g.FillEllipse(b, Convert.ToInt32(x - 0.125 * widthTo16), Convert.ToInt32(y - 0.625 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
        }

        public void Three_Dots(Graphics g, SolidBrush b, int x, int y)
        {
            One_Dot(g, b, Convert.ToInt32(x), Convert.ToInt32(y));
            Two_Dots(g, b, Convert.ToInt32(x), Convert.ToInt32(y));
        }

        public void Four_Dots(Graphics g, SolidBrush b, int x, int y)
        {
            Two_Dots(g, b, Convert.ToInt32(x), Convert.ToInt32(y));
            g.FillEllipse(b, Convert.ToInt32(x - 0.625 * widthTo16), Convert.ToInt32(y - 0.125 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
            g.FillEllipse(b, Convert.ToInt32(x + 0.375 * widthTo16), Convert.ToInt32(y - 0.125 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
        }

        public void Five_Dots(Graphics g, SolidBrush b, int x, int y)
        {
            One_Dot(g, b, Convert.ToInt32(x), Convert.ToInt32(y));
            Four_Dots(g, b, Convert.ToInt32(x), Convert.ToInt32(y));
        }

        public void Six_Dots(Graphics g, SolidBrush b, int x, int y)
        {
            Four_Dots(g, b, Convert.ToInt32(x), Convert.ToInt32(y));
            g.FillEllipse(b, Convert.ToInt32(x - 0.375 * widthTo16), Convert.ToInt32(y + 0.125 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
            g.FillEllipse(b, Convert.ToInt32(x + 0.125 * widthTo16), Convert.ToInt32(y - 0.375 * heightTo9), Convert.ToInt32(0.25 * widthTo16), Convert.ToInt32(0.25 * heightTo9));
        }

        public void Dices(Graphics g, SolidBrush d, SolidBrush r, int[] dots, int x, int y)
        {
            for (int i = 0; i < 5; i++)
            {
                rectangle(g, r, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                switch (dots[i])
                {
                    case 1:
                        One_Dot(g, d, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                        break;
                    case 2:
                        Two_Dots(g, d, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                        break;
                    case 3:
                        Three_Dots(g, d, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                        break;
                    case 4:
                        Four_Dots(g, d, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                        break;
                    case 5:
                        Five_Dots(g, d, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                        break;
                    case 6:
                        Six_Dots(g, d, Convert.ToInt32(x + 3.35 * i * widthTo16), Convert.ToInt32(y));
                        break;
                }
            }
        }

        #endregion

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Font f = new Font("Arial", Convert.ToInt32(min(0.25 * widthTo16, 0.25 * heightTo9)));
            Font G = new Font("Arial", Convert.ToInt32(min(0.375 * widthTo16, 0.375 * heightTo9)));
            switch (scene)
            {
                case Scenes.Game:
                    int x1 = Convert.ToInt32(1.25 * widthTo16);
                    int y1 = ClientSize.Height - Convert.ToInt32(1.875 * heightTo9);
                    if (timer1.Enabled == false)
                    {
                        e.Graphics.DrawString(Game_GoOn.str, f, Game_GoOn_brush, ClientSize.Width / 2 - Game_GoOn.str.Length * Convert.ToInt32(0.25 * widthTo16 * 0.35), ClientSize.Height - Convert.ToInt32(heightTo9 * 0.625));
                    }
                    Graphics g = e.Graphics;
                    SolidBrush R = new SolidBrush(Color.Red);
                    SolidBrush W = new SolidBrush(Color.White);
                    Pen pen = new Pen(Color.Black, 3);
                    Dices(g, R, W, Player1.digits, Convert.ToInt32(x1), Convert.ToInt32(y1));
                    Dices(g, R, W, Player2.digits, Convert.ToInt32(x1), Convert.ToInt32(1.25 * heightTo9));
                    g.DrawString(Player1.gold.ToString(), f, Game_Stakes_brush, Convert.ToInt32(widthTo16 * 0.875), ClientSize.Height / 2 + Convert.ToInt32(0.625 * heightTo9));
                    g.DrawString(stake.ToString(), G, Game_Stakes_brush, Convert.ToInt32(widthTo16 * 0.75), ClientSize.Height / 2 - Convert.ToInt32(0.375 * heightTo9));
                    g.DrawString(Player2.gold.ToString(), f, Game_Stakes_brush, Convert.ToInt32(widthTo16 * 0.875), ClientSize.Height / 2 - Convert.ToInt32(1.25 * heightTo9));
                    switch (stage)
                    {
                        case Stages.Draw:
                            Game_GoOn.str = "Бросить кубики";
                            Game_GoOn.size_changed(ClientSize.Width / 2 - Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Width / 2 + Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Height - Convert.ToInt32(0.625 * heightTo9), ClientSize.Height - Convert.ToInt32(0.25 * heightTo9));
                            break;
                        case Stages.Opponent_draw:
                            Game_GoOn.str = "Бросок оппонента";
                            Game_GoOn.size_changed(ClientSize.Width / 2 - Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Width / 2 + Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Height - Convert.ToInt32(0.625 * heightTo9), ClientSize.Height - Convert.ToInt32(0.25 * heightTo9));
                            break;
                        case Stages.Stakes:
                            Game_GoOn.str = "Делайте ставки";
                            Game_GoOn.size_changed(ClientSize.Width / 2 - Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Width / 2 + Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Height - Convert.ToInt32(0.625 * heightTo9), ClientSize.Height - Convert.ToInt32(0.25 * heightTo9));
                            if (timer1.Enabled == false)
                            {
                                Font s = new Font("Arial", Convert.ToInt32(min(0.375 * widthTo16, 0.375 * heightTo9)));
                                g.DrawEllipse(circle_pen, ClientSize.Width / 4 - Convert.ToInt32(0.75 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.75 * heightTo9), Convert.ToInt32(1.5 * widthTo16), Convert.ToInt32(1.5 * heightTo9));
                                g.DrawEllipse(circle_pen, ClientSize.Width / 2 - Convert.ToInt32(0.75 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.75 * heightTo9), Convert.ToInt32(1.5 * widthTo16), Convert.ToInt32(1.5 * heightTo9));
                                g.DrawEllipse(circle_pen, ClientSize.Width / 4 * 3 - Convert.ToInt32(0.75 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.75 * heightTo9), Convert.ToInt32(1.5 * widthTo16), Convert.ToInt32(1.5 * heightTo9));
                                g.DrawLine(circle_pen, ClientSize.Width / 4 - Convert.ToInt32(0.5 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.5 * heightTo9), ClientSize.Width / 4 + Convert.ToInt32(0.5 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.5*heightTo9));
                                g.DrawLine(circle_pen, ClientSize.Width / 4 - Convert.ToInt32(0.5 * widthTo16), ClientSize.Height / 2 + Convert.ToInt32(0.5 * heightTo9), ClientSize.Width / 4 + Convert.ToInt32(0.5 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.5 * heightTo9));
                                g.DrawString("+10", s, Game_Stakes_brush, ClientSize.Width / 2 - Convert.ToInt32(0.5 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.25 * heightTo9));
                                g.DrawString("+20", s, Game_Stakes_brush, ClientSize.Width / 4 * 3 - Convert.ToInt32(0.5 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(0.25 * heightTo9));
                            }
                            break;
                        case Stages.Redraw:
                            Game_GoOn.str = "Перебросьте неудачные кубики";
                            Game_GoOn.size_changed(ClientSize.Width / 2 - Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Width / 2 + Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Height - Convert.ToInt32(0.625 * heightTo9), ClientSize.Height - Convert.ToInt32(0.25 * heightTo9));
                            if (timer1.Enabled == false)
                            {
                                Circles(g, circle_pen, x1 - Convert.ToInt32(1 * widthTo16), y1 - Convert.ToInt32(1 * heightTo9), Player1.do_circle);
                            }
                            break;
                        case Stages.Opponent_redraw:
                            Game_GoOn.str = "Бросок оппонента";
                            Game_GoOn.size_changed(ClientSize.Width / 2 - Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Width / 2 + Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Height - Convert.ToInt32(0.625 * heightTo9), ClientSize.Height - Convert.ToInt32(0.25 * heightTo9));
                            if (timer1.Enabled == false)
                            {
                                Circles(g, circle_pen, x1 - Convert.ToInt32(1 * widthTo16), Convert.ToInt32(0.25 * heightTo9), Player2.do_circle);
                            }
                            break;
                        case Stages.End:
                            break;
                    }
                    break;
                case Scenes.Menu:
                    e.Graphics.DrawString("Начать игру", f, Menu_Start_brush, ClientSize.Width / 2 - Convert.ToInt32(widthTo16 * 0.875), ClientSize.Height / 2 - Convert.ToInt32(0.625 * heightTo9));
                    e.Graphics.DrawString("Рекорды", f, Menu_Record_brush, ClientSize.Width / 2 - Convert.ToInt32(0.6875 * widthTo16), ClientSize.Height / 2);
                    e.Graphics.DrawString("Помощь", f, Menu_help_brush, ClientSize.Width / 2 - Convert.ToInt32(widthTo16 * 0.625), ClientSize.Height / 2 + Convert.ToInt32(heightTo9 * 0.625));
                    e.Graphics.DrawString("Выйти", f, Menu_Quit_brush, ClientSize.Width / 2 - Convert.ToInt32(widthTo16 * 0.5), ClientSize.Height / 2 + Convert.ToInt32(heightTo9 * 1.25));
                    timer1.Enabled = false;
                    break;
                case Scenes.Records:
                    timer1.Enabled = false;
                    e.Graphics.DrawString("Вернуться", f, Records_Return_brush, ClientSize.Width / 2 - Convert.ToInt32(widthTo16 * 0.875), ClientSize.Height - Convert.ToInt32(heightTo9 * 1.25));
                    int c = 1;
                    for (int i = 0; i < 10; i++)
                    {
                        if (c == 1)
                        {
                            e.Graphics.DrawString(scores[i], f, Brushes.White, Convert.ToInt32(2.5 * widthTo16), Convert.ToInt32((1.25 + 0.625 * i) * heightTo9));
                        }
                        else
                        {
                            e.Graphics.DrawString(scores[i], f, Brushes.White, ClientSize.Width - Convert.ToInt32(2.5 * widthTo16), Convert.ToInt32((1.25 + 0.625 * (i - 1)) * heightTo9));
                        }
                        c = c * (-1);
                    }
                    break;
                case Scenes.Enter_Name:
                    if (currentplayer == Player1)
                    {
                        string s = "Вы выиграли за " + turn.ToString() + " раздач!";
                        e.Graphics.DrawString(s, G, Game_Stakes_brush, ClientSize.Width / 2 - (Convert.ToInt32(s.Length * 0.125 * widthTo16)), Convert.ToInt32(1.875 * heightTo9));
                        Keyboard(e.Graphics, f);
                        e.Graphics.DrawString("Вернуться", f, Records_Return_brush, ClientSize.Width / 2 - Convert.ToInt32(0.875 * widthTo16), ClientSize.Height - Convert.ToInt32(1.25 * heightTo9));
                        e.Graphics.FillRectangle(Brushes.White, ClientSize.Width / 2 - Convert.ToInt32(3.0625 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(1.5 * heightTo9), Convert.ToInt32(6.125 * widthTo16), Convert.ToInt32(0.5 * heightTo9));
                        e.Graphics.DrawString(name.ToString(), f, Brushes.Black, ClientSize.Width / 2 - Convert.ToInt32(name.Length * 0.0875 * widthTo16), ClientSize.Height / 2 - Convert.ToInt32(1.4375 * heightTo9));
                    }
                    if (currentplayer == Player2)
                    {
                        string s = "Вы проиграли(";
                        e.Graphics.DrawString(s, G, Game_Stakes_brush, ClientSize.Width / 2 - (Convert.ToInt32(s.Length * 0.125 * widthTo16)), Convert.ToInt32(2.5 * heightTo9));
                        e.Graphics.DrawString("Вернуться", f, Records_Return_brush, ClientSize.Width / 2 - Convert.ToInt32(0.875 * widthTo16), ClientSize.Height - Convert.ToInt32(1.125 * heightTo9));
                    }
                    break;
                case Scenes.Help:
                    e.Graphics.DrawString("Вернуться", f, Records_Return_brush, ClientSize.Width / 2 - Convert.ToInt32(0.875 * widthTo16), ClientSize.Height - Convert.ToInt32(1.25 * heightTo9));
                    Font h = new Font("Arial", Convert.ToInt32(min(0.1875 * widthTo16, 0.1875 * heightTo9)));
                    for (int i = 0; i < 13; i++)
                    {
                        e.Graphics.DrawString(help[i], h, Brushes.Black, ClientSize.Width / 2 - Convert.ToInt32(help[i].Length * 0.0625 * widthTo16), Convert.ToInt32(0.25 * heightTo9 + 0.5 * heightTo9 * i));
                    }
                    break;
            }



        }

        private void switch_player()
        {
            if (currentplayer == Player1)
            {
                currentplayer = Player2;
            }
            else { currentplayer = Player1; }
        }

        public void Redraw()
        {

            time = 0;
            timer1.Enabled = true;
        }

        private void Warp(Player player)
        {
            player.combo = Combinations.none;
            for (int i = 0; i < 5; i++)
            {
                player.parameter[i] = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time++;
            for (int i = 0; i < 5; i++)
            {
                if (currentplayer.do_circle[i] == 1)
                    currentplayer.digits[i] = rand.Next(1, 7);
            }
            if (time == 30)
            {

                currentplayer.land_a_combo();
                if (stage == Stages.End)
                {
                    the_winner_is();
                }
                if (stage == Stages.Stakes)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        Player1.do_circle[i] = Player1.do_circle[i] * (-1);
                        Player2.do_circle[i] = Player2.do_circle[i] * (-1);
                    }
                }
                Invalidate();
                timer1.Enabled = false;
            }
            Invalidate();

        }

        public void another_round()
        {
            currentplayer = Player1;
            Game_GoOn.str = "Бросить кубики";
            stage = Stages.Draw;
            Warp(Player1);
            Warp(Player2);
            for (int i = 0; i < 5; i++)
            {
                Player1.do_circle[i] = 1;
                Player2.do_circle[i] = 1;
            }
            if (Player1.gold == 0)
            {
                currentplayer = Player2;
                scene = Scenes.Enter_Name;
            }
            if (Player2.gold == 0)
            {
                currentplayer = Player1;
                scene = Scenes.Enter_Name;
            }
            turn++;
            Invalidate();
        }

        public void the_winner_is()
        {
            if (Player1.combo > Player2.combo)
            {
                Player1.gold += stake;
                stake = 0;
                Game_GoOn.str = "Вы выиграли в раздаче";
            }
            else if (Player2.combo > Player1.combo)
            {
                Player2.gold += stake;
                stake = 0;
                Game_GoOn.str = "Вы проиграли в раздаче";
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    if (Player1.parameter[i] > Player2.parameter[i])
                    {
                        Player1.gold += stake;
                        stake = 0;
                        Game_GoOn.str = "Вы выиграли в раздаче";
                        break;
                    }
                    else if (Player2.parameter[i] > Player1.parameter[i])
                    {
                        Player2.gold += stake;
                        stake = 0;
                        Game_GoOn.str = "Вы проиграли в раздаче";
                        break;
                    }
                }
            }
            if (stake != 0)
            {
                Player1.gold += stake / 2;
                Player2.gold += stake / 2;
                stake = 0;
                Game_GoOn.str = "Ничья";
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClientSize = new Size(1280, 720);
            int X = ClientSize.Width;
            int Y = ClientSize.Height;
            scores = File.ReadAllLines("scores.txt");
            board.input(scores);
            currentplayer = Player1;
            Game_GoOn.str = "Бросить кубики";
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int X = ClientSize.Width;
            int Y = ClientSize.Height;
            switch (scene)
            {
                case Scenes.Menu:
                    if (Menu_Start.area(e.X, e.Y) && !timer1.Enabled)
                    { scene = Scenes.Game; Invalidate(); }
                    if (Menu_Record.area(e.X, e.Y))
                    { scene = Scenes.Records; Invalidate(); }
                    if (Menu_Help.area(e.X, e.Y))
                    { scene = Scenes.Help; Invalidate(); }
                    if (Menu_Close.area(e.X, e.Y))
                    { Close(); }
                    break;
                case Scenes.Game:
                    switch (stage)
                    {
                        case Stages.Draw:
                            if (Game_GoOn.area(e.X, e.Y) && !timer1.Enabled)
                            {
                                Player1.gold -= 10;
                                stake += 10;
                                stage = Stages.Opponent_draw;
                                Redraw();
                            }
                            break;
                        case Stages.Opponent_draw:
                            if (Game_GoOn.area(e.X, e.Y) && !timer1.Enabled)
                            {
                                switch_player();
                                Player2.gold -= 10;
                                stake += 10;
                                stage = Stages.Stakes;
                                Redraw();
                            }
                            break;
                        case Stages.Stakes:
                            for (int i = 0; i < 3; i++)
                            {
                                if (Game_Stakes.area(e.X - (X / 4) * i, e.Y) && !timer1.Enabled)
                                {
                                    if (Player1.gold >= i * 10 && Player2.gold >= i * 10)
                                    {
                                        Player1.gold -= i * 10;
                                        stake += i * 10;
                                        if (i > 0)
                                            if (AI.money_management(Player2, Player1, i))
                                            {
                                                Player2.gold -= i * 10; stake += i * 10;
                                            }
                                            else
                                            {
                                                Player1.gold += stake;
                                                stake = 0;
                                                another_round();
                                                break;
                                            }
                                        Game_GoOn.str = "Перебросьте неудачные кости";
                                        Invalidate();
                                        stage = Stages.Redraw;
                                    }
                                }
                            }
                            break;
                        case Stages.Redraw:
                            for (int i = 0; i < 5; i++)
                            {
                                if (Game_Redraw_Circles.area(e.X - Convert.ToInt32(3 * i * widthTo16), e.Y) && !timer1.Enabled)
                                { Player1.do_circle[i] = Player1.do_circle[i] * (-1); Invalidate(); }
                            }
                            if (Game_GoOn.area(e.X, e.Y) && !timer1.Enabled)
                            {
                                switch_player();
                                AI.behavior(Player2, Player1);
                                stage = Stages.Opponent_redraw;
                                Warp(Player1);
                                Redraw();
                            }
                            break;

                        case Stages.Opponent_redraw:
                            if (Game_GoOn.area(e.X, e.Y) && !timer1.Enabled)
                            {
                                switch_player();
                                stage = Stages.End;
                                Warp(Player2);
                                Redraw();
                            }
                            break;
                        case Stages.End:
                            if (Game_GoOn.area(e.X, e.Y) && !timer1.Enabled)
                            {
                                another_round();
                            }
                            break;
                    }


                    break;
                case Scenes.Records:
                    if (Records_return.area(e.X, e.Y))
                    { scene = Scenes.Menu; Invalidate(); }
                    break;
                case Scenes.Enter_Name:
                    if (name.Length < 20)
                    {
                        for (int i = 0; i < 26; i++)
                        {
                            if (i < 10)
                            {
                                if (Enter_name_Key.area(e.X - Convert.ToInt32(0.625 * widthTo16) * i, e.Y))
                                {
                                    if (shift)
                                    {
                                        name.Append(alphabet[i]);
                                    }
                                    else
                                    {
                                        name.Append(alphabet[i].ToLower());
                                    }
                                    break;
                                }
                            }
                            else
                            if (i > 9 && i < 20)
                            {
                                if (Enter_name_Key.area(e.X - Convert.ToInt32(0.625 * widthTo16) * (i - 10), e.Y - Convert.ToInt32(0.625 * heightTo9)))
                                {
                                    if (shift)
                                    {
                                        name.Append(alphabet[i]);
                                    }
                                    else
                                    {
                                        name.Append(alphabet[i].ToLower());
                                    }
                                    break;
                                }
                            }
                            if (i > 19)
                            {
                                if (Enter_name_Key.area(e.X - Convert.ToInt32(0.625 * widthTo16) * (i - 20), e.Y - Convert.ToInt32(1.25 * heightTo9)))
                                {
                                    if (shift)
                                    {
                                        name.Append(alphabet[i]);
                                    }
                                    else
                                    {
                                        name.Append(alphabet[i].ToLower());
                                    }
                                    break;
                                }
                            }
                        }
                        if (Enter_name_Space.area(e.X, e.Y))
                        {
                            name.Append('_');
                        }
                    }

                    if (Enter_name_Bspace.area(e.X, e.Y))
                    {
                        if (name.Length > 0)
                            name.Remove(name.Length - 1, 1);
                    }

                    if (Enter_name_Bspace.area(e.X - Convert.ToInt32(1.25 * widthTo16), e.Y))
                    {
                        if (shift)
                        {
                            shift = false;
                        }
                        else
                        {
                            shift = true;
                        }
                    }
                    if (Records_return.area(e.X, e.Y))
                    {
                        if (currentplayer == Player1)
                        {
                            board.move(name.ToString(), turn);
                            for (int i = 0; i < 10; i++)
                            {
                                if (i % 2 == 0)
                                {
                                    scores[i] = board.names[i / 2];
                                }
                                else
                                {
                                    scores[i] = board.scores[i / 2].ToString();
                                }
                            }
                            File.Delete("scores.txt");
                            File.WriteAllLines("scores.txt", scores);
                        }
                        scene = Scenes.Menu;
                        Invalidate();
                        Restart();
                    }
                    Invalidate();
                    break;
                case Scenes.Help:
                    if (Records_return.area(e.X, e.Y))
                    { scene = Scenes.Menu; Invalidate(); }
                    break;
            }

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (scene)
            {
                case Scenes.Menu:

                    if (Menu_Start.area(e.X, e.Y))
                    { if (Menu_Start_brush.Color == Color.Snow) { Menu_Start_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Menu_Start_brush.Color == Color.Orange) { Menu_Start_brush.Color = Color.Snow; Invalidate(); }

                    if (Menu_Record.area(e.X, e.Y))
                    { if (Menu_Record_brush.Color == Color.Snow) { Menu_Record_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Menu_Record_brush.Color == Color.Orange) { Menu_Record_brush.Color = Color.Snow; Invalidate(); }

                    if (Menu_Close.area(e.X, e.Y))
                    { if (Menu_Quit_brush.Color == Color.Snow) { Menu_Quit_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Menu_Quit_brush.Color == Color.Orange) { Menu_Quit_brush.Color = Color.Snow; Invalidate(); }

                    if (Menu_Help.area(e.X, e.Y))
                    { if (Menu_help_brush.Color == Color.Snow) { Menu_help_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Menu_help_brush.Color == Color.Orange) { Menu_help_brush.Color = Color.Snow; Invalidate(); }
                    break;

                case Scenes.Records:
                    if (Records_return.area(e.X, e.Y))
                    { if (Records_Return_brush.Color == Color.Snow) { Records_Return_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Records_Return_brush.Color == Color.Orange) { Records_Return_brush.Color = Color.Snow; Invalidate(); }
                    break;

                case Scenes.Game:
                    if (Game_GoOn.area(e.X, e.Y) && stage != Stages.Stakes)
                    { if (Game_GoOn_brush.Color == Color.Snow) { Game_GoOn_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Game_GoOn_brush.Color == Color.Orange) { Game_GoOn_brush.Color = Color.Snow; Invalidate(); }
                    break;
                case Scenes.Enter_Name:
                    if (Records_return.area(e.X, e.Y))
                    { if (Records_Return_brush.Color == Color.Snow) { Records_Return_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Records_Return_brush.Color == Color.Orange) { Records_Return_brush.Color = Color.Snow; Invalidate(); }
                    break;
                case Scenes.Help:
                    if (Records_return.area(e.X, e.Y))
                    { if (Records_Return_brush.Color == Color.Snow) { Records_Return_brush.Color = Color.Orange; Invalidate(); } }
                    else if (Records_Return_brush.Color == Color.Orange) { Records_Return_brush.Color = Color.Snow; Invalidate(); }
                    break;
            }
        }

        private void Form1_ClientSizeChanged(object sender, EventArgs e)
        {
            int X = ClientSize.Width;
            int Y = ClientSize.Height;
            Menu_Start.size_changed(X / 2 - Convert.ToInt32(0.875 * widthTo16), X / 2 + Convert.ToInt32(1.125 * widthTo16), Y / 2 - Convert.ToInt32(0.625 * heightTo9), Y / 2 - Convert.ToInt32(0.25 * heightTo9));
            Menu_Record.size_changed(X / 2 - Convert.ToInt32(0.875 * widthTo16), X / 2 + Convert.ToInt32(1.125 * widthTo16), Y / 2, Y / 2 + Convert.ToInt32(0.375 * heightTo9));
            Menu_Close.size_changed(X / 2 - Convert.ToInt32(0.875 * widthTo16), X / 2 + Convert.ToInt32(1.125 * widthTo16), Y / 2 + Convert.ToInt32(1.25 * heightTo9), Y / 2 + Convert.ToInt32(1.625 * heightTo9));
            Game_Redraw_Circles.size_changed(Convert.ToInt32(0.5 * widthTo16), Convert.ToInt32(2.5 * widthTo16), Y - Convert.ToInt32(2.5 * heightTo9), Y - Convert.ToInt32(0.5 * heightTo9));
            Records_return.size_changed(X / 2 - Convert.ToInt32(0.875 * widthTo16), X / 2 + Convert.ToInt32(1 * widthTo16), Y - Convert.ToInt32(1.25 * heightTo9), Y - Convert.ToInt32(0.875 * heightTo9));
            Game_Stakes.size_changed(X / 4 - Convert.ToInt32(0.75 * widthTo16), X / 4 + Convert.ToInt32(0.75 * widthTo16), Y / 2 - Convert.ToInt32(0.75 * heightTo9), Y / 2 + Convert.ToInt32(0.75 * heightTo9));
            Enter_name_Key.size_changed(X / 2 - Convert.ToInt32(3.0625 * widthTo16), X / 2 - Convert.ToInt32(2.5625 * widthTo16), Y / 2 - Convert.ToInt32(0.875 * heightTo9), Y / 2 - Convert.ToInt32(0.375 * heightTo9));
            Enter_name_Bspace.size_changed(X / 2 + Convert.ToInt32(0.6875 * widthTo16), X / 2 + Convert.ToInt32(1.8125 * widthTo16), Y / 2 + Convert.ToInt32(0.375 * heightTo9), Y / 2 + Convert.ToInt32(0.875 * heightTo9));
            Enter_name_Space.size_changed(X / 2 - Convert.ToInt32(3.0625 * widthTo16), X / 2 + Convert.ToInt32(3.0625 * widthTo16), Y / 2 + Convert.ToInt32(1 * heightTo9), Y / 2 + Convert.ToInt32(1.5 * heightTo9));
            Menu_Help.size_changed(X / 2 - Convert.ToInt32(0.875 * widthTo16), X / 2 + Convert.ToInt32(1.125 * widthTo16), Y / 2 + Convert.ToInt32(0.625 * heightTo9), Y / 2 + Convert.ToInt32(1 * heightTo9));
            if (Game_GoOn.str != null)
            Game_GoOn.size_changed(ClientSize.Width / 2 - Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Width / 2 + Convert.ToInt32(Game_GoOn.str.Length * 0.0875 * widthTo16), ClientSize.Height - Convert.ToInt32(0.625 * heightTo9), ClientSize.Height - Convert.ToInt32(0.25 * heightTo9));
            widthTo16 = ClientSize.Width / 16;
            heightTo9 = ClientSize.Height / 9;
            Invalidate();
        }

        public double max(double one, double two)
        {
            if (one > two)
                return one;            
            else
                return two;
        }

        public double min (double one, double two)
        {
            if (one < two)
                return one;
            else
                return two;
        }

        private void Restart()
        {
            Warp(Player1);
            Warp(Player2);
            for (int i = 0; i < 5; i++)
            {
                Player1.do_circle[i] = 1;
                Player2.do_circle[i] = 1;
                Player1.digits[i] = i + 1;
                Player2.digits[i] = i + 1;
            }
            Player1.gold = 200;
            Player2.gold = 200;
            turn = 1;
            currentplayer = Player1;
            stage = Stages.Draw;
            name.Clear();
        }


    }

}
