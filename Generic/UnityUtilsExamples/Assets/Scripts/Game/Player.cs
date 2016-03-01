using Actions;
using System.Collections.Generic;
using System.Collections.ObjectModel;


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
            changed();
        }

        public Nation getObservingNation()
        {
            return observingNation;
        }

        public void setObservinNation(Nation nat)
        {
            observingNation = nat;
            changed();
        }

        public Player(List<string> data, int id) : base (data, id)
        {
            players.Add(this);
        }

        public override List<string> serialize()
        {
            List<string> diRitorno = base.serialize();
            diRitorno.Add(name);
            if (observingNation != null)
                diRitorno.Add(observingNation.getName());
            else
                diRitorno.Add("");
            return diRitorno;
        }

        public override void deserialize(List<string> data)
        {
            base.deserialize(data);
            name = data[0];
            data.RemoveAt(0);
            if (data[0].Length != 0)
                observingNation = Nation.getNation(data[0]);
            data.RemoveAt(0);
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
    }
}
