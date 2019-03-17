using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Покер_с_костями
{
    class MyButton
    {
        public int left_edge;
        public int right_edge;
        public int bottom_edge;
        public int top_edge;
        public string str;
        public MyButton()
        { }

        public MyButton(int left, int right, int top, int bot)
        {
            this.left_edge = left;
            this.right_edge = right;
            this.top_edge = top;
            this.bottom_edge = bot;
        }

        public void size_changed(int left, int right, int top, int bot)
        {
            this.left_edge = left;
            this.right_edge = right;
            this.top_edge = top;
            this.bottom_edge = bot;
        }

        public bool area(int X, int Y)
        {
            if (X > left_edge && X < right_edge && Y > top_edge && Y < bottom_edge)
                return true;
            else return false;
        }
    }

    class LeaderBoard
    {
        public string[] names = { "", "", "", "", "" };
        public int[] scores = { 0, 0, 0, 0, 0 };

        public LeaderBoard()
        { }

        public void input(string[] arr)
        {
            for (int i = 0; i < 5; i++)
            {
                names[i] = arr[i * 2];
            }
            for (int i = 0; i < 5; i++)
            {
                scores[i] = Convert.ToInt16(arr[i * 2 + 1]);
            }
        }

        public void move(string somename, int somescore)
        {
            for (int i = 0; i < 5; i++)
            {
                if (scores[i] > somescore)
                {
                    for (int j = 4; j > i; j--)
                    {
                        scores[j] = scores[j - 1];
                        names[j] = names[j - 1];

                    }
                    scores[i] = somescore;
                    names[i] = somename;
                    break;
                }

            }
        }
    }
}
