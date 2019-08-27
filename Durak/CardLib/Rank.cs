// Project: CardLib
// Filename: Rank.cs
// Author: Ryan Beckett
// Date: Feb. 2, 2018
// Description: A file containing definitions for playing card ranks

// With inspiration from Textbook:
/**
 * Watson et al., Karli. ( © 2013). Beginning visual c# 2012 programming. [Books24x7 version] 
 * Available from http://common.books24x7.com.dproxy.library.dc-uoit.ca/toc.aspx?bookid=51079. 
 * 
 * */
namespace CardLib
{
    public enum Rank
    {
        Ace = 1, // shouldn't ace be at the bottom??
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King
    }
}