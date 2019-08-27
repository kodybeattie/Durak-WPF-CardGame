// Project: CardLib
// Filename: Flags.cs
// Author: Ryan Beckett
// Date: Mar. 11, 2018
// Description: A file containing definitions for gameplay flags
namespace CardLib
{
    // to use the flags, start with a deck size: 19, 35, 51 (they are actually 0-based decks)
    // then add options, AceHigh is 64, UseTrumps is 128, 13 cards in a suit (as opposed to 9) is 4
    // if you chose to UseTrumps, then add a TrumpSuit (Diamonds=512, Hearts=1024, Spades=2048, Clubs=0)
    // for example, if I wanted 36 card deck, where aces are high, trumps are clubs:
    // 35+64+128=227
    // another example: if I wanted 36 card deck, aces high, UseTrumps, trumps are hearts, suit size is 13
    // 35+64+128+1024+4=1255
    // in the example above, you must specify 128 for UseTrumps and then 1024 for hearts
    // yet another example: if I want 19 card deck, aces not high, no trumps, suit size 9
    // 19
    public enum DeckFlags
    {   
        None = 0,
        BigSuit = 0x04,       
        Small = 0x13,//20 card deck (actually 19)
        Medium = 0x23,      //36 card deck (actually 35)
        Large = 0x33,      //52 card deck (actually 51)
        AceHigh =  0x40,
        UseTrump = 0x80,
        TrumpSuit = 0xFF
    }
}