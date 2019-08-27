/*
 * Author      : Group01
 * filename    : CardBox.xaml.cs
 * Date        : 01-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the customized use control that is in charge of all the playing card 
 *               images properties that on the card box
 */

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using CardLib;


namespace Durak
{
    /// <summary>
    /// Interaction logic for CardBox.xaml
    /// </summary>
    public partial class CardBox : UserControl
    {
        //static CardBox()
        //{
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(CardBox), new FrameworkPropertyMetadata(typeof(CardBox)));
        //}
        /// <summary>
        /// A default constructor for a card object
        /// </summary>
        public CardBox()
        {
            InitializeComponent();
            myOrientation = Orientation.Vertical;
            myCard = null;
            imgCardDisplay.Source = GetCardImage();
        }

        /// <summary>
        /// Paramterized Constructor for a CardBox object
        /// </summary>
        /// <param name="card"> Card Object </param>
        /// <param name="orientation"></param>
        public CardBox(PlayingCard card, Orientation orientation = Orientation.Vertical, int iTurnId = 0)
        {
            InitializeComponent();
            myOrientation = orientation;
            myCard = card;
            imgCardDisplay.Source = GetCardImage();
            this.Tag = iTurnId;
        }

        private PlayingCard myCard;
        /// <summary>
        /// Gets and sets a Card object
        /// </summary>
        public PlayingCard Card
        {
            set
            {

                // Use image source converter
                myCard = value;
                // set the picture box image to the appropriate card image
                imgCardDisplay.Source = GetCardImage();
            }
            get
            {
                return myCard;
            }
        }

        /// <summary>
        /// Sets and gets whether the card is face up or not
        /// </summary>
        public bool FaceUp
        {
            set
            {
                // if the cards face up value does not match the one being set
                if (myCard.Faceup != value)
                {
                    // set the face up value
                    myCard.Faceup = value;
                    // update the cards image
                    UpdateCardImage();

                    // if the card flip event has been set
                    if (CardFlipped != null)
                    {
                        CardFlipped(this, new EventArgs());
                    }
                }
            }
            get
            {
                return Card.Faceup;
            }
        }

        /// <summary>
        /// Tracks the cards orientation
        /// </summary>
        private Orientation myOrientation;

        /// <summary>
        /// Sets and gets the cards orientation
        /// </summary>
        public Orientation CardOrientation
        {
            set
            {
                // if the cards orientation does not match the value being set
                if (myOrientation != value)
                {
                    // set the orientaion
                    myOrientation = value;
                    // adjust the height and the width
                    this.RenderSize = new Size(RenderSize.Height, RenderSize.Width);
                    // update the card image
                    UpdateCardImage();
                }
            }
            get
            {
                return myOrientation;
            }
        }

        /// <summary>
        /// Updates the cards image based on rank and suit
        /// </summary>
        public void UpdateCardImage()
        {
            imgCardDisplay.Source = GetCardImage();
            // if the card is horizontal set it to vertical
            //if (myOrientation == Orientation.Horizontal)
            //{
            //    RotateTransform rotate = new RotateTransform(90);
            //    mainGrid.RenderTransform = rotate;
            //    //imgCardDisplay.RenderTransform = rotate;

            //}
        }

        /// <summary>
        /// Prints the card box as a string based on rank and suit
        /// If the control is face down it will print a different
        /// message.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // Holds output
            String output = "";
            // If card is face up
            if (FaceUp)
            {
                // print card as string with rank and suit
                output = myCard.ToString();
            }
            else
            {
                // card is face down so print this
                output = "A face down card";
            }
            return output;
        }

        /// <summary>
        /// Handles when the control loads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBox_Load(object sender, EventArgs e)
        {
            // Update the cards image upon load
            UpdateCardImage();
        }

        public void biggerImage()
        {
            mainGrid.Height = mainGrid.Height + 17;
            mainGrid.Width = mainGrid.Width + 17;
            imgCardDisplay.Height = imgCardDisplay.Height + 17;
            imgCardDisplay.Width = imgCardDisplay.Width + 17;

        }

        public void smallerImage()
        {
            mainGrid.Height = mainGrid.Height - 17;
            mainGrid.Width = mainGrid.Width - 17;
            imgCardDisplay.Height = imgCardDisplay.Height - 17;
            imgCardDisplay.Width = imgCardDisplay.Width - 17;

        }
        /// <summary>
        /// Handles when a card is flipped
        /// </summary>
        public event EventHandler CardFlipped;

        /// <summary>
        /// Handesl a click event
        /// </summary>
        public event EventHandler CardBoxClick;

        void CardBox_MouseEnter(object sender, EventArgs e)
        {
            this.biggerImage();
        }

        void CardBox_MouseLeave(object sender, EventArgs e)
        {
            this.smallerImage();
        }

        /// <summary>
        /// Handles when the control is clicked on
        /// CardBoxClick="OnCardBoxClick"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CardBox_MouseLeftButtonDown(object sender, EventArgs e)
        {
            // if Eventhandler is set
            if (CardBoxClick != null)
            {
                CardBoxClick(this, e);
            }
        }
        public BitmapImage GetCardImage()
        {
            string imageName;
            imageName = myCard.rank.ToString().ToLower() + "_" + myCard.suit.ToString().ToLower() + ".png";
            BitmapImage bitimg = new BitmapImage();
            bitimg.BeginInit();
            bitimg.UriSource = new Uri(@"/Durak;component/images/" + imageName, UriKind.Relative);
            if (CardOrientation == Orientation.Horizontal)
            {
                mainGrid.Height = 56;
                mainGrid.Width = 82;
                bitimg.Rotation = Rotation.Rotate90;
            }
            bitimg.EndInit();
            return bitimg;
        }
    }//CardBox
}