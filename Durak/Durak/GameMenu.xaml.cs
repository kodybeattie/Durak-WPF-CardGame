/*
 * Author      : Group01
 * filename    : GameMenu.xaml.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the code behind the game menu gui and have some properties for different 
 *               buttons
 */


using System;
using System.Windows;
using System.Windows.Controls;
using CardLib;
using GetRandom;

namespace Durak
{
    /// <summary>
    /// Interaction logic for GameMenu.xaml
    /// </summary>
    public partial class GameMenu : Window
    {
        /// <summary>
        /// The difficulty the player would like to play on
        /// </summary>
        private int trumpsuit = -1;
        /// <summary>
        /// The number of players the player would like to play with
        /// </summary>
        private int numPlayers;
        /// <summary>
        /// The number of cards the player would like to play with
        /// </summary>
        private int deckSize;
        private bool gameStarted = false;
        public int Result = 0;

        /// <summary>
        /// 
        /// </summary>
        public bool GameStarted
        {
            get
            {
                return gameStarted;
            }
        }

        /// <summary>
        /// Gets the difficulty the user has selected from the
        /// form.
        /// </summary>
        public int TrumpSuit
        {
            set
            {
                trumpsuit = value;
            }
            get
            {
                return trumpsuit;
            }
        }

        /// <summary>
        /// Gets the number of players the user has selected on the form
        /// </summary>
        public int NumPlayers
        {
            set
            {
                numPlayers = value;
            }
            get
            {
                return numPlayers;
            }
        }

        /// <summary>
        /// Gets the deck size the user has selected from the form
        /// </summary>
        public int DeckSize
        {
            set
            {
                deckSize = value;
            }
            get
            {
                return deckSize;
            }

        }
        /// <summary>
        /// Resets the form to default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ResetForm()
        {
            rbnRandom.IsChecked = true;
            rbnPlayers2.IsChecked = true;
            rbnSize36.IsChecked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton rbTemp in spTrumps.Children)
            {
                if (rbTemp != null && rbTemp.IsChecked == true)
                {
                    TrumpSuit = Int32.Parse(rbTemp.Tag.ToString());
                }
            }
            foreach (RadioButton rbTemp in spDeckSize.Children)
            {
                if (rbTemp != null && rbTemp.IsChecked == true)
                {
                    DeckSize = (Int32.Parse(rbTemp.Content.ToString()));
                }
            }
            foreach (RadioButton rbTemp in spNumPlayers.Children)
            {
                if (rbTemp != null && rbTemp.IsChecked == true)
                {
                    NumPlayers = (Int32.Parse(rbTemp.Content.ToString()));
                }
            }
            if (TrumpSuit == 4)
            {
                //if trump suit not specified, randomly generate one
                TrumpSuit = (int)RangedRandom.GenerateUnsignedNumber(4, 0);
            }
            GameGui.numPlayers = NumPlayers;
            Result = --DeckSize + (int)DeckFlags.AceHigh + (int)DeckFlags.UseTrump + (TrumpSuit << 9);
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowMenu()
        {
            this.Show();
        }
        public GameMenu()
        {
            InitializeComponent();
            ResetForm();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers6_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = false;
            rbnSize20.IsChecked = false;
            rbnSize36.IsChecked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers5_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = false;
            rbnSize20.IsChecked = false;
            rbnSize36.IsChecked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers4_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = false;
            rbnSize20.IsChecked = false;
            rbnSize36.IsChecked = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers3_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbnPlayers2_Checked(object sender, RoutedEventArgs e)
        {
            rbnSize20.IsEnabled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@".\help\oop_project_group_1.chm");
        }
    }
}
