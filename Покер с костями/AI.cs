using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Покер_с_костями
{
    class AI
    {
        public static void behavior(Player me, Player opponent)
        {
            switch (me.combo)
            {
                case Combinations.none:
                    int theBiggestNumber = 0;
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (me.digits[diceWereChecking] > theBiggestNumber)
                            theBiggestNumber = me.digits[diceWereChecking];
                    }
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (me.digits[diceWereChecking] != theBiggestNumber)
                        {
                            me.do_circle[diceWereChecking] = 1;
                        }
                    }
                    break;
                case Combinations.pair:
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (me.digits[diceWereChecking] != me.parameter[0] && me.digits[diceWereChecking] != me.parameter[1])
                            me.do_circle[diceWereChecking] = 1;
                    }
                    break;
                case Combinations.two_pairs:
                    if (opponent.combo > Combinations.full_house)
                    {
                        for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                        {
                            if (me.digits[diceWereChecking] != me.parameter[0])
                                me.do_circle[diceWereChecking] = 1;
                        }
                    }
                    else
                        for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                        {
                            if (me.digits[diceWereChecking] != me.parameter[0] && me.digits[diceWereChecking] != me.parameter[1])
                                me.do_circle[diceWereChecking] = 1;
                        }
                    break;
                case Combinations.set:
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (me.digits[diceWereChecking] != me.parameter[0] && me.digits[diceWereChecking] != me.parameter[1])
                            me.do_circle[diceWereChecking] = 1;
                    }
                    break;
                case Combinations.full_house:
                    if (opponent.combo > me.combo)
                    {
                        for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                        {
                            if (me.digits[diceWereChecking] != me.parameter[0])
                                me.do_circle[diceWereChecking] = 1;
                        }
                    }
                    break;
                case Combinations.street:
                    break;
                case Combinations.grand_street:
                    break;
                case Combinations.quads:
                    if (opponent.combo > me.combo && opponent.parameter[0] > me.parameter[0])
                        for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                        {
                            me.do_circle[diceWereChecking] = 1;
                        }
                    else
                    {
                        for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                        {
                            if (me.digits[diceWereChecking] != me.parameter[0])
                                me.do_circle[diceWereChecking] = 1;
                        }
                    }
                    break;
                case Combinations.perfect:
                    if (opponent.combo == me.combo && opponent.parameter[0] > me.parameter[0])
                    {
                        for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                        {
                            me.do_circle[diceWereChecking] = 1;
                        }
                    }
                    break;
            }
        }

        public static bool money_management(Player me, Player opponent, int amount)
        {
            if (me.combo > opponent.combo)
            {
                if (opponent.combo <= Combinations.two_pairs)
                {
                    return true;
                }
                else
                {
                    if (amount == 2 && opponent.parameter[0] < me.parameter[0]) { return true; }
                    else if (amount == 1) { return true; }
                    else { return false; }
                }
            }
            else if (me.parameter[0] > opponent.parameter[0] && opponent.combo < Combinations.full_house)
            { return true; }
            else { return false; }
        }
    }
}
