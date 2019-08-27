/*
 * Author      : Group01
 * filename    : Round.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the class that represents the round that supports the ICloneable interface
 */

using CardLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Durak
{
    public class Round : List<Turn>, ICloneable
    {
        /// <summary>
        /// Tracks what is currently happening..attacking, defending
        /// This aids in controling game flow
        /// </summary>
        private Game m_Game;
        public bool attackMode = true;
        private Turn currAttacker = null;
        private Turn currDefender = null;
        private int m_ID = 0;
        public int m_CurrentTurn = 0;//tracks actual current turn
        
        public Round()
        {
            m_ID++;
        }
        // on init, all players are guaranteed at least one turn
        public bool Initialize(Game game, Players players, int iLoserId =0)
        {
            m_Game = game;
            bool bRet = false;
            Player prevLosingPlayer = players[iLoserId];
            Add(new Turn(prevLosingPlayer, HandType.attack, Count));
            int i = 1;
            foreach (Player player in players)
            {
                if (prevLosingPlayer != player)
                {
                    Add(new Turn(player, (HandType)(i % 2), Count));
                    i++;
                }
            }
            if (PlayerType.computer == prevLosingPlayer.GetMode())
            {
                Game.btnDoneGuard = true;//not exactly a short-circuit
                m_Game.Timed_Response(ResponseType.next_turn_bypass);
            }
            bRet = true;//there used to be a try/catch here
            return bRet;
        }
        
        public void Expand()
        {
            if (Count <= m_CurrentTurn)//if there aren't enough turns..
            {
                Turn newTurn = GetCurrentTurn();
                Add(newTurn++);//adds a new turn automatically
            }
        }
        //always returns the results of the last turn in the round
        public bool HasPlayerLost(Player player)
        {
            bool bRet = false;
            foreach (Turn turn in this)
            {
                if (turn.GetPlayer() == player)
                {
                    bRet = turn.isLoser();
                }
            }
            return bRet;
        }
        public Player FindLoser()
        {
            Player loser = null;
            foreach (Player player in Game.m_Players)
            {
                if (HasPlayerLost(player))
                {
                    loser = player;
                }
            }
            return loser;
        }
        public Turn GetCurrentTurn()
        {
            return this[m_CurrentTurn];//if you are excepting here, it means you are letting players play cards in other players hands
        }
     
        public void outputStatusToWindow(ref GameGui gg)
        {
            foreach (Turn turn in this)
            {
                Player myPlayer = turn.GetPlayer();
                switch (myPlayer.ID)
                {
                    case 0:
                        gg.lblPlayer0.Content = "Player 0 (" + ((PlayerType.computer==myPlayer.GetMode())?"computer":"human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        Game.WriteToLog(gg.lblPlayer0.Content.ToString());
                        break;
                    case 1:
                        gg.lblPlayer1.Content = "Player 1 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        Game.WriteToLog(gg.lblPlayer1.Content.ToString());
                        break;
                    case 2:
                        gg.lblPlayer2.Content = "Player 2 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        Game.WriteToLog(gg.lblPlayer2.Content.ToString());
                        break;
                    case 3:
                        gg.lblPlayer3.Content = "Player 3 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        break;
                    case 4:
                        gg.lblPlayer4.Content = "Player 4 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        break;
                    case 5:
                        gg.lblPlayer5.Content = "Player 5 (" + ((PlayerType.computer == myPlayer.GetMode()) ? "computer" : "human") + ") round " + GetHashCode().ToString() + " turn no. " + turn.GetHashCode().ToString();
                        break;

                    default:
                        break;
                }
            }
        }
        /// <summary>
        /// Gets the player currently defending this round
        /// </summary>
        public void GetAttacker()
        {
            uint attackerChoice = GetRandom.RangedRandom.GenerateUnsignedNumber(0, Convert.ToUInt32((this.Count<Turn>())-1), 0);
            currAttacker = this.ElementAt(Convert.ToInt32(attackerChoice));
            if (attackerChoice == 5)
            {
                currDefender = this.ElementAt(0);
            }
            else
            {
                currDefender = this.ElementAt((Convert.ToInt32(attackerChoice)) + 1);
            }
        }

        public int PlayerPlayOrder()
        {
            int iRet = 0;
            Round prevRound = (Round)Clone();
            prevRound--;
            if (null != prevRound)
            {
                Player prevLostPlayer = prevRound.FindLoser();
                if (null != prevLostPlayer)
                {
                    iRet = prevLostPlayer.ID;
                }
            }
            return iRet;
        }

        /// <summary>
        /// returns a turn that has been decremented
        /// </summary>
        /// <param name="turn">Round</param>
        /// <returns>Round (may be null)</returns>
        public static Round operator --(Round round)
        {
            Round aRound = null;
            if (Game.m_Rounds.Count > 0)
            {
                int iRoundId = Game.m_Rounds[Game.m_Rounds.Count - 1].m_ID;

                if (round.m_ID > iRoundId)
                {
                    aRound = Game.m_Rounds[iRoundId];
                }
                else
                {
                    aRound = Game.m_Rounds[round.m_ID];
                }
            }
            return aRound;
        }
        /// <summary>
        /// Implement a deep clone of the round.
        /// </summary>
        /// <returns>object</returns>
        /// 
        public object Clone()
        {
            Round round = new Round();
            round.m_ID--;//decrement as constructor always increments
            return round;
        }
        /// <summary>
        /// Used to return value that identifies the player object
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return m_ID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="turn"></param>
        /// <param name="gg"></param>
        public void updateBoldedStatus(Turn turn, ref GameGui gg)
        {
            gg.lblPlayer0.FontWeight = FontWeights.Normal;
            gg.lblPlayer1.FontWeight = FontWeights.Normal;
            gg.lblPlayer2.FontWeight = FontWeights.Normal;
            gg.lblPlayer3.FontWeight = FontWeights.Normal;
            gg.lblPlayer4.FontWeight = FontWeights.Normal;
            gg.lblPlayer5.FontWeight = FontWeights.Normal;
            switch (turn.GetPlayer().ID)
            {
                case 0:
                    gg.lblPlayer0.FontWeight = FontWeights.Bold;
                    break;
                case 1:
                    gg.lblPlayer1.FontWeight = FontWeights.Bold;
                    break;
                case 2:
                    gg.lblPlayer2.FontWeight = FontWeights.Bold;
                    break;
                case 3:
                    gg.lblPlayer3.FontWeight = FontWeights.Bold;
                    break;
                case 4:
                    gg.lblPlayer4.FontWeight = FontWeights.Bold;
                    break;
                case 5:
                    gg.lblPlayer5.FontWeight = FontWeights.Bold;
                    break;

                default:
                    break;
            }
        }
    }
}
