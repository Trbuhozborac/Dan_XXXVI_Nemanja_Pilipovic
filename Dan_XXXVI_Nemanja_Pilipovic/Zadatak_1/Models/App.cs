using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Zadatak_1.Models
{
    class App
    {
        #region Static Fields and Objects

        static int[,] matrix;
        static List<int> randomNumbers = new List<int>();
        static Random random = new Random();
        static Object obj = new Object();
        static readonly string location = @"~\..\..\..\OddNumbers.txt";

        #endregion

        #region Functions

        /// <summary>
        /// Starts Application by calling CreateThreads method
        /// </summary>
        public void Start()
        {
            CreateThreads();
        }

        /// <summary>
        /// Creates and starts the Threads and call methods for each thread
        /// </summary>
        private void CreateThreads()
        {
            Thread threadOne = new Thread(() => CreateMatrix());
            Thread threadTwo = new Thread(() => PopulateMatrix());
            threadOne.Start();
            threadTwo.Start();
            threadOne.Join();
            threadTwo.Join();
            
            Thread threadThree = new Thread(() => GetOddNumbers());
            Thread threadFour = new Thread(() => WriteFromFile());
            threadThree.Start();
            threadFour.Start();

        }

        /// <summary>
        /// Creates matrix 100 by 100
        /// </summary>
        private void CreateMatrix()
        {
            lock (obj)
            {
                matrix = new int[100, 100];
                Monitor.Pulse(obj);
                Monitor.Wait(obj);
            }
        }

        /// <summary>
        /// Populate matrix with random values
        /// </summary>
        private void PopulateMatrix()
        {
            lock (obj)
            {
                if(matrix == null)
                {
                    Monitor.Wait(obj);
                }
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

        /// <summary>
        /// Gets random value and put it in the randomNumbers List
        /// </summary>
        /// <returns>Random number between 10 and 99</returns>
        private int GetRandomNumber()
        {
            int randomNumber = random.Next(10, 100);
            randomNumbers.Add(randomNumber);
            return randomNumber;
        }

        /// <summary>
        /// Get all odd numbers from List with all numbers and write all odd nubmers to txt file
        /// </summary>
        private void GetOddNumbers()
        {
            List<int> oddNumbers = new List<int>();
            for (int i = 0; i < 10000; i++)
            {
                if (randomNumbers[i] % 2 == 0)
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
                lock (location)
                {                    
                    using (StreamWriter sw = new StreamWriter(location))
                    {
                        foreach (int number in oddNumbersArray)
                        {
                            sw.WriteLine(number);
                        }
                    }
                    Monitor.Wait(location);
                }
            }
            else
            {
                Console.WriteLine("File not found");
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Write to console all numbers from txt file
        /// </summary>
        private void WriteFromFile()
        {
            if (File.Exists(location))
            {
                lock (location)
                {
                    Monitor.Pulse(location);
                    using (StreamReader sr = new StreamReader(location))
                    {
                        string numbers = sr.ReadToEnd();
                        Console.WriteLine(numbers);
                    }
                }
            }
            else
            {
                Console.WriteLine("File not found");
                Environment.Exit(0);
            }
        }

        #endregion       
    }
}
