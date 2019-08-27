// Filename: Cards.cs
// Project: CardLib
// Author: Ryan Beckett
// Version: 1.4
// Date: Feb. 5, 2018
// Since: Mar. 11, 2018
// Description: A class for dealing with collections of cards. Most of the comparison
// operations require an equal number of cards to be compared with.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    public abstract class PlayingCards : List<PlayingCard>, ICloneable, IComparable
    {
        /// <summary>
        /// Required ICollectionBase method
        /// </summary>
        /// <param name="cards">PlayingCards</param>
        public void CopyTo(PlayingCards cards)
        {
            for (int i = 0; i < this.Count; i++)
            {
                cards[i] = this[i];
            }
        }
        /// <summary>
        /// IClonable support
        /// </summary>
        /// <returns>PlayingCards object</returns>
        public abstract object Clone();
        /// <summary>
        /// Required for IComparable, uses relational operators to determine whether one
        /// set of cards is greater or less than another
        /// </summary>
        /// <param name="obj">object (PlayingCards implied)</param>
        /// <returns>-1, 0, 1</returns>
        public int CompareTo(object obj)
        {
            // test if it's PlayingCards
            if (obj is PlayingCards)
            {
                //difference
                return this.GetHashCode() - obj.GetHashCode();
            }
            else
            {
                throw (new ArgumentException("Cannot compare PlayingCards objects with objects of type {0}", obj.GetType().ToString()));
            }
        }
        /// <summary>
        /// Used to generate a semi-unique value that identifies the PlayingCards as an object
        /// </summary>
        /// <returns>int</returns>
        /// <remarks>uses PlayingCard class GetHashCode
        /// TODO: account for count of cards?
        /// </remarks>
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (PlayingCard card in this)
            {
                hashCode += card.GetHashCode();
            }
            return hashCode;
        }
        #region COMPARISON AND RELATIONAL OPERATORS
        /// <summary>
        /// used to determine if the 2 PlayingCards objects passed in are equals
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool operator ==(PlayingCards leftCards, PlayingCards rightCards)
        {
            bool bRet = false;
            int rhsCountEquals = 0;
            int lhsCountEquals = 0;
            int cntLeftCards = leftCards.Count;
            int cntRightCards = rightCards.Count;

            if (cntLeftCards < cntRightCards)
            {
                rhsCountEquals = getEqualsTos(rightCards, leftCards);
                bRet = (rhsCountEquals == cntRightCards);
            }
            else// (cntLeftCards > cntRightCards) || (cntLeftCards == cntRightCards)
            {
                lhsCountEquals = getEqualsTos(leftCards, rightCards);
                bRet = (lhsCountEquals == cntLeftCards);
            }
            return bRet;
        }
        /// <summary>
        /// used to determine if the 2 PlayingCards objects passed in aren't equal
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool operator !=(PlayingCards leftCards, PlayingCards rightCards)
        {
            return !(leftCards == rightCards);
        }
        /// <summary>
        /// must be overridden whenever we are overriding the mathematical operators
        /// </summary>
        /// <param name="obj">PlayingCards (implied)</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj);
        }
        /// <summary>
        /// Equals implementation (Required for IEquatable)
        /// </summary>
        /// <param name="obj">PlayingCards</param>
        /// <returns>bool</returns>
        public bool Equals(PlayingCards obj)
        {
            return this == (PlayingCards)obj;
        }
        /// <summary>
        /// greater than relational operator cycles through the cards, evaluating whether the 
        /// cumulative value of cards in the first set is higher than the other
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool operator >(PlayingCards leftCards, PlayingCards rightCards)
        {
            bool bRet = false;
            if (leftCards != rightCards)
            {
                int rhsGreaterThans = 0;
                int lhsGreaterThans = 0;
                // only compare if the number of cards is equals
                if (getSameLength(leftCards, rightCards))
                {
                    rhsGreaterThans = getGreaterThans(rightCards, leftCards);
                    lhsGreaterThans = getGreaterThans(leftCards, rightCards);
                    bRet = (lhsGreaterThans > rhsGreaterThans);
                }
            }

            return bRet;
        }
        /// <summary>
        /// counts each card in each collection and evaluates how many cards are equals in both collections
        /// </summary>
        /// <param name="firstCards">PlayingCards</param>
        /// <param name="secondCards">PlayingCards</param>
        /// <returns>int</returns>
        private static int getEqualsTos(PlayingCards firstCards, PlayingCards secondCards)
        {
            int equalsTos = 0;
            int cntFirstCards = firstCards.Count;
            int cntSecondCards = secondCards.Count;
            //offset cnt because .Count att. is 1-based
            for (int secondCounter = cntSecondCards - 1; secondCounter >= 0; secondCounter--)
            {
                for (int firstCounter = cntFirstCards - 1; firstCounter >= 0; firstCounter--)
                {
                    if (firstCards[firstCounter] == secondCards[secondCounter])
                    {
                        equalsTos++;
                    }
                }
            }
            return equalsTos;
        }
        /// <summary>
        /// counts each card in each collection and evaluates how many cards in first parameter are 
        /// greater thans cards in the second parameter
        /// </summary>
        /// <param name="firstCards">PlayingCards</param>
        /// <param name="secondCards">PlayingCards</param>
        /// <returns>int</returns>
        private static int getGreaterThans(PlayingCards firstCards, PlayingCards secondCards)
        {
            int greaterThans = 0;
            int cntFirstCards = firstCards.Count;
            int cntSecondCards = secondCards.Count;
            //offset cnt because .Count att. is 1-based
            for (int secondCounter = cntSecondCards - 1; secondCounter >= 0; secondCounter--)
            {
                for (int firstCounter = cntFirstCards - 1; firstCounter >= 0; firstCounter--)
                {
                    if (firstCards[firstCounter] > secondCards[secondCounter])
                    {
                        greaterThans++;
                    }
                }
            }
            return greaterThans;
        }
        /// <summary>
        /// less than relational operator cycles through the cards, evaluating whether the 
        /// cumulative value of cards in the first set is less than the other
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool operator <(PlayingCards leftCards, PlayingCards rightCards)
        {
            bool bRet = false;
            if (leftCards != rightCards)
            {
                // only compare if the number of cards is equals
                if (getSameLength(leftCards, rightCards))
                {
                    bRet = !(leftCards > rightCards);
                }
            }
            return bRet;
        }
        /// <summary>
        /// greater than or equals to relational operator
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool operator >=(PlayingCards leftCards, PlayingCards rightCards)
        {
            bool bRet = false;
            if (leftCards == rightCards)
            {
                bRet = true;
            }
            else
            {
                bRet = (leftCards > rightCards);
            }
            return bRet;
        }
        /// <summary>
        /// less than or equals to relational operator
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool operator <=(PlayingCards leftCards, PlayingCards rightCards)
        {
            bool bRet = false;
            if (leftCards == rightCards)
            {
                bRet = true;
            }
            else
            {
                bRet = (leftCards < rightCards);
            }
            return bRet;
        }

        public PlayingCard GetLowestCard()
        {
            PlayingCard lowestCard = new PlayingCard();
            this.Sort();
            lowestCard = this.ElementAt(0);
            return lowestCard;
        }

        #endregion
        /// <summary>
        /// Function: getSameLength
        /// Determines if two groups of cards have the same number of cards in them
        /// </summary>
        /// <param name="leftCards">PlayingCards</param>
        /// <param name="rightCards">PlayingCards</param>
        /// <returns>bool</returns>
        public static bool getSameLength(PlayingCards leftCards, PlayingCards rightCards)
        {
            bool bRet = false;
            int cntLeftCards = leftCards.Count;
            int cntRightCards = rightCards.Count;
            if (!(cntLeftCards < cntRightCards))
            {
                if (!(cntRightCards < cntLeftCards))
                {
                    bRet = true;
                }
            }
            return bRet;
        }
        /// <summary>
        /// Used to determine how many cards in this group of cards are facedown
        /// </summary>
        /// <returns>int</returns>
        public int getCountCardsFaceDown()
        {
            int count = 0;
            foreach (PlayingCard card in this)
            {
                if (!card.Faceup)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// retrieves the number of suits in this collection
        /// </summary>
        /// <param name="suitIn">Suit</param>
        /// <returns>int</returns>
        public int getCountBySuit(Suit suitIn)
        {
            int count = 0;
            foreach (PlayingCard card in this)
            {
                if (suitIn == card.suit)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// retrieves the number of ranks in this collection
        /// </summary>
        /// <param name="rankIn">Rank</param>
        /// <returns>int</returns>
        public int getCountByRank(Rank rankIn)
        {
            int count = 0;
            foreach (PlayingCard card in this)
            {
                if (rankIn == card.rank)
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// Turns the cards over so that they are opposite of what they were before
        /// </summary>
        public void Turnover()
        {
            foreach (PlayingCard card in this)
            {
                card.Flip();
            }
        }
    }//PlayingCards
}
