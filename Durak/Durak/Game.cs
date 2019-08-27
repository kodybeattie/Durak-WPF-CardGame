/*
 * Author      : Group01
 * filename    : Gamecs
 * Date        : 01-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the game class that is used to initialize the game
 */

using CardLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO;

namespace Durak
{
    /// <summary>
    /// How the delay should respond
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// Clear everything
        /// </summary>
        clear_all = 0,
        /// <summary>
        /// Move to next turn
        /// </summary>
        next_turn,
        /// <summary>
        /// Jump over next turn
        /// </summary>
        next_turn_bypass
    }
    public class Game
    {
        /// <summary>
        /// The path to the file to write to
        /// </summary>
        private static String filePath = ("./logs/Game" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt");
        /// <summary>
        /// A deck to hold cards
        /// </summary>
        public Deck m_Deck = null;
        /// <summary>
        /// The deck to discard cards into
        /// </summary>
        public Deck discardDeck = new Deck(0);
        /// <summary>
        /// Flags when deck has been emptied
        /// </summary>
        public static bool m_bDeckOutFlag = false;
        /// <summary>
        /// A collection of players in the game
        /// </summary>
        public static Players m_Players = null;
        /// <summary>
        /// The GUI for the game
        /// </summary>
        public GameGui m_Gg = null;
        /// <summary>
        /// A list of rounds during the game
        /// </summary>
        public static List<Round> m_Rounds = new List<Round>();
        /// <summary>
        /// Handles chnaging the player order
        /// </summary>
        public static bool btnDoneGuard = false;//needed to deal with loser-won situations (change of player order)
        /// <summary>
        /// Controls the hand size of players
        /// </summary>
        public int m_HandSize = 0;
        DispatcherTimer m_DispatcherTimer = null;
        /// <summary>
        /// Creates a new game.
        /// </summary>
        /// <param name="numPlayers"></param>
        /// <param name="gameFlags"></param>
        /// <param name="gg"></param>
        public Game(int numPlayers, int gameFlags, GameGui gg)
        {
            m_Gg = gg;
            Initialize(numPlayers, gameFlags);//removed exception handling for release, so throw away bool
            m_Gg.SetupGui();
            m_Gg.UpdateDisplay(m_Players);//arranges red backgrounds according to number of players
            m_Gg.UpdateElements(m_Players);//adds cardboxes to screen based on players hands
            Round rnd = new Round();
            rnd.Initialize(this, m_Players);//game is ready for input
            m_Rounds.Add(rnd);
        }
        /// <summary>
        /// Initializes a game
        /// </summary>
        /// <param name="chosenNumPlayers"></param>
        /// <param name="gameFlags"></param>
        /// <returns></returns>
        public bool Initialize(int chosenNumPlayers, int gameFlags)
        {
            bool bRet = false;
            int trumpsuit = ((gameFlags >> 9) & (byte)DeckFlags.TrumpSuit);//process deck flags
            m_Deck = new Deck(gameFlags);
            m_Deck.LastCardDrawn += new System.EventHandler(m_Gg.Deck_LastCardDrawn);
            m_Deck.CardDealt += new System.EventHandler(m_Gg.Deck_CardDealt);
            m_Deck.Shuffle();
            m_Gg.gbxDeck.Header = "Deck: " + m_Deck.DeckCount + " cards";
            m_Players = new Players(chosenNumPlayers);
            m_HandSize = Util.CalculateInitialHandSize(m_Deck.SuitSize, chosenNumPlayers);
            m_Players.DealHands(chosenNumPlayers, m_HandSize, ref m_Deck, true);
            return bRet;
        }
        /// <summary>
        /// Handles when a cardbox has been clicked
        /// </summary>
        /// <param name="sender"></param>
        public void CardBoxClicked(object sender)
        {
            Turn curTurn = m_Rounds[m_Rounds.Count - 1].GetCurrentTurn();
            Turn prevTurn = (Turn)curTurn.Clone();
            prevTurn--;
            CardBox box = (CardBox)sender;
            PlayingCard selectedCard = (PlayingCard)(box).Card.Clone();//make a copy
            StackPanel sp = (StackPanel)box.Parent;
            Grid g = (Grid)sp.Parent;
            PlayerUI pui = (PlayerUI)g.Parent;
            pui.DeleteCard(box);
            //Validate input params...
            if (curTurn.isDefending())
            {
                m_Gg.m_BattleArea.AddDefenseCard(new CardBox(selectedCard, Orientation.Vertical, curTurn.GetHashCode()));
                curTurn.Defend(selectedCard);
                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " defended using " + selectedCard.ToString());
            }
            else
            {
                m_Gg.m_BattleArea.AddAttackCard(new CardBox(selectedCard, Orientation.Vertical, curTurn.GetHashCode()));
                curTurn.Attack(selectedCard);
                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " attacked using " + selectedCard.ToString());
            }
            curTurn.GetPlayer().m_Hand -= selectedCard;
            m_Rounds[m_Rounds.Count - 1].updateBoldedStatus(curTurn, ref m_Gg);
            btnDoneGuard = false;
        }
        /// <summary>
        /// Handles when the done button has been clicked
        /// </summary>
        /// <param name="bClickedByHuman"></param>
        public void DoneClicked(bool bClickedByHuman)
        {
            Round curRound = m_Rounds[m_Rounds.Count - 1];
            curRound.Expand();
            Turn curTurn = curRound.GetCurrentTurn();
            Turn prevTurn = (Turn)curTurn.Clone();
            prevTurn--;
            Player curPlayer = curTurn.GetPlayer();
            Player prevPlayer = prevTurn.GetPlayer();
            Hand prevHand = prevTurn.GetHand();
            Hand curHand = curTurn.GetHand();
            if (curTurn.GetPlayer().ID == prevTurn.GetPlayer().ID)
            {
                if (!curTurn.isDefending())//impossible to defend on the first turn
                {
                    if (!bClickedByHuman)
                    {
                        curHand = curPlayer.Attack(prevHand);//a player object may attack, only if it's not human
                        foreach (PlayingCard card in curHand)
                        {
                            m_Gg.m_BattleArea.AddAttackCard(new CardBox(card, Orientation.Vertical, curTurn.GetHashCode()));
                            curPlayer.m_Hand -= card;//remove the card from the player's hand
                            WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " attacked using " + card.ToString());
                        }
                        curTurn.SetHand(curHand);
                        if (0 == curHand.Count)
                        {
                            WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " player (prev turn) sucessfully attacked/defended");
                        }
                        m_Gg.UpdateElements(m_Players);//update UI
                    }
                }
                if (false == btnDoneGuard)
                {
                    Timed_Response(ResponseType.next_turn);//timed response gives time for the user to view cards on the table
                }
            }
            else
            {
                int iLoserId = 0;//used to track loser of this round. The loser will have the first turn in next round
                if (!bClickedByHuman)
                {
                    if (false == btnDoneGuard)
                    {
                        if (curTurn.GetPlayer().CanDefend(prevHand))
                        {
                            curHand = curPlayer.MakeDefense(prevHand);
                            foreach (PlayingCard card in curHand)
                            {
                                m_Gg.m_BattleArea.AddDefenseCard(new CardBox(card, Orientation.Vertical, curTurn.GetHashCode()));
                                curPlayer.m_Hand -= card;
                                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                    + " defended using " + card.ToString());
                            }
                            curTurn.SetHand(curHand);//override any existing hand
                            if (0 == curHand.Count)
                            {
                                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                        + " player (prev turn) sucessfully attacked/defended");
                                iLoserId = prevTurn.GetPlayer().ID;
                            }
                        }
                        else
                        {
                            iLoserId = ProcessLoser(prevTurn, prevHand, prevPlayer.ID, curTurn, curHand, bClickedByHuman);
                        }
                    }
                }
                //need to get the current count of cards in each player's hands
                int ranksInPrevHand = GameUtil.getCountMaxRanksInHand(prevHand);
                int ranksInCurHand = GameUtil.getCountMaxRanksInHand(curHand);
                bool bTemp = GameUtil.doHandSuitsMatch(prevHand, curHand);
                if (ranksInPrevHand == ranksInCurHand)//test for equal hands, do nothing if not equals
                {
                    if (false == btnDoneGuard)
                    {
                        if (prevHand > curHand)//compare
                        {
                            iLoserId = ProcessLoser(prevTurn, prevHand, prevPlayer.ID, curTurn, curHand, bClickedByHuman);
                        }
                        else if (curHand > prevHand)
                        {
                            iLoserId = ProcessLoser(curTurn, curHand, curPlayer.ID, prevTurn, prevHand, bClickedByHuman);
                        }
                        else
                        {//draw
                            MessageBox.Show("Draw");
                        }
                    }
                }
                else
                {
                    if (curTurn.isDefending())//attacking humans are free to place as many cards on the board as they want, errors to be caught by Cardbox_Clicked method
                    {
                        if (bClickedByHuman)//notify user of their mistake
                        { 
                            if (GameUtil.ShallIDoThisForYou("You did not place the correct amount of cards in the battle area. Forfeit?"))
                            {
                                WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!(PlayerType.human == prevTurn.GetPlayer().GetMode())) ? "computer" : "human")
                                        + " player (prev turn) sucessfully attacked/defended");
                                iLoserId = prevTurn.GetPlayer().ID;
                            }
                        }
                    }
                }
                m_Gg.UpdateElements(m_Players);//refresh gui with new placements
                DoneWithRound(iLoserId);//iLoserId used to set player order in the next round
                curRound.m_CurrentTurn--;
            }
            curRound.m_CurrentTurn++;
            curRound.outputStatusToWindow(ref m_Gg);
            curRound.updateBoldedStatus(curRound.GetCurrentTurn(), ref m_Gg);
        }
        /// <summary>
        /// Processes a player that has lost a round
        /// </summary>
        /// <param name="leftTurn"></param>
        /// <param name="leftHand"></param>
        /// <param name="playerId"></param>
        /// <param name="rightTurn"></param>
        /// <param name="rightHand"></param>
        /// <param name="bRightHuman"></param>
        /// <returns></returns>
        int ProcessLoser(Turn leftTurn, Hand leftHand, int playerId, Turn rightTurn, Hand rightHand, bool bRightHuman)
        {
            int iLoserId = 0;
            String tempMessage = ("Player " + playerId.ToString() + " " + ((!bRightHuman) ? "computer" : "human")
                                    + " player sucessfully attacked/defended");
            MessageBox.Show(tempMessage);//output information message
            WriteToLog(tempMessage);
            leftTurn.SetLost();//unused
            foreach (PlayingCard card in leftHand)//add loser's card to winner
            {
                //rightTurn.GetPlayer().Hand.Add(card);
                discardDeck.Add(card);
                leftTurn.GetPlayer().m_Hand.Remove(card);
            }
            foreach (PlayingCard card in rightHand)//add prev player's card to winner
            {
                rightTurn.GetPlayer().m_Hand.Add(card);
            }
            iLoserId = playerId;//loser actually means winner, and vice versa
            return iLoserId;
        }
        /// <summary>
        /// Delays responses so that player can see what is happening on the screen
        /// </summary>
        /// <param name="type"></param>
        public void Timed_Response(ResponseType type)//submits timer requests based on parameter
        {
            m_DispatcherTimer = new DispatcherTimer();

            if (ResponseType.clear_all == type)
            {
                m_DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m_DispatcherTimer.Tick += new EventHandler(m_Gg.dtDestroy_Tick);
            }
            else if (ResponseType.next_turn == type)
            {
                m_DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m_DispatcherTimer.Tick += new EventHandler(m_Gg.dtNext_Tick);
            }
            else if (ResponseType.next_turn_bypass == type)
            {
                m_DispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                m_DispatcherTimer.Tick += new EventHandler(m_Gg.dtBypass_Tick);
            }
            
            m_DispatcherTimer.Start();
        }
        /// <summary>
        /// Handles when a round is finished
        /// </summary>
        /// <param name="iLoserId"></param>
        public void DoneWithRound(int iLoserId)
        {
            Timed_Response(ResponseType.clear_all);

            //setup next round
            Round rnd = new Round();
            rnd.Initialize(this, m_Players, iLoserId);
            m_Rounds.Add(rnd);
        }
        /// <summary>
        /// Writes text to log
        /// </summary>
        /// <param name="text"></param>
        public static void WriteToLog(String text)
        {
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.Write(Environment.NewLine + text);
            }
        }
        /// <summary>
        /// Handles when a player forfeits their turn 
        /// </summary>
        public void ForfeitTurn()
        {
            
            Round curRound = m_Rounds[m_Rounds.Count - 1];
            Turn curTurn = curRound.GetCurrentTurn();
            Turn prevTurn = (Turn)curTurn.Clone();
            prevTurn--;
            bool bHuman = (PlayerType.human == prevTurn.GetPlayer().GetMode());
            WriteToLog("Player " + prevTurn.GetPlayer().ID.ToString() + " " + ((!bHuman) ? "computer" : "human")
                                    + " player (prev turn) sucessfully attacked/defended");
            DoneWithRound(prevTurn.GetPlayer().ID);
        }
        /// <summary>
        /// Checks to see if a hand is a valid attack hand
        /// </summary>
        /// <param name="attackHand"></param>
        /// <returns></returns>
        public bool IsValidAttackHand(PlayingCards attackHand)
        {
            bool isValid = false;
            if (attackHand.Count() > 1)
            {
                for (int index = 0; index < attackHand.Count(); index++)
                {
                    if (index != 0)
                    {
                        if (attackHand.ElementAt(index - 1).rank == attackHand.ElementAt(index).rank)
                        {
                            isValid = true;
                        }
                    }
                }
            }
            else
            {
                isValid = true;
            }
            return isValid;
        }
        /// <summary>
        /// Check to see if a hand is a valid defense hand
        /// </summary>
        /// <param name="defenseHand"></param>
        /// <param name="attackHand"></param>
        /// <returns></returns>
        public bool IsValidDefenseHand(PlayingCards defenseHand, PlayingCards attackHand)
        {
            bool isValid = false;
            //www.java2s.com/Tutorial/CSharp/0450__LINQ/Sortobjectbyitsproperty.htm
            //^^^^^^^^^^^ Found here
            defenseHand.Sort((c1,c2) => c1.suit.CompareTo(c2.suit));
            attackHand.Sort((c1, c2) => c1.suit.CompareTo(c2.suit));
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            if (attackHand.Count() == defenseHand.Count())
            {
                for (int index = 0; index < attackHand.Count(); index++)
                {
                    if (defenseHand.ElementAt(index).suit == attackHand.ElementAt(index).suit)
                    {
                        if (defenseHand.ElementAt(index).rank > attackHand.ElementAt(index).rank)
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                    else
                    {
                        isValid = false;
                    }
                }
            }
            return isValid;
        }
    }
}