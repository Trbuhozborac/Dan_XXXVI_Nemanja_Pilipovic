using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Zadatak_1.Models
{
    class App
    {
        static int[,] matrix;
        static List<int> randomNumbers = new List<int>();
        static Random random = new Random();
        static Object obj = new Object();
        static readonly string location = @"~\..\..\..\OddNumbers.txt";
        

        public void Start()
        {
            CreateThreads();
        }

        private void CreateThreads()
        {
            Thread threadOne = new Thread(() => CreateMatrix());
            Thread threadTwo = new Thread(() => PopulateMatrix());
            threadOne.Start();
            threadTwo.Start();
            threadTwo.Join();

            Thread threadThree = new Thread(() => GetOddNumbers());
            threadThree.Start();
            threadThree.Join();

            Thread threadFour = new Thread(() => WriteFromFile());
            threadFour.Start();
        }

        private void CreateMatrix()
        {
            lock (obj)
            {
                matrix = new int[100, 100];
                Monitor.Wait(obj);
            }
        }

        private void PopulateMatrix()
        {
            lock (obj)
            {
                Monitor.Pulse(obj);
                for (int i = 0; i < 100; i++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        matrix[i, j] = GetRandomNumber();
                    }
                }                                
            }            
        }

        private int GetRandomNumber()
        {
            int randomNumber = random.Next(10, 100);
            randomNumbers.Add(randomNumber);
            return randomNumber;            
        }

        private void GetOddNumbers()
        {
            List<int> oddNumbers = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                if(randomNumbers[i] % 2 == 0)
                {
                    continue;
                }
                else
                {
                    oddNumbers.Add(randomNumbers[i]);
                }
            }
            int[] oddNumbersArray = new int[oddNumbers.Count];
            for (int i = 0; i < oddNumbers.Count; i++)
            {
                oddNumbersArray[i] = oddNumbers[i];
            }
            if (File.Exists(location))
            {
                using (StreamWriter sw = new StreamWriter(location))
                {
                    foreach (int number in oddNumbersArray)
                    {
                        sw.WriteLine(number);
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found");
                Environment.Exit(0);
            }
        }


        private void WriteFromFile()
        {
            if (File.Exists(location))
            {
                using (StreamReader sr = new StreamReader(location))
                {
                    string numbers = sr.ReadToEnd();
                    Console.WriteLine(numbers);
                }
            }
            else
            {
                Console.WriteLine("File not found");
                Environment.Exit(0);
            }
        }
    }
}
