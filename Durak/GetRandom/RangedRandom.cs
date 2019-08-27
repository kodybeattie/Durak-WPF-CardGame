// Project: GetRandom
// Filename: RangedRandom.cs
// Author: Ryan Beckett
// Date: Feb. 3, 2018
// Description: A class library for generating random numbers
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetRandom
{
    /// <summary>
    /// Class: RangedRandom
    /// Used with permission from OOP II (3200-02) Lab 5 written on Dec. 26, 2017
    /// Author(s): Ryan Beckett, Mathew Kostrzewa, Shreeji Patel
    /// </summary>
    public static class RangedRandom
    {
        private static Random randomNumberGen = new Random();

        // Function: GenerateUnsignedNumber
        // Description: Used to generate a random number within a range
        public static uint GenerateUnsignedNumber(uint ceiling, uint entropy)
        {
            if (0 != ceiling)
            {
                double myDouble = randomNumberGen.NextDouble() * ceiling;
                return (uint)myDouble;
            }
            else
            {
                return 0;
            }
        }

        // Function: GenerateUnsignedNumber
        // Description: Used to generate a random number within a range
        public static uint GenerateUnsignedNumber(uint floor, uint ceiling, uint entropy)
        {
            uint temp = 0;
            if (0 != ceiling)
            {
                do
                {
                    temp = GenerateUnsignedNumber(ceiling, entropy);
                } while (temp < floor);
            }
            return temp;
        }

        // Function: PrimeRandomNumberGenerator
        // Description: seeds the Random Number Generator with time, then calls 
        // GenerateUnsignedNumber function to 'prime' the Random Number Generator
        public static void PrimeRandomNumberGenerator(uint length = 4, uint entropy = 0)
        {
            // seed the random number generator
            randomNumberGen = new Random();

            // with our custom random number generator, we must first prime it
            GenerateUnsignedNumber(length, entropy);
        }
    }
}
