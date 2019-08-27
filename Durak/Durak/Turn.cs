/*
 * Author      : Group01
 * filename    : Turn.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the turn class that is in charge of any player's turn
 */

using System;
using CardLib;

namespace Durak
{
    public class Turn : ICloneable
    {
        private bool m_bLoser = false;
        /// <summary>
        /// identifies turn in a round
        /// </summary>
        private int m_ID = 0;
        /// <summary>
        /// a type of hand unique to the turn (ie. attackHand or defenceHand)
        /// </summary>
        private Hand m_Hand = new Hand(HandType.attack);
        /// <summary>
        /// the player associated with the turn
        /// </summary>
        private Player m_Player = null;
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="curTurnID">id number</param>
        public Turn(int curTurnID = 0)
        {
            m_ID = ++curTurnID;
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="curTurnID">id number</param>
        public Turn(Player player, int curTurnID = 0)
        {
            m_Player = player;
            m_ID = ++curTurnID;
        }
        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="player">player</param>
        /// <param name="curTurnID">id number</param>
        /// 
        public Turn(Player player, HandType type, int curTurnID = 0)
        {
            m_Player = player;
            m_ID = ++curTurnID;
            m_Hand.m_Type = type;
        }
        /// <summary>
        /// lookup function used to query current and past turn results
        /// </summary>
        /// <returns>bool</returns>
        public bool isLoser()
        {
            return m_bLoser;
        }
        public void SetLost()
        {
            m_bLoser = true;
        }
        /// <summary>
        /// used to determine the mode of the turn
        /// </summary>
        /// <returns>bool</returns>
        public bool isDefending()
        {
            bool bRet = false;
            bRet = (HandType.defend == m_Hand.m_Type);
            return bRet;
        }
        /// <summary>
        /// gets the turn's hand
        /// </summary>
        /// <returns></returns>
        public Hand GetHand()
        {
            return m_Hand;
        }
        public void SetHand(Hand hand)
        {
            m_Hand = hand;
        }
        /// <summary>
        /// used to build a hand of attack cards
        /// </summary>
        /// <param name="cards">PlayingCards (or Hand)</param>
        /// <returns>PlayingCards (or Hand)</returns>
        public PlayingCards Attack(PlayingCards cards)
        {
            foreach (PlayingCard card in cards)
            {
                m_Hand += card;
            }
            return m_Hand;
        }
        /// <summary>
        /// used to build a hand of Attack cards
        /// </summary>
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCards (or Hand)</returns>
        public PlayingCards Attack(PlayingCard card)
        {
            m_Hand += card;
            return m_Hand;
        }
        
        /// <summary>
        /// used to build a hand of defense cards
        /// </summary>
        /// <param name="card">PlayingCard</param>
        /// <returns>PlayingCards (or Hand)</returns>
        public PlayingCards Defend(PlayingCard card)
        {
            m_Hand += card;
            return m_Hand;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Player GetPlayer()
        {
            return m_Player;
        }
        
        /// <summary>
        /// returns a turn that has been incremented
        /// </summary>
        /// <param name="turn">Turn</param>
        /// <returns>Turn (may be null)</returns>
        public static Turn operator ++(Turn turn)
        {
            int iTurnId = Game.m_Rounds[Game.m_Rounds.Count - 1].m_CurrentTurn;
            if (turn.m_ID > iTurnId)
            {
                //empty turn found, so return it
                //(ie. return Turn--;
                return Game.m_Rounds[Game.m_Rounds.Count - 1][turn.m_ID];
            }
            else
            {
                return new Turn(turn.m_Player++,(turn.m_Hand.m_Type==HandType.attack)?HandType.defend:HandType.attack, iTurnId);//player++ gets the next player in the list
            }
        }
        /// <summary>
        /// returns a turn that has been decremented
        /// </summary>
        /// <param name="turn">Turn</param>
        /// <returns>Turn (may be null)</returns>
        public static Turn operator --(Turn turn)
        {
            int iTurnId = Game.m_Rounds[Game.m_Rounds.Count - 1].m_CurrentTurn;//
            
            if (turn.m_ID > iTurnId)
            {
                return Game.m_Rounds[Game.m_Rounds.Count - 1][iTurnId - 1];
            }
            else
            {
                return Game.m_Rounds[Game.m_Rounds.Count - 1][turn.m_ID];
            }
        }
        /// <summary>
        /// Required for IComparable, uses relational operators to determine whether one
        /// turn is a loser or not
        /// </summary>
        /// <param name="obj">object (Turn implied)</param>
        /// <returns>-1, 0, 1</returns>
        public int CompareTo(object obj)
        {
            // test if it's turn
            if (obj is Turn)
            {
                if (Game.m_Players.Count < ((Turn)obj).m_ID)//only test for isLoser if they have played at least 1 round
                {
                    return (isLoser()) ? -1 : 1;
                }
                else
                {
                    //difference
                    return this.GetHashCode() - obj.GetHashCode();
                }
            }
            else
            {
                throw (new ArgumentException("Cannot compare Turn objects with objects of type {0}", obj.GetType().ToString()));
            }
        }
        /// <summary>
        /// Implement a deep clone of the turn.
        /// </summary>
        /// <returns>object</returns>
        /// 
        public object Clone()
        {
            Turn turn = new Turn(m_Player);
            turn.m_ID--;//decrement as constructor always increments
            return turn;
        }
        /// <summary>
        /// Used to return value that identifies the turn object
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            return m_ID;
        }
    }
}
