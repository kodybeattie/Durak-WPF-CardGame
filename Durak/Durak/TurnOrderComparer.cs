/*
 * Author      : Group01
 * filename    : TurnOrderComparer.cs
 * Date        : 08-Mar-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This file implements the IComparer interface that is used to compare the cards.
 */


using System.Collections.Generic;

namespace Durak
{
    public class TurnOrderComparer : IComparer<Turn>
    {
        public int Compare(Turn prev, Turn cur)
        {
            return prev.CompareTo(cur);
        }
    }
}