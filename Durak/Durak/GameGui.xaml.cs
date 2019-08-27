/*
 * Author      : Group01
 * filename    : GameGui.xaml.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the code behind the actual game gui and have some properties for different 
 *               button and other controls
 *
 * Gui Background - https://www.acepokersolutions.com/bovada-poker-mods/bovada-mods/APS-t1.jpg
 SVG-cards
=========

A set of playing cards in SVG, now also with a rendering in PNG and
installable via NPM.

*   Version: 3.0.3
*   License: LGPL-2.1
*   Install via NPM: `npm install --save svg-cards` or just download the
    [SVG](https://raw.githubusercontent.com/htdebeer/SVG-cards/master/svg-cards.svg)
    file.

![Example use of SVG
Cards](https://raw.githubusercontent.com/htdebeer/SVG-cards/master/example_use.png)

This is a fork of [SVG-cards 2.0.1](http://svg-cards.sourceforge.net/), which
was created by [David Bellot](http://david.bellot.free.fr/). He writes in the
README of the original package:

> This is a set of playing cards made in pure SVG with all kings, queens,
> jacks, numbers, jokers and backs of cards. This set of SVG files is intended
> to be used in games, figures, illustrations, web sites as long as you
> provide the code source and the LGPL license (see the COPYING file).
> Although this is a free software, the license is the LGPL so you can use
> this set of cards even in a non-free software.
>
> The kings, queens and jacks are based on the french representation, because
> I find them beautiful. You can access to each either by rendering the file
> into a pixmap and clipping each card or by using their name with a DOM
> interface.

I agree with David that these cards are beautiful! I am grateful for his work
and that he published them under an open source licence. However, while playing
around with the SVG file containing the cards, I found that using the cards in
SVG was not as straightforward as I would have liked. For example, I expected
the following line,

    <use xlink:href="svg-cards.svg#red_joker" x=40" y="12" />

to put the red joker card with its top-left corner at (40, 12). It does not,
as it takes into account the place of the red joker in the SVG file: the card
top-left corner shows up at (207.575, 750.55).

In this fork I translated all cards to set their top-left corner at the origin
(0,0). After this change, the above USE element works as expected: it places
the red joker card's top-left corner at (40, 12).

For transforming the cards after using them, it is good to know that the
cards have the following natural dimensions:

- width: 169.075
- height: 244.64
- center: (+98.0375, +122.320)

Furthermore, to enable to create different stocks of cards, I set the color on
the back card to `inherit` instead of `#0062ff`. The color of the back card can
be changed by setting the fill on the USE-element. For example:

    <use xlink:href="svg-cards-indented.svg#back" x="150" y="10" fill="red"/>

The use of the cards are demonstrated in the
[`example_use.svg`](https://raw.githubusercontent.com/htdebeer/SVG-cards/master/example_use.svg) file.

The naming of the cards is kept as in the original (citing the original
README):

> Names are the following :
>
> black_joker red_joker back {king,queen,jack}_{club,diamond,heart,spade}
> {1,2,3,4,5,6,7,8,9,10}_{club,diamond,heart,spade}
>
> Examples :
> - the ace of club is 1_club
> - the queen of diamond is queen_diamond
>
> and so on...

When developing applications with these cards, I had also need of the card
circumference and the four suits. To that end I introduced the following
shapes you can USE:

- *card-base*, with the same dimensions as a card
- *suit-club*
- *suit-heart*
- *suit-diamond*
- *suit-spade*

The suits have the following dimensions:

- width: 15.42
- height: 15.88
- center: (7.71, 7.94)

A while later I also discovered that the back card is too complex to be
rendered swiftly in my web browsers. When rendering a deck of 52 cards facing
down, it took almost 2 seconds to render about 50000 elements in the DOM. To
overcome these issues, I created an alternative back by removing all the
frills from the original
one. You can USE is via:

- *alternate-back*

I have also added a nicely formatted SVG file, `svg-cards-indented.svg`, which
makes the SVG file easier to inspect using a text editor. Converting from
indented to unindented version goes via
[xmllint](http://xmlsoft.org/xmllint.html):  `xmllint --noblanks
svg-cards-indented.svg > svg-cards.svg`

To automatically convert these SVG files to PNG I developed a separate
project: [svg-cards-to-png](https://github.com/htdebeer/svg-cards-to-png). For
convenience, PNG files are included in this repository in the `png`
subdirectory. There are two directories, `png/1x` and `png/2x`, with PNG
files of the SVG cards with, respectively, their natural dimensions and twice
their natural dimensions. Furthermore, 16 different colored back cards are
included as well.

[desphilboy](https://github.com/desphilboy) has made this project into a
[NPM](https://www.npmjs.com/) package to make it easier for web developers to
use it in their projects. Now it can be installed and used like other
dependencies: `install --save svg-cards`.
*/

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CardLib;
using GetRandom;


namespace Durak
{
    
    /// <summary>
    /// Interaction logic for GameGui.xaml
    /// </summary>
    public partial class GameGui : Window
    {
        /* 
         * A list of all players - easier to use list as we dont care where
         *                          the index of the player is. We just want
         *                          to tack it onto to the end.
         */
        public static int numPlayers = 0;//during gameply this number should change, but it doesn't
        protected PlayerUIs UIs = new PlayerUIs();
        private int m_nGameFlags = 0;
        private Game m_Game = null;
        public PlayArea m_BattleArea = null;
        PlayerUI m_PersonPlayerUI = null;

        /// <summary>
        /// Creates a game gui with a human player and a dynamix number
        /// of computer players int numPlayers, int trumpsuit, int deckSize
        /// </summary>
        /// <param name="numPlayers"></param>
        public GameGui()
        {
            InitializeComponent();
            RangedRandom.PrimeRandomNumberGenerator();
            WindowState = WindowState.Maximized;
            Initialize();
            Round temp = new Round();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            m_Game = null;
            GameMenu gm = new GameMenu();
            gm.ShowDialog();
            m_nGameFlags = gm.Result;
            RangedRandom.PrimeRandomNumberGenerator();
            m_Game = new Game(numPlayers, m_nGameFlags, this);
        }

        /// <summary>
        /// Sets up the basic components of the gui
        /// </summary>
        public void SetupGui()
        { 
            m_BattleArea = new PlayArea();
            Grid.SetRow(m_BattleArea, 1);
            Grid.SetColumn(m_BattleArea, 1);
            Grid.SetRowSpan(m_BattleArea, 5);
            Grid.SetColumnSpan(m_BattleArea, 8);
            mainGrid.Children.Add(m_BattleArea);
            ShowTrump();
            ShowDeck(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerNum"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        /// <param name="col"></param>
        /// <param name="colSpan"></param>
        /// <param name="cardOrientation"></param>
        /// <param name="handOrientation"></param>
        /// <param name="faceUp"></param>
        private void BuildComputerPlayer(int playerNum, int row, int rowSpan, int col,
            int colSpan, Orientation cardOrientation, Orientation handOrientation = Orientation.Horizontal,
            bool faceUp = false)
        {
            PlayerUI compPlayer = new PlayerUI(playerNum, handOrientation);
            UIs.Add(compPlayer);
            Grid.SetRow(compPlayer, row);
            Grid.SetColumn(compPlayer, col);
            Grid.SetRowSpan(compPlayer, rowSpan);
            Grid.SetColumnSpan(compPlayer, colSpan);
            mainGrid.Children.Add(compPlayer);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="players"></param>
        public void UpdateDisplay(Players players)
        {
            if (UIs.Count == 0)
            {
                m_PersonPlayerUI = new PlayerUI(0);
                Grid.SetRow(m_PersonPlayerUI, 6);
                Grid.SetColumn(m_PersonPlayerUI, 4);
                Grid.SetRowSpan(m_PersonPlayerUI, 2);
                Grid.SetColumnSpan(m_PersonPlayerUI, 3);
                mainGrid.Children.Add(m_PersonPlayerUI);
                UIs.Add(m_PersonPlayerUI);
                if (players.Count == 2)
                {
                    BuildComputerPlayer(1, 0, 2, 4, 3, Orientation.Vertical);
                }
                else if (players.Count == 3)
                {
                    BuildComputerPlayer(1, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(2, 0, 2, 6, 3, Orientation.Vertical);
                }
                else if (players.Count == 4)
                {
                    BuildComputerPlayer(1, 2, 5, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(2, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(3, 0, 2, 6, 3, Orientation.Vertical);
                }
                else if (players.Count == 5)
                {
                    BuildComputerPlayer(1, 4, 4, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(2, 0, 4, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(3, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(4, 0, 2, 6, 3, Orientation.Vertical);

                }
                else if (players.Count == 6)
                {
                    BuildComputerPlayer(1, 5, 3, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(2, 1, 3, 0, 2, Orientation.Horizontal, Orientation.Vertical);
                    BuildComputerPlayer(3, 0, 2, 2, 3, Orientation.Vertical);
                    BuildComputerPlayer(4, 0, 2, 6, 3, Orientation.Vertical);
                    BuildComputerPlayer(5, 2, 3, 8, 2, Orientation.Horizontal, Orientation.Vertical);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="players"></param>
        public void UpdateElements(Players players)
        { 
            for (int i = 0; i < players.Count; i++)
            {
                UIs.ElementAt(i).Update(i, players[i].m_Hand);
            }
            UpdateEventHandlers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UI"></param>
        /// <param name="row"></param>
        /// <param name="rowSpan"></param>
        /// <param name="col"></param>
        /// <param name="colSpan"></param>
        private void SendUIToScreen(PlayerUI UI, int row, int rowSpan, int col, int colSpan)
        {
            Grid.SetRow(UI, row);
            Grid.SetColumn(UI, col);
            Grid.SetRowSpan(UI, rowSpan);
            Grid.SetColumnSpan(UI, colSpan);
            mainGrid.Children.Add(UI);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateEventHandlers()
        {
            foreach (PlayerUI gui in UIs)
            {
                foreach (CardBox box in gui.spPlayerHand.Children)
                {
                    box.CardBoxClick += OnCustomButtonClick;
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCustomButtonClick(object sender, EventArgs e)
        {
            m_Game.CardBoxClicked(sender);
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowTrump()
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"/Durak;component/images/suit-" + Util.SuitToStr(PlayingCard.trump).ToLower() + ".png", UriKind.Relative);
            bitimg.EndInit();
            imgTrumpDisplay.Source = bitimg;
        }
        /// <summary>
        /// Makes the deck visible or not.
        /// </summary>
        /// <param name="flipped"></param>
        private void ShowDeck(bool flipped)
        {
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            if (flipped)
                bitimg.UriSource = new Uri(@"/Durak;component/images/back-navy.png", UriKind.Relative);
            else
                bitimg.UriSource = new Uri(@"/Durak;component/images/back-blank.png", UriKind.Relative);
            bitimg.EndInit();
            imgDeck.Source = bitimg;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnForfeit_Click(object sender, RoutedEventArgs e)
        {
            if (GameUtil.ShallIDoThisForYou("Do you wish to forfeit your turn?"))
            {
                m_Game.ForfeitTurn();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            m_Game.DoneClicked(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dtDestroy_Tick(object sender, EventArgs e)
        {
            m_BattleArea.Destroy();
            ((DispatcherTimer)sender).Stop();
            Players players = Game.m_Players;
            if (!Game.m_bDeckOutFlag)
            {
                players.DealHands(players.Count, m_Game.m_HandSize, ref m_Game.m_Deck);
                if (Game.m_bDeckOutFlag) 
                {
                    MessageBox.Show("Deck has been emptied");
                    ShowDeck(false);
                }
            }
            UpdateElements(players);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dtNext_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            Players players = Game.m_Players;
            UpdateElements(players);
            m_Game.DoneClicked(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void dtBypass_Tick(object sender, EventArgs e)
        {
            ((DispatcherTimer)sender).Stop();
            Players players = Game.m_Players;
            UpdateElements(players);
            m_Game.DoneClicked(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Deck_LastCardDrawn(object sender, EventArgs e)
        {
            Game.m_bDeckOutFlag = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Deck_CardDealt(object sender, EventArgs e)
        {
            gbxDeck.Header = "Deck: " + ((Deck)sender).DeckCount + " cards";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Help_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@".\help\oop_project_group_1.chm");
        }
    }
}
