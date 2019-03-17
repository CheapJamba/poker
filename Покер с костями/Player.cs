using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Покер_с_костями
{
    public enum Combinations { none, pair, two_pairs, set, full_house, street, grand_street, quads, perfect };

    class Player
    {

        public Combinations combo;
        public int[] parameter = { 0, 0, 0, 0, 0 };
        public int[] digits = { 6, 2, 3, 4, 5 };
        public int[] do_circle = { 1, 1, 1, 1, 1 };
        public int gold = 200;

        public Player() { }

        public Player(Combinations combo, int parameter_1, int parameter_2, int parameter_3, int parameter_4, int parameter_5, int digit_1, int digit_2, int digit_3, int digit_4, int digit_5)
        {
            this.combo = combo;
            this.parameter[0] = parameter_1;
            this.parameter[1] = parameter_2;
            this.parameter[2] = parameter_3;
            this.parameter[3] = parameter_4;
            this.parameter[4] = parameter_5;
            digits[0] = digit_1;
            digits[1] = digit_2;
            digits[2] = digit_3;
            digits[3] = digit_4;
            digits[4] = digit_5;
        }

        public void land_a_combo()
        {
            for (int numberWereLookingFor = 6; numberWereLookingFor > 0; numberWereLookingFor--)
            {
                int dicesWithThatNumber = 0;
                for (int diceBeingChecked = 0; diceBeingChecked < 5; diceBeingChecked++)
                {
                    if (digits[diceBeingChecked] == numberWereLookingFor)
                    {
                        dicesWithThatNumber++;
                    }

                }
                switch (dicesWithThatNumber)
                {
                    case 2:
                        if (combo == Combinations.none)
                        {
                            combo = Combinations.pair;
                            parameter[0] = numberWereLookingFor;
                        }
                        else if (combo == Combinations.pair)
                        {
                            combo = Combinations.two_pairs;
                            if (numberWereLookingFor > parameter[0])
                            {
                                int lessImportantParameter = parameter[0];
                                parameter[0] = numberWereLookingFor;
                                parameter[1] = lessImportantParameter;
                            }
                            else parameter[1] = numberWereLookingFor;
                        }
                        else if (combo == Combinations.set)
                        {
                            combo = Combinations.full_house;
                            parameter[1] = numberWereLookingFor;
                        }
                        break;
                    case 3:
                        if (combo == Combinations.none)
                        {
                            combo = Combinations.set;
                            parameter[0] = numberWereLookingFor;
                        }
                        else if (combo == Combinations.pair)
                        {
                            combo = Combinations.full_house;
                            parameter[1] = parameter[0];
                            parameter[0] = numberWereLookingFor;
                        }
                        break;
                    case 4:
                        if (combo == Combinations.none)
                        {
                            combo = Combinations.quads;
                            parameter[0] = numberWereLookingFor;
                        }
                        break;
                    case 5:
                        if (combo == Combinations.none)
                        {
                            combo = Combinations.perfect;
                            parameter[0] = numberWereLookingFor;
                        }
                        break;
                }
            }

            if (combo == Combinations.none)
            {
                int sum = 0;
                for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                {
                    sum += digits[diceWereChecking];
                }
            }

            if (combo == Combinations.pair)
            {
                int parameterPlace = 1; //The number of paired dices should always be at first place and then other dices numbers should from highest to lowest
                for (int numberWereLoockingFor = 6; numberWereLoockingFor > 0; numberWereLoockingFor--)
                {
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (digits[diceWereChecking] == parameter[0])
                            continue;
                        if (digits[diceWereChecking] == numberWereLoockingFor)
                        { parameter[parameterPlace] = numberWereLoockingFor; parameterPlace++; }
                    }
                }
            }

            if (combo == Combinations.set)
            {
                int parameterPlace = 1; //The same goes here, that goes to pairs
                for (int numberWereLookingFor = 6; numberWereLookingFor > 0; numberWereLookingFor--)
                {
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (digits[diceWereChecking] == parameter[0])
                            continue;
                        if (digits[diceWereChecking] == numberWereLookingFor)
                        { parameter[parameterPlace] = numberWereLookingFor; parameterPlace++; }
                    }
                }
            }

            if (combo == Combinations.two_pairs)
            {
                for (int numberWereLookingFor = 6; numberWereLookingFor > 0; numberWereLookingFor--) //And here too, as you might have guessed
                {
                    for (int diceWereChecking = 0; diceWereChecking < 5; diceWereChecking++)
                    {
                        if (digits[diceWereChecking] == parameter[0] || digits[diceWereChecking] == parameter[1])
                            continue;
                        if (digits[diceWereChecking] == numberWereLookingFor)
                        { parameter[2] = numberWereLookingFor; }
                    }
                }
            }
        }
    }
}
