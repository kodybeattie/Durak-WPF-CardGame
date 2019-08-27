/*
 * Author      : Group01
 * filename    : Players.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This class is the representation of a players for the durak game
 */

using System;
using System.Collections.Generic;
using System.Linq;
using CardLib;
namespace Durak
{
    public class Players : List<Player>, ICloneable
    {

        public static int NumPlayers = 0;
        public Players(int numPlayers)
        {
            NumPlayers = numPlayers;
            if (!Initialize())
                throw new Exception();
        }
        public bool Initialize()
        {
            bool bRet = false;
            try
            {
                Add(new Player(0, PlayerType.human));
                for (int i = 1; i < NumPlayers; i++)
                {
                    Add(new Player(i, PlayerType.computer));
                }
                bRet = true;
            }
            catch (Exception ex)
            {

            }
            return bRet;
        }
        public bool DealHands(int chosenNumPlayers, int handSize, ref Deck deck, bool bInit = false)
        {
            bool bRet = true;
            foreach (Player player in this)
            {
                if (bInit)
                    player.m_Hand = new Hand(HandType.attack);
                while (player.m_Hand.Count() < handSize)
                {
                    if (!Game.m_bDeckOutFlag)
                    {
                        player.m_Hand.Add(deck.DealCard(0));
                        bRet = true;
                    }
                    else
                    {
                        bRet = false;
                        break;
                    }
                }
            }
            return bRet;
        }
        /// <summary>
        /// Required ICollectionBase method
        /// </summary>
        /// <param name="cards">Players</param>
        public void CopyTo(Players players)
        {
            for (int i = 0; i < this.Count; i++)
            {
                players[i] = this[i];
            }
        }
        /// <summary>
        /// Used to implement the iCloneable interface
        /// This is a shallow clone
        /// </summary>
        /// <returns>Object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
