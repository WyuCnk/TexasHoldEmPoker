using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TexasHoldEmPoker
{
    public class Faces : IComparable<Faces>
    {
        private Dictionary<string, Suit> _suits;
        private Dictionary<Suit, int> _sum_of_suits;
        private int[] _sum_of_numbers;
        private int _max_straight_length;

        /// <summary>
        /// 扑克牌
        /// </summary>
        public List<Poker> Pokers { get; set; }

        /// <summary>
        /// 牌型
        /// </summary>
        public Texture Texture { get; set; }

        /// <summary>
        /// 在牌型相同时进行判断
        /// </summary>
        public List<int> Weight { get; set; }

        public Faces(string[] pokers)
        {
            _suits = new Dictionary<string, Suit>();
            _suits["d"] = Suit.Diamond;
            _suits["s"] = Suit.Spade;
            _suits["c"] = Suit.Club;
            _suits["h"] = Suit.Heart;
            _sum_of_suits = new Dictionary<Suit, int>();
            _sum_of_suits[Suit.Club] = 0;
            _sum_of_suits[Suit.Diamond] = 0;
            _sum_of_suits[Suit.Heart] = 0;
            _sum_of_suits[Suit.Spade] = 0;
            _sum_of_numbers = new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            int len = pokers.Length, number;
            Pokers = new List<Poker>();
            Weight = new List<int>();
            for (int i = 0; i < len; i++)
            {
                Poker poker = new Poker();
                poker.Suit = _suits[pokers[i].Substring(pokers[i].Length - 1, 1)];
                switch (pokers[i].Substring(0, 1))
                {
                    case "A":
                        number = 1;
                        break;
                    case "J":
                        number = 11;
                        break;
                    case "Q":
                        number = 12;
                        break;
                    case "K":
                        number = 13;
                        break;
                    default:
                        number = Convert.ToInt16(pokers[i].Substring(0, pokers[i].Length - 1));
                        break;
                }

                poker.number = number;
                Pokers.Add(poker);
                _sum_of_suits[poker.Suit]++;
                _sum_of_numbers[number]++;
            }

            JudgeTexture();
        }


        public int CompareTo(Faces faces)
        {
            int result = 0;
            if (Texture > faces.Texture)
            {
                result = -1;
            }
            else if (Texture < faces.Texture)
            {
                result = 1;
            }
            else
            {
                int w1, w2;
                switch (Texture)
                {
                    case Texture.Royal_flush:
                        result = 0;
                        break;
                    case Texture.Straight_flush:
                        if (Weight[0] > faces.Weight[0])
                        {
                            result = -1;
                        }
                        else if (Weight[0] < faces.Weight[0])
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                    case Texture.Flush:
                        if (Weight[0] == 1 && faces.Weight[0] != 1)
                        {
                            result = -1;
                        }
                        else if (Weight[0] != 1 && faces.Weight[0] == 1)
                        {
                            result = 1;
                        }
                        else
                        {
                            int i = Weight.Count - 1, j = faces.Weight.Count - 1;
                            bool flag = false;
                            while (i >= Weight.Count - 5 && j >= faces.Weight.Count - 5 && i > 0 && j > 0)
                            {
                                if (Weight[i] > faces.Weight[j])
                                {
                                    result = -1;
                                    flag = true;
                                    break;
                                }

                                if (Weight[i] < faces.Weight[j])
                                {
                                    result = 1;
                                    flag = true;
                                    break;
                                }

                                i--;
                                j--;
                            }

                            if (!flag)
                            {
                                result = 0;
                            }
                        }

                        break;
                    case Texture.Four_of_a_kind:
                        w1 = Weight[0] * 15 + Weight[1];
                        w2 = faces.Weight[0] * 15 + faces.Weight[1];
                        if (w1 > w2)
                        {
                            result = -1;
                        }
                        else if (w1 < w2)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                    case Texture.Full_house:
                        w1 = Weight[0] * 15 + Weight[1];
                        w2 = faces.Weight[0] * 15 + faces.Weight[1];
                        if (w1 > w2)
                        {
                            result = -1;
                        }
                        else if (w1 < w2)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                    case Texture.Straight:
                        if (Weight[0] > faces.Weight[0])
                        {
                            result = -1;
                        }
                        else if (Weight[0] < faces.Weight[0])
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                    case Texture.Three_of_a_kind:
                        if (Weight[0] > faces.Weight[0])
                        {
                            result = -1;
                        }
                        else if (Weight[0] < faces.Weight[0])
                        {
                            result = 1;
                        }

                        break;
                    case Texture.Two_pairs:
                        w1 = Weight[0] * 15 * 15 + Weight[1] * 15 + Weight[2];
                        w2 = faces.Weight[0] * 15 * 15 + faces.Weight[1] * 15 + faces.Weight[2];
                        if (w1 > w2)
                        {
                            result = -1;
                        }
                        else if (w1 < w2)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                    case Texture.One_pair:
                        w1 = Weight[0] * 15 * 15 * 15 + Weight[1] * 15 * 15 + Weight[2] * 15 + Weight[3];
                        w2 = faces.Weight[0] * 15 * 15 * 15 + faces.Weight[1] * 15 * 15 + faces.Weight[2] * 15 +
                             faces.Weight[3];
                        if (w1 > w2)
                        {
                            result = -1;
                        }
                        else if (w1 < w2)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                    default:
                        w1 = Weight[0] * 15 * 15 * 15 * 15 + Weight[1] * 15 * 15 * 15 + Weight[2] * 15 * 15 +
                             Weight[3] * 15 + Weight[4];
                        w2 = faces.Weight[0] * 15 * 15 * 15 * 15 + faces.Weight[1] * 15 * 15 * 15 +
                             faces.Weight[2] * 15 * 15 + faces.Weight[3] * 15 + Weight[4];
                        if (w1 > w2)
                        {
                            result = -1;
                        }
                        else if (w1 < w2)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }

                        break;
                }
            }

            return result;
        }

        public void print()
        {
            string result = "";
            foreach (var poker in Pokers)
            {
                switch (poker.number)
                {
                    case 1:
                        result += "A";
                        break;
                    case 11:
                        result += "J";
                        break;
                    case 12:
                        result += "Q";
                        break;
                    case 13:
                        result += "K";
                        break;
                    default:
                        result += poker.number;
                        break;
                }

                switch (poker.Suit)
                {
                    case Suit.Club:
                        result += "c";
                        break;
                    case Suit.Diamond:
                        result += "d";
                        break;
                    case Suit.Heart:
                        result += "h";
                        break;
                    case Suit.Spade:
                        result += "s";
                        break;
                }

                result += " ";
            }
            Console.Write(result);
            Console.WriteLine(Texture);
        }

        /// <summary>
        /// 判断牌型
        /// </summary>
        private void JudgeTexture()
        {
            List<int> list;
            int straight_length;
            _max_straight_length = 0;
            //判断是否是同花、同花顺、皇家同花顺
            foreach (var sumOfSuit in _sum_of_suits)
            {
                if (sumOfSuit.Value >= 5)
                {
                    //判断是否是皇家同花顺
                    list = Pokers.Where(x => x.Suit == sumOfSuit.Key).Select(x => x.number).ToList();
                    list.Sort();
                    if (list[0] == 1 && list[list.Count - 4] == 10)
                    {
                        Texture = Texture.Royal_flush;
                        return;
                    }

                    //判断是否是同花顺
                    int i = list.Count - 1;
                    while (i >= 4 && list[i] != list[i - 4] + 4)
                    {
                        i--;
                    }

                    if (i >= 4)
                    {
                        Texture = Texture.Straight_flush;
                        Weight.Add(list[i]);
                        return;
                    }

                    Texture = Texture.Flush;
                    Weight = list.ToList();
                    return;
                }
            }

            //判断是否是四条
            if (_sum_of_numbers.Contains(4))
            {
                Texture = Texture.Four_of_a_kind;
                int index = Array.IndexOf(_sum_of_numbers, 4);
                Weight.Add(index == 1 ? 14 : index);
                if (_sum_of_numbers[1] != 0 && _sum_of_numbers[1] != 4)
                {
                    Weight.Add(14);
                }
                else
                {
                    for (int i = 13; i >= 2; i--)
                    {
                        if (_sum_of_numbers[i] != 0 && _sum_of_numbers[i] != 4)
                        {
                            Weight.Add(i);
                            break;
                        }
                    }
                }

                return;
            }

            //判断是否是葫芦
            if (_sum_of_numbers.Contains(3) && _sum_of_numbers.Contains(2))
            {
                Texture = Texture.Full_house;
                int index = Array.IndexOf(_sum_of_numbers, 3);
                Weight.Add(index == 1 ? 14 : index);
                index = Array.LastIndexOf(_sum_of_numbers, 2);
                Weight.Add(index == 1 ? 14 : index);
                return;
            }

            if (_sum_of_numbers.Where(x => x == 3).ToList().Count == 2)
            {
                Texture = Texture.Full_house;
                if (_sum_of_numbers[1] == 3)
                {
                    Weight.Add(14);
                }
                else
                {
                    Weight.Add(Array.LastIndexOf(_sum_of_numbers, 3));
                }

                Weight.Add(Array.IndexOf(_sum_of_numbers, 3, 2));
                return;
            }

            //判断是否是顺子
            list = new List<int>();
            for (int i = 1; i < 14; i++)
            {
                if (_sum_of_numbers[i] != 0)
                {
                    list.Add(i);
                }
            }

            if (list[0] == 1 && list[list.Count - 4] == 10)
            {
                Texture = Texture.Straight;
                Weight.Add(14);
                return;
            }

            int j = list.Count - 1;
            while (j >= 4 && list[j] != list[j - 4] + 4)
            {
                j--;
            }

            if (j >= 4)
            {
                Texture = Texture.Straight;
                Weight.Add(list[j]);
                return;
            }

            //判断是否是三条
            if (_sum_of_numbers.Contains(3))
            {
                Texture = Texture.Three_of_a_kind;
                int index = Array.IndexOf(_sum_of_numbers, 3);
                Weight.Add(index == 1 ? 14 : index);
                if (_sum_of_numbers[1] == 1)
                {
                    Weight.Add(14);
                }

                for (int i = 13; i >= 2; i--)
                {
                    if (Weight.Count == 5)
                    {
                        break;
                    }

                    if (_sum_of_numbers[i] == 1)
                    {
                        Weight.Add(i);
                    }
                }

                return;
            }

            //判断是否是两对或者一对或者高牌
            int l = _sum_of_numbers.Where(x => x == 2).ToList().Count;
            if (l == 2)
            {
                Texture = Texture.Two_pairs;
                if (_sum_of_numbers[1] == 2)
                {
                    Weight.Add(14);
                    Weight.Add(Array.LastIndexOf(_sum_of_numbers, 2));
                    Weight.Add(Array.LastIndexOf(_sum_of_numbers, 1));
                }
                else
                {
                    for (int i = 13; i >= 2; i--)
                    {
                        if (Weight.Count == 2)
                        {
                            break;
                        }

                        if (_sum_of_numbers[i] == 2)
                        {
                            Weight.Add(i);
                        }
                    }

                    if (_sum_of_numbers[1]==1)
                    {
                        Weight.Add(14);
                    }
                    else
                    {
                        Weight.Add(Array.LastIndexOf(_sum_of_numbers, 1));
                    }
                }
            }
            else if (l == 1)
            {
                Texture = Texture.One_pair;
                int index = Array.IndexOf(_sum_of_numbers, 2);
                Weight.Add(index == 1 ? 14 : index);
                if (_sum_of_numbers[1] == 1)
                {
                    Weight.Add(14);
                }

                for (int i = 13; i >= 2; i--)
                {
                    if (Weight.Count == 4)
                    {
                        break;
                    }

                    if (_sum_of_numbers[i] == 1)
                    {
                        Weight.Add(i);
                    }
                }
            }
            else
            {
                Texture = Texture.High_card;
                if (_sum_of_numbers[1] == 1)
                {
                    Weight.Add(14);
                }

                for (int i = 13; i >= 2; i--)
                {
                    if (Weight.Count == 5)
                    {
                        break;
                    }

                    if (_sum_of_numbers[i] == 1)
                    {
                        Weight.Add(i);
                    }
                }
            }
        }
    }
}