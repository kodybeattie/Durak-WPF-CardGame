// Filename: Util.cs
// Project: CardLib
// Author: Ryan Beckett
// Version: 1.0
// Date: Mar. 20, 2018
// Description: A class containing utility functions.
// additional help for function getBitSet from:
/**
 * Kernighan, B. W., & Ritchie, D. M. (1988).
 * The C programming language. Englewood Cliffs, NJ: Prentice-Hall.
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public static class Util
    {
        /// <summary>
        /// Given a suit size, calculates the start of ranks in the suit
        /// </summary>
        /// <param name="suitSize">int</param>
        /// <returns>int</returns>
        public static int CalculateBaseRank(int suitSize)
        {
            int iBaseRank = 0;
            if (suitSize == 9)
            {
                iBaseRank = 6;
                PlayingCard.isAceHigh = true;
            }
            else if (suitSize == 5)
            {
                iBaseRank = 10;
                PlayingCard.isAceHigh = true;
            }
            else
            {
                iBaseRank = (int)((PlayingCard.isAceHigh) ? Rank.Two : Rank.Ace);
            }
            return iBaseRank;
        }
        /// <summary>
        /// Given a suit size, calculates the suit size taking into account the matter of aces high
        /// </summary>
        /// <param name="suitSize">number of ranks in suit</param>
        /// <returns>int</returns>
        public static int CalculateOffsetSuitSize(int suitSize)
        {
            int offset = suitSize - (suitSize - (int)PlayingCard.baseRank) - (int)((PlayingCard.isAceHigh) ? Rank.Two : Rank.Ace);
            int offsetSuitSize = suitSize + offset;
            return offsetSuitSize;
        }
        public static int CalculateInitialHandSize(int suitSize, int numPlayers)
        {
            int iRet = 0;
            switch (numPlayers)
            {
                case 2:
                    { iRet = 6; break; }
                case 3:
                    { iRet = 6; break; }
                case 4:
                    { iRet = (suitSize > 5) ? 6 : 5; break; }
                case 5:
                    { iRet = (suitSize > 5) ? 6: 4; break; }
                case 6:
                    { iRet = (suitSize > 5) ? 6 : 3; break; }
                default:
                    break;
            }
            return iRet;
        }
        /// <summary>
        /// The following function sourced from:
        /// Exercise 2.9 - Kernighan, B. W., & Ritchie, D. M. (1988).
        /// The C programming language. Englewood Cliffs, NJ: Prentice-Hall.
        /// </summary>
        /// <param name="x">input number</param>
        /// <param name="p">position to search from</param>
        /// <param name="n">number of bits to find</param>
        /// <returns>int</returns>
        public static int getbits(int x, int p, int n)
        {
            return (x >> (p + 1)) & ~(~0 << n);
        }
        public static String SuitToStr(Suit suit)
        {
            String str = "";
            switch (suit)
            {
                case Suit.Club:
                    str = "club";
                    break;
                case Suit.Diamond:
                    str = "diamond";
                    break;
                case Suit.Heart:
                    str = "heart";
                    break;
                case Suit.Spade:
                    str = "spade";
                    break;
                default:
                    break;
            }
            return str;
        }
    }
}
