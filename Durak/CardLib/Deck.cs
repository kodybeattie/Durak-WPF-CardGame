/**
* Project: CardLib
* Filename: Deck.cs
* Author: Ryan Beckett
* Version 1.8
* Date: Feb. 2, 2018
* Since: Mar. 16, 2018
* Description: A class representing a deck of playing cards 
* 
*/

using System;
using GetRandom;
namespace CardLib
{
    /// <summary>
    /// Deck class functions as a wrapper around a PlayingCards object.
    /// It contains most of the actual card game settings.
    /// The entry point of Deck is the constructor, where intialization flags are passed in.
    /// The primary exit point is the GetCard method, which removes cards from off the deck.
    /// </summary>
    public sealed class Deck : PlayingCards, ICloneable
    {
        // event definition to handle out of cards situation
        public event EventHandler LastCardDrawn;
        // event definition to handle deck updating
        public event EventHandler CardDealt;
        /// <summary>
        /// Actual amount of cards in the deck
        /// </summary>
        private int m_Count = 0;
        public int DeckCount
        {
            get { return m_Count; }
            private set
            {
                m_Count = value;
            }
        }
        /// <summary>
        /// Flags - deck initialization setting
        /// </summary>
        private int m_Flags = 227;
        public int Flags
        {
            get { return m_Flags; }
            private set
            {
                m_Flags = value;
            }
        }
        /// <summary>
        /// DeckSize - the capacity of the deck
        /// </summary>
        private int m_DeckSize = 35;
        public int DeckSize
        {
            get { return m_DeckSize; }
            private set
            {
                m_DeckSize = value;
            }
        }
        /// <summary>
        /// SuitSize - number of ranks allowed in a suit
        /// </summary>
        private int m_SuitSize = 9;
        public int SuitSize
        {
            get { return m_SuitSize; }
            private set
            {
                m_SuitSize = value;
            }
        }
        /// <summary>
        /// Entropy - a value which can modify the randomness of the deck's cards
        /// Best entropy source eg. clock()/iterator/100
        /// </summary>
        private int m_Entropy = 0;
        public int Entropy
        {
            get { return m_Entropy; }
            set
            {
                /*TODO: Enforce validation eg. shouldn't be greater than 51*/
                m_Entropy = value;
            }
        }
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Deck()
        {
            PlayingCard.useTrumps = true;
            Initialize();
        }
        /// <summary>
        /// Initializes the deck with sequence of cards, also sets some static properties
        /// and gets the random number generator ready
        /// </summary>
        private void Initialize()
        { 
            if ((int)DeckFlags.Large == m_DeckSize)
                m_SuitSize = 13;//incase forgot to add +4 to the flags controlling suit size
            PlayingCard.baseRank = (Rank)Util.CalculateBaseRank(m_SuitSize);//can be either 1, 2, 6 or 10
            PlayingCard newCard = new PlayingCard(Suit.Club, PlayingCard.baseRank, m_SuitSize);
            for (int i=0;i<=m_DeckSize;i++)
            {
                PlayingCard nextCard = (PlayingCard)newCard.Clone();
                Add(nextCard);
                newCard++;
            }
        }
        /// <summary>
        /// parameterized constructor - dissects flags parameter to extract all the information
        /// should only be used to parse flags
        /// </summary>
        /// <param name="flags">int</param>
        public Deck(int flags)
        {
            m_Flags = flags;
            PlayingCard.useTrumps = (0 != ((short)flags & (int)DeckFlags.UseTrump));
            PlayingCard.isAceHigh = (0 != ((short)flags & (int)DeckFlags.AceHigh));
            m_DeckSize = ((short)flags & (int)DeckFlags.Large);
            m_SuitSize = ((int)DeckFlags.Small==m_DeckSize)?5:(Util.getbits(flags, 1, 1) == 0) ? 9 : 13; // get 3rd bit from right
            PlayingCard.trump = (Suit)((flags >> 9) & (byte)DeckFlags.TrumpSuit);
            Initialize();
        }
        /// <summary>
        /// Used to shuffle the deck of cards. Deletes the contents of a PlayingCards 
        /// collection and recreates a new one with random cards
        /// </summary>
        public void Shuffle()
        {
            Clear();
            m_Count = 0;
            // loop is used to create cards and put them into newDeck
            // on each iteration, the class variable m_Deck is updated with the contents
            // of the newDeck array, as shown in the textbook
            for (int Counter = 0; Counter <= m_DeckSize; Counter++)
            {
                PlayingCard pCard = null;
                do
                {
                    uint floor = (uint)PlayingCard.baseRank;
                    uint ceiling = (uint)Util.CalculateOffsetSuitSize(m_SuitSize);
                    uint myRank = 0;
                    uint mySuit = RangedRandom.GenerateUnsignedNumber(4, 0);
                    if (PlayingCard.isAceHigh && (int)DeckFlags.Large != m_DeckSize)
                    {
                        ceiling++;
                        floor--;
                        myRank = RangedRandom.GenerateUnsignedNumber(floor, ceiling, (uint)m_Entropy) + 1;
                    }
                    else
                    {
                        myRank = RangedRandom.GenerateUnsignedNumber(ceiling, (uint)m_Entropy) + 1;
                    }
                    pCard = new PlayingCard((Suit)mySuit, (Rank)myRank, m_SuitSize, true);
                    
                } while (IsCardAlreadyInDeck(pCard));// if an existing card with the same suit and rank is there, don't add it
                Add(pCard);
                m_Count++;
            }
            //unfortunately the shuffle loop has a limitation that, when given a deck with an abnormal size
            //and aces high, the loop cannot accomodate the gap in ranks, so just set the rank ceiling to 14
            //and change numeric 14 cards to aces afterwards
            if (PlayingCard.isAceHigh && (int)DeckFlags.Large != m_DeckSize)
            {
                foreach (PlayingCard card in this)
                {
                    if (14 == (uint)card.rank)
                    {
                        card.rank = Rank.Ace;
                    }
                }
            }
            Turnover();
        }
        /// <summary>
        /// Used to determine if the card passed in as a parameter is already present in the deck
        /// </summary>
        /// <param name="card">PlayingCard</param>
        /// <remarks>if the deck didn't have all cards facedown, this method would not work</remarks>
        /// <returns>bool</returns>
        private bool IsCardAlreadyInDeck(PlayingCard card)
        {
            bool bRet = false;
            if (0 != Count)//TODO: should be m_Count
            {
                bRet = Contains(card);
            }
            return bRet;
        }
        /// <summary>
        /// Takes a card from the deck, and returns it as an object, also removing it from the deck
        /// </summary>
        /// <param name="iIndex">int</param>
        /// <returns>Card object</returns>
        public PlayingCard DealCard(int iIndex) 
        {
            PlayingCard card = null;
            if (iIndex >= 0 && iIndex <= m_DeckSize)
            {
                // if iIndex is same as size, then it could be last card in deck
                if ((iIndex == (Count-1)) && (LastCardDrawn != null))
                {
                    LastCardDrawn(this, EventArgs.Empty);
                    //goto errorOut;
                }
                card = (PlayingCard)this[iIndex].Clone();//deep clone
                Remove(this[iIndex]); // has been dealt
                m_Count--;
                if (CardDealt != null)
                {
                    CardDealt(this, EventArgs.Empty);
                }
            }
            else
            {
                throw new CardOutOfRangeException(Clone() as PlayingCards);
            }
            //errorOut:
            return card;
        }
        /// <summary>
        /// Used to implement the iCloneable interface
        /// This is a deep clone
        /// </summary>
        /// <returns>Object</returns>
        public override object Clone()
        {
            Deck deck = new Deck(m_Flags);
            return deck;
        }
        /// <summary>
        /// Adds a card to the deck
        /// </summary>
        /// <param name="cards">Deck</param>
        /// <param name="card">PlayingCard</param>
        /// <returns>Deck</returns>
        public static Deck operator +(Deck cards, PlayingCard card)
        {
            int deckLen = ((short)cards.m_Flags & (int)DeckFlags.Large);
            int suitLen = ((int)DeckFlags.Small == deckLen) ? 5 : (Util.getbits(cards.m_Flags, 1, 1) == 0) ? 9 : 13;
            int rankBase = Util.CalculateBaseRank(suitLen);
            if (!(cards.Contains(card)))
            {
                if (!((cards.Count + 1) > deckLen))
                {
                    if (!((cards.getCountBySuit(card.suit) + 1) > suitLen))
                    {
                        if (!((int)card.rank < rankBase))
                        {
                            cards.Add(card);
                        }
                        else
                        {
                            throw new CardOutOfRangeException(cards);
                        }
                    }
                    else
                    {
                        throw new CardOutOfRangeException(cards);
                    }
                }
                else
                {
                    throw new CardOutOfRangeException(cards);
                }
            }
            else
            {
                throw new CardAlreadyInDeckException(cards);
            }
            return cards as Deck;
        }
        /// <summary>
        /// Removes a card from the deck
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="card"></param>
        /// <returns></returns>
        public static Deck operator -(Deck cards, PlayingCard card)
        {
            if (cards.Count > 0)
            {
                if (cards.Contains(card))
                {
                    cards.Remove(card);
                }
            }
            return cards as Deck;
        }
    }//Deck
}
