// Project: CardLib
// Filename: Card.cs
// Author: Ryan Beckett
// Version: 1.4
// Date: Feb. 3, 2018
// Since: Mar. 16, 2018
// Description: A class representing an playing card


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardLib
{
    /// <summary>
    /// Class: Card
    /// Description: card class, as directed by the textbook
    /// </summary>
    public abstract class Card : ICloneable
    {
        public Suit suit;
        public Rank rank;
        public static int suitSize;// needed for GetHashCode
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="suit">enum</param>
        /// <param name="rank">enum</param>
        /// <param name="suitSize">int</param>
        public Card(Suit suitIn, Rank rankIn, int suitSizeIn)
        {
            suit = suitIn;
            rank = rankIn;
            suitSize = suitSizeIn;
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Card()
        {
            suit = 0;
            rank = 0;
            suitSize = 13;
        }
        /// <summary>
        /// Converts the object's contents (suit, rank) to text
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return "The " + rank + " of " + suit + "s" ;
        }
        /// <summary>
        /// Used to implement the iCloneable interface
        /// </summary>
        /// <returns>Object</returns>
        public abstract object Clone();
        #region COMPARISON AND RELATIONAL OPERATORS
        /// <summary>
        /// used to determine if the 2 Cards passed in are equal
        /// </summary>
        /// <param name="leftCard">Card</param>
        /// <param name="rightCard">Card</param>
        /// <returns>bool</returns>
        public static bool operator ==(Card leftCard, Card rightCard)
        {
            return (leftCard.suit == rightCard.suit) && (leftCard.rank == rightCard.rank);
        }
        /// <summary>
        /// used to determine if the 2 cards passed in aren't equal
        /// </summary>
        /// <param name="leftCard">Card</param>
        /// <param name="rightCard">Card</param>
        /// <returns>bool</returns>
        public static bool operator !=(Card leftCard, Card rightCard)
        {
            return !(leftCard == rightCard);
        }
        /// <summary>
        /// must be overridden whenever we are overriding the mathematical operators
        /// </summary>
        /// <param name="obj">Card (implied)</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return this == (Card)obj;
        }
        /// <summary>
        /// Used to compare cards when called from a child class. Required to overcome the limitations
        /// of overloading an operator for a reference type
        /// </summary>
        /// <param name="obj">Card (implied)</param>
        /// <returns>bool</returns>
        protected abstract bool CardEquals(object obj);
        /// <summary>
        /// Used to generate a semi-unique value that identifies the Card object
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return suitSize * (int)suit + (int)rank;
        }
        #endregion
    } // Card
}
