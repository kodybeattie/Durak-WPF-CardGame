// Project: CardLib
// Filename: PlayingCard.cs
// Author: Ryan Beckett
// Version: 1.0
// Date: Mar. 16, 2018
// Description: A class representing an playing card that supports additional
// properties and methods than a regular playing card.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace CardLib
{
    public class PlayingCard : Card, ICloneable, IComparable, IEquatable<PlayingCard>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public PlayingCard() : base(Suit.Club, Rank.Two, 13) { } // unused

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="theSuit">enum</param>
        /// <param name="theRank">enum</param>
        /// <param name="bFaceUp">bool</param>
        public PlayingCard(Suit theSuit, Rank theRank, int suitSize, bool bFaceUp = true)
            : base(theSuit, theRank, suitSize)
        {
            isFaceup = bFaceUp;
        }

        /// <summary>
        /// Faceup property is only to be used by playing cards
        /// </summary>
        private bool isFaceup;
        public bool Faceup
        {
            get { return isFaceup; }
            set
            {
                isFaceup = value;
            }
        }
        /// <summary>
        /// BaseRank
        /// </summary>
        public static Rank baseRank = Rank.Ace;

        /// <summary>
        /// Converts the PlayingCard's contents (suit, rank, faceup) to text
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            return rank + "_of_" + suit + "s";//, Face " + ((Faceup)?"Up":"Down");
        }

        /// <summary>
        /// Trump usage. If true, trumps are higher than others
        /// </summary>
        public static bool useTrumps = false;

        /// <summary>
        /// suit to use if useTrumps is true
        /// </summary>
        public static Suit trump = Suit.Club;

        /// <summary>
        /// whether aces are higher than kings or lower than deuces
        /// </summary>
        public static bool isAceHigh = true;

        #region COMPARISONS AND RELATIONAL OPERATORS
        /// <summary>
        /// greater than relational operator
        /// </summary>
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <remarks>Also contains logic to handle our custom rules (trump, acehigh etc.)</remarks>
        /// <returns>bool</returns>
        public static bool operator >(PlayingCard leftCard, PlayingCard rightCard)
        {
            if (leftCard.suit == rightCard.suit)
            {
                if (isAceHigh)
                {
                    if (leftCard.rank == Rank.Ace)
                    {
                        if (rightCard.rank == Rank.Ace)
                        {
                            // comparing aces
                            return false;
                        }
                        else
                        {
                            // ace is higher
                            return true;
                        }
                    }
                    else
                    {
                        if (rightCard.rank == Rank.Ace)
                        {
                            // ace is higher
                            return false;
                        }
                        else
                        {
                            // no ace found, so just return the comparison
                            return (leftCard.rank > rightCard.rank);
                        }
                    }
                }
                else
                {
                    // no ace found, so just return the comparison
                    return (leftCard.rank > rightCard.rank);
                }
            }
            else
            {
                if (useTrumps && (rightCard.suit == PlayingCard.trump))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// less than relational operator
        /// </summary>
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator <(PlayingCard leftCard, PlayingCard rightCard)
        {
            return !(leftCard > rightCard);
        }
        /// <summary>
        /// greater than or equals to relational operator
        /// </summary>
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <remarks>Also contains logic to handle our custom rules (trump, acehigh etc.)</remarks>
        /// <returns>bool</returns>
        public static bool operator >=(PlayingCard leftCard, PlayingCard rightCard)
        {
            if (leftCard.suit == rightCard.suit)
            {
                if (isAceHigh)
                {
                    if (leftCard.rank == Rank.Ace)
                    {
                        // ace is higher
                        return true;
                    }
                    else
                    {
                        if (rightCard.rank == Rank.Ace)
                        {
                            // ace is higher
                            return false;
                        }
                        else
                        {
                            // no ace found, so just return the comparison
                            return (leftCard.rank >= rightCard.rank);
                        }
                    }
                }
                else
                {
                    // no ace found, so just return the comparison
                    return (leftCard.rank >= rightCard.rank);
                }
            }
            else
            {
                if (useTrumps && (rightCard.suit == PlayingCard.trump))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        /// <summary>
        /// less than or equals to relational operator
        /// </summary>
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator <=(PlayingCard leftCard, PlayingCard rightCard)
        {
            return !(leftCard >= rightCard);
        }
        /// <summary>
        /// Used to generate a semi-unique value that identifies the PlayingCard as an object
        /// </summary>
        /// <returns>int</returns>
        /// <remarks>uses base class GetHashCode and accounts for isFaceUp</remarks>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">PlayingCard (implied)</param>
        /// <returns>int</returns>
        public int CompareTo(object obj)
        {
            // test if it's PlayingCard
            if (obj is PlayingCard)
            {
                //difference
                return this.GetHashCode() - obj.GetHashCode();
            }
            else
            {
                throw (new ArgumentException("Cannot compare PlayingCard objects with objects of type {0}", obj.GetType().ToString()));
            }
        }
        /// <summary>
        /// used to determine if the 2 PlayingCard objects are equal
        /// </summary>
        /// <param name="leftCard">PlayingCard (Card implied)</param>
        /// <param name="rightCard">PlayingCard (Card implied)</param>
        /// <remarks>This method uses the base class's CardEquals method to do the comparison</remarks>
        /// <returns>bool</returns>
        public static bool operator ==(PlayingCard leftCard, PlayingCard rightCard)
        {
            bool bRet = false;
            //if (leftCard.Faceup & rightCard.Faceup)
            {
                //overcome the limitations of overloading an operator for a reference type
                bRet = leftCard.CardEquals(rightCard);
            }
            return bRet;
        }

        /// <summary>
        /// used to determine if the 2 PlayingCard objects are not equal
        /// </summary>
        /// <param name="leftCard">PlayingCard</param>
        /// <param name="rightCard">PlayingCard</param>
        /// <returns>bool</returns>
        public static bool operator !=(PlayingCard leftCard, PlayingCard rightCard)
        {
            return !(leftCard == rightCard);
        }
        /// <summary>
        /// must be overridden whenever we are overriding the mathematical operators
        /// </summary>
        /// <param name="obj">PlayingCard (implied)</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj);
        }
        /// <summary>
        /// Equals implementation (Required for IEquatable)
        /// </summary>
        /// <param name="obj">PlayingCard</param>
        /// <returns>bool</returns>
        public bool Equals(PlayingCard obj)
        {
            return this == (PlayingCard)obj;
        }
        /// <summary>
        /// Required to overcome the limitations of overloading an operator for a reference type
        /// </summary>
        /// <param name="obj">PlayingCard (implied)</param>
        /// <remarks>Requires faceup comparison to be performed beforehand</remarks>
        /// <returns>bool</returns>
        protected override bool CardEquals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// returns a PlayingCard that has been incremented as long as the new card doesn't go past suitsize or give an invalid rank
        /// </summary>
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCard (may be null)</returns>
        public static PlayingCard operator ++(PlayingCard card)
        {
            int offset = (int)suitSize - ((int)suitSize - (int)baseRank) - (int)((PlayingCard.isAceHigh) ? Rank.Two : Rank.Ace);
            int offsetSuitSize = (int)suitSize + offset;
            PlayingCard newCard = null;
            if (isAceHigh)
            {
                //if existing cards rank is greater or equal to offsetted suit size, and ace is high, set to ace
                if ((int)card.rank >= offsetSuitSize)
                {
                    newCard = new PlayingCard(card.suit, Rank.Ace, suitSize, false);
                }
                else
                {
                    //if existing cards rank is equals to offsetted suit size, and ace is high, set to ace
                    if ((int)card.rank == offsetSuitSize)
                    {
                        newCard = new PlayingCard(card.suit, Rank.Ace, suitSize, false);
                    }
                    else
                    {
                        //if existing cards rank is equals to ace, and ace is high, rotate suit
                        if (card.rank == Rank.Ace)
                        {
                            newCard = new PlayingCard((Suit)((int)++(card.suit)), (Rank)baseRank, suitSize, false);
                        }
                        else
                        {
                            //increment rank
                            newCard = new PlayingCard(card.suit, (Rank)((int)++(card.rank)), suitSize, false);
                        }
                    }
                }
            }
            else
            {
                //if existing cards rank is greater or equal to offsetted suit size, and ace is not high, rotate suit
                if ((int)card.rank >= offsetSuitSize)
                {
                    newCard = new PlayingCard((Suit)((int)++(card.suit)), Rank.Ace, suitSize, false);
                }
                else
                {
                    //increment rank
                    newCard = new PlayingCard(card.suit, (Rank)((int)++(card.rank)), suitSize, false);
                }
            }
            return newCard;
        }
        /// <summary>
        /// returns a card that has been decremented, as long as the new card's suit isn't negative or an invalid rank
        /// </summary>
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCard (may be null)</returns>
        public static PlayingCard operator --(PlayingCard card)
        {
            PlayingCard newCard = null;
            if ((int)(card.rank - 1) < (int)baseRank)
            {
                if (!((int)(card.suit - 1) < 0))//decrement suit and rotate rank
                {
                    newCard = new PlayingCard((Suit)((int)--(card.suit)),
                        ((isAceHigh) ? Rank.Ace : Rank.King), suitSize, false);
                }
            }
            else//decrement rank
            {
                newCard = new PlayingCard(card.suit, (Rank)((int)--(card.rank)), suitSize, false);
            }
            return newCard;
        }
        
        #endregion
        /// <summary>
        /// Implement a deep clone of the PlayingCard.
        /// </summary>
        /// <returns>object</returns>
        public override object Clone()
        {
            return MemberwiseClone();
        }
        /// <summary>
        /// Flips the playing card
        /// </summary>
        public void Flip()
        {
            Faceup = !Faceup;
        }

    }//PlayingCard
}
