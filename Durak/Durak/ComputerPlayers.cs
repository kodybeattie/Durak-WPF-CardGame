/*
 * Author      : Group01
 * filename    : ComputerPlayers.cs
 * Date        : 12-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This file is the representation of the computer players(if more than 1)
 */

using System;
using System.Collections.Generic;

namespace Durak
{
    class ComputerPlayers : List<ComputerPlayer>, ICloneable
    {
        public static int NumPlayers = 0;
        public ComputerPlayers(int numPlayers)
        {
            NumPlayers = numPlayers;
            if (!Initialize())
                throw new Exception();
        }
        public bool Initialize()
        {
            bool bRet = false;
            try
            {
                for (int i = 0; i < NumPlayers; i++)
                {
                    Add(new ComputerPlayer(i));
                }
                bRet = true;
            }
            catch (Exception ex)
            {

            }
            return bRet;
        }
        /// <summary>
        /// Required ICollectionBase method
        /// </summary>
        /// <param name="cards">Players</param>
        public void CopyTo(ComputerPlayers players)
        {
            for (int i = 0; i < this.Count; i++)
            {
                players[i] = this[i];
            }
        }
        /// <summary>
        /// Used to implement the iCloneable interface
        /// This is a shallow clone
        /// </summary>
        /// <returns>Object</returns>
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
