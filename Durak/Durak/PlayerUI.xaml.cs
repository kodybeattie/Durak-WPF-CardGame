
/*
 * Author      : Group01
 * filename    : PlayerUI.xaml.cs
 * Date        : 20-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the code behind the palyer user interface with different controls
 */

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CardLib;
namespace Durak
{
    /// <summary>
    /// Interaction logic for PlayerUI.xaml
    /// </summary>
    public partial class PlayerUI : UserControl
    {
        private const int POP = 25;
        private Orientation myOrientation;
        public PlayerUI(int playerNum, Orientation orientation = Orientation.Horizontal)
        {
            InitializeComponent();
            spPlayerHand.Orientation = orientation;
            myOrientation = orientation;
            if (orientation == Orientation.Vertical)
            {
                mainGrid.Height = 700;
                mainGrid.Width = 100;
            }
            lblPlayerNum.Content = "Player " + playerNum.ToString();
            
            BuildBoard();
        }
        public void BuildBoard()
        {

            spPlayerHand.Height = mainGrid.Height;
            RealignCards(spPlayerHand);
            //if (spPlayerHand.Children.Count > 12)
            //{
            //    newMargin -= ((Convert.ToDouble(spPlayerHand.Children.Count) / Convert.ToDouble(100)) * Convert.ToDouble(10)) * 2.5;
            //}
            //foreach (CardBox box in spPlayerHand.Children.OfType<CardBox>())
            //{
            //    box.Margin = new Thickness(newMargin);
            //}
        }

        public void AddCard(CardBox card)
        {
            spPlayerHand.Children.Add(card);
            RealignCards(spPlayerHand);
        }

        public void DeleteCard(CardBox card)
        {
            spPlayerHand.Children.Remove(card);
            RealignCards(spPlayerHand);
        }

        public int GetNumCards()
        {
            return (spPlayerHand.Children.Count);
        }

        /// <summary>
        /// Not sure about this...not quite complete
        /// may have to get rid of it entirely
        /// Suppose to up date the gui based on a players hand
        /// </summary>
        /// <param name="hand"></param>
        public void Update(int playerNum, PlayingCards hand)
        {
            spPlayerHand.Children.Clear();
            foreach (PlayingCard card in hand)
            {
                spPlayerHand.Children.Add(new CardBox(card, (Orientation)((0 == (int)myOrientation) ? 1 : 0)));
                RealignCards(spPlayerHand);
            }
            lblPlayerNum.Content = "Player " + playerNum.ToString();
        }

        private void RealignCards(StackPanel panelHand)
        {
            // Determine the number of cards/controls in the panel.
            int myCount = panelHand.Children.OfType<CardBox>().Count();
            // If there are any cards in the panel
            if (myCount > 0)
            {
                // Determine how wide one card/control is.
                int cardWidth = (int)panelHand.Children.OfType<CardBox>().ElementAt(0).RenderSize.Width;
                // Determine where the lefthand edge of a card/ control placed
                // in the middle of the panel should be
                int startPoint = ((int)panelHand.RenderSize.Width - cardWidth) / 2;
                // An offset for the remaining cards
                int offset = 0;
                // If there are more than one cards/controls in the panel
                if (myCount > 1)
                {
                    // Determine what the offset should be for each card based on the
                    // space available and the number of card/controls
                    offset = ((int)panelHand.RenderSize.Width - cardWidth / 2 * 17) / (myCount);
                    // If the offset is bigger than the card/control width, i.e. there is lots of room,
                    // set the offset to the card width. The cards/controls will not overlap at all.
                    if (offset > cardWidth)
                        offset = cardWidth;
                    // Determine width of all the cards/controls
                    int allCardsWidth = (myCount) * offset + cardWidth;
                    // Set the start point to where the lefthand edge of the "first" card should be.
                    startPoint = ((int)panelHand.RenderSize.Width - allCardsWidth) / 2;
                }
                // Aligning the cards: Note that I align them in reserve order from how they
                // are stored in the controls collection. This is so that cards on the left
                // appear underneath cards to the right. This allows the user to see the rank
                // and suit more easily.
                // Align the "first" card (which is the last control in the collection)
                panelHand.Children.OfType<CardBox>().ElementAt(myCount - 1).Margin = new Thickness(POP,0,0,0);
                System.Diagnostics.Debug.Write(panelHand.Children[myCount - 1].ToString() + "\n");
                //panelHand.Children.OfType<CardBox>().ElementAt(myCount - 1).Left = startPoint;
                // for each of the remaining controls, in reverse order.
                for (int index = myCount - 1; index >= 0; index--)
                {
                    // Align the current card
                    panelHand.Children.OfType<CardBox>().ElementAt(index).Margin = new Thickness(offset,0,0,0);
                    panelHand.Children.OfType<CardBox>().ElementAt(index).VerticalAlignment = VerticalAlignment.Bottom;
                    //panelHand.Children.OfType<CardBox>().ElementAt(index).Left = panelHand.Children[index + 1].Left + offset;
                }
            }
        }
    }//PlayerUI
}
