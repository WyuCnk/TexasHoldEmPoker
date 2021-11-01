using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic;

namespace TexasHoldEmPoker
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> pokers = new List<string>();
            string[] units =  {"d", "h", "s", "c"};
            string[] numbers = {"A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K"};
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    pokers.Add(numbers[j]+units[i]);
                }
            }

            // var randomList = RandomList(9, pokers);
            // var player1 = randomList.ToList();
            // player1.RemoveAt(8);
            // player1.RemoveAt(7);
            // var player2 = randomList.ToList();
            // player2.RemoveAt(0);
            // player2.RemoveAt(0);

            // string[] player1 = {"3c", "3d", "6c", "6s", "8h", "9c", "10c"};
            // string[] player2 = {"6c", "6s", "9h", "9c", "10c", "Jc", "2s"};
            

            // Faces faces1 = new Faces(player1.ToArray()), faces2 = new Faces(player2.ToArray());
            //
            // foreach (var s in player1)
            // {
            //     Console.Write(s+" ");
            // }
            // Console.WriteLine(faces1.Texture);
            //
            // foreach (var s in player2)
            // {
            //     Console.Write(s+" ");
            // }
            // Console.WriteLine(faces2.Texture);
            // Console.WriteLine(faces1.CompareTo(faces2));
            var p = SetPlayers(10, pokers);
            List<Faces> players = new List<Faces>();
            foreach (var s in p)
            {
                players.Add(new Faces(s.ToArray()));
            }
            players.Sort();
            foreach (var player in players)
            {
                player.print();
            }

            Console.WriteLine("The winners are:");
            List<Faces> winners = new List<Faces>();
            winners.Add(players[0]);
            int index = 1;
            while (index<players.Count && players[index].CompareTo(players[index-1])==0)
            {
                winners.Add(players[index]);
                index++;
            }

            foreach (var winner in winners)
            {
                winner.print();
            }
        }

        static List<T> RandomList<T>(int length, List<T> list)
        {
            List<T> result = new List<T>();
            Random rd = new Random(Guid.NewGuid().GetHashCode());
            List<int> index = new List<int>();
            int next=rd.Next(0, list.Count);
            for (int i = 0; i < length; i++)
            {
                while (index.Contains(next))
                {
                    next = rd.Next(0, list.Count);
                }
                result.Add(list[next]);
                index.Add(next);
            }
            return result;
        }

        static List<List<string>> SetPlayers(int numberOfPlayers,List<string> pokers)
        {
            List<List<string>> players = new List<List<string>>();
            var tableList = RandomList(5, pokers);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players.Add(tableList.ToList());
            }

            foreach (var s in tableList)
            {
                pokers.Remove(s);
            }

            for (int i = 0; i < numberOfPlayers; i++)
            {
                var poker = RandomList(2, pokers);
                players[i].AddRange(poker.ToArray());
                pokers.Remove(poker[0]);
                pokers.Remove(poker[1]);
            }

            return players;
        }
    }
}