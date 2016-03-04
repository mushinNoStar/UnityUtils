using Actions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Game
{
    /// <summary>
    /// there should be one player foreach user or ia.
    /// players are used to determin how to show and to use action.
    /// </summary>
    public sealed class Player : Actor
    {
        private string name;
        private static List<Player> players = new List<Player>();
        private Nation observingNation = null;

        public Player(string nm) : base()
        {
            name = nm;
            players.Add(this);
        }

        public Nation getObservingNation()
        {
            return observingNation;
        }

        public void setObservinNation(Nation nat)
        {
            observingNation = nat;
        }

        public string getName()
        {
            return name;
        }

        public static Player getPlayer(string name)
        {
            foreach (Player pl in players)
                if (pl.getName() == name)
                    return pl;
            return null;
        }
        
        public static ReadOnlyCollection<Player> getPlayers()
        {
            return players.AsReadOnly();
        }

        [Serializable]
        public class SerializablePlayer
        {
            public string name;
            public int nationID = -1;

            public SerializablePlayer()
            {}

            public SerializablePlayer(Player pl)
            {
                name = pl.name;
                if (pl.observingNation != null)
                    nationID = Nation.getNations().IndexOf(pl.getObservingNation());
                else
                    nationID = -1;
            }

            public void setUpPlayer(Player pl)
            {
                if (nationID != -1)
                    pl.observingNation = Nation.getNations()[nationID];
            }
        }
    }
}
