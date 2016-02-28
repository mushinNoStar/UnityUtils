using Actions;
using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace Game
{
    public class Player : Actor
    {
        public readonly string name;
        private static List<Player> players = new List<Player>();

        public Player(string nm) : base()
        {
            name = nm;
            players.Add(this);
        }

        public ReadOnlyCollection<Player> getPlayer()
        {
            return players.AsReadOnly();
        }


    }
}
