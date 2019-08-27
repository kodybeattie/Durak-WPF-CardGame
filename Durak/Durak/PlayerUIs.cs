/*
 * Author      : Group01
 * filename    : PlayerUIs.cs
 * Date        : 06-Apr-2018
 * Purpose     : This file is created as the requirement for the final project for OOP-4200.
 * Description : This is the player user interface class
 */

using System;
using System.Collections.Generic;


namespace Durak
{
    public class PlayerUIs : List<PlayerUI>, ICloneable
    {
        public static int NumPlayers = 0;
        public PlayerUIs()
        {
        }

        public PlayerUIs(int numPlayers)
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
                    Add(new PlayerUI(i));
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
        /// <param name="cards">PlayersUIs/param>
        public void CopyTo(PlayerUIs UIs)
        {
            for (int i = 0; i < this.Count; i++)
            {
                UIs[i] = this[i];
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
