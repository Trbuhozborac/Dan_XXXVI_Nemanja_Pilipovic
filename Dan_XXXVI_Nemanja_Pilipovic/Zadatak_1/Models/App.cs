using System;
using System.Collections.Generic;
using System.Threading;

namespace Zadatak_1.Models
{
    class App
    {
        static int[,] matrix;
        static List<int> randomNumbers = new List<int>();
        static Random random = new Random();
        static Object obj = new Object();

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
    }
}
