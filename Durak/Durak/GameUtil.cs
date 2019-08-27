/*
 * Author      : Group01
 * filename    : GameUtil.cs
 * Date        : 10-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This file includes the utility functions and methods for the game of durak
 */

using CardLib;
using System.Windows;

namespace Durak
{
    public static class GameUtil
    {
        public static int getCountMaxRanksInHand(Hand hand)
        {
            int[] arrCntRanks = new int[14];
            for (int i = 0; i < hand.Count; i++)
            {
                for (int j = 13; j > 0; j--)
                {
                    if (hand[i].rank == (Rank)j)
                    {
                        arrCntRanks[j]++;
                    }
                }
            }
            int x = 0;
            for (int z = 1; z < 14; z++)
            {
                if (arrCntRanks[z] != 0)
                {
                    x++;
                }
            }
            return x;
        }
        public static bool ShallIDoThisForYou(string question)
        {
            MessageBoxResult result = MessageBox.Show(question, "Durak", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                return true;
            }
            else if (result == MessageBoxResult.No)
            {
                return false;
            }
            else
            {
                return false;
            }
        }
        public static Suit FindMostPrevalentSuit(Hand hand)
        {
            int iCountSuits = 0;
            int iPrevCountSuits = 0;
            Suit mostPrevalentSuit = Suit.Club;
            foreach (PlayingCard card in hand)
            {
                iCountSuits = hand.getCountBySuit(card.suit);
                if (iCountSuits > iPrevCountSuits)
                {
                    iPrevCountSuits = iCountSuits;
                    mostPrevalentSuit = card.suit;
                }
            }
            return mostPrevalentSuit;
        }
        public static bool doHandSuitsMatch(Hand leftHand, Hand rightHand)
        {
            bool bRet = false;
            Hand leftFlush = leftHand.getFlush();
            Hand rightFlush = rightHand.getFlush();
            bRet = (leftFlush.Count == rightFlush.Count);
            if (bRet)
            {
                //have flushes, just need to compare the actual suits of the flushes
                bRet = (leftFlush[0].suit == rightFlush[0].suit);
            }
            return bRet;
        }
        public static int GetCountTrumps(Hand hand)
        {
            int iRet = 0;
            foreach (PlayingCard card in hand)
            {
                if (PlayingCard.trump == card.suit)
                {
                    iRet++;
                }
            }
            return iRet;
        }
        public static Hand getTrumps(Hand handIn)
        {
            Hand newHand = new Hand(handIn.m_Type);
            foreach (PlayingCard card in handIn)
            {
                if (PlayingCard.trump == card.suit)
                {
                    newHand.Add(card);
                }
            }
            return newHand;
        }
        public static bool CompareHighestTrumps(Hand leftHand, Hand rightHand)
        {
            bool bRet = false;
            for (int i = 0; i < leftHand.Count; i++)
            {
                for (int j = 0; j < rightHand.Count; j++)
                {
                    if (leftHand[i] > rightHand[j])
                    {
                        if (PlayingCard.trump == leftHand[j].suit)
                        {
                            bRet = true;
                        }
                    }
                }
            }
            return bRet;
        }
        public static bool CompareTrumps(Hand leftHand, Hand rightHand)
        {
            int iCounter = 0;
            foreach (PlayingCard outerCard in rightHand)
            {
                foreach (PlayingCard innerCard in leftHand)
                {
                    if (outerCard > innerCard)
                    {
                        iCounter++;
                    }
                }
            }
            return (0 != iCounter);
        }
        public static bool CompareHands(Hand leftHand, Hand rightHand)
        {
            int iCounter = 0;
            foreach (PlayingCard outerCard in rightHand)
            {
                foreach (PlayingCard innerCard in leftHand)
                {
                    if (outerCard > innerCard)
                    {
                        iCounter++;
                    }
                }
            }
            return (0 != iCounter);
        }
    }
}