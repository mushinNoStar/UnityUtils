using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game
{
    public class Connection
    {
        public readonly ReadOnlyCollection<Sector> sectors;
        public readonly int connectionValue = 0;
        public readonly Galaxy galaxy;

        public Connection(Galaxy gal, Sector s1, Sector s2, int value = 0)
        {
            galaxy = gal;
            foreach (Connection cn in s1.getConnections())
            {
                if (cn.sectors[0] == s1 && cn.sectors[1] == s1)
                    throw new System.Exception("already existing connection");
                if (cn.sectors[1] == s1 && cn.sectors[0] == s1)
                    throw new System.Exception("already existing connection");
            }
            List<Sector> sec = new List<Sector>();
            sec.Add(s1);
            sec.Add(s2);
            sectors = sec.AsReadOnly();
            connectionValue = value;
        }
        
        public void checkConsistency()
        {
            foreach (Sector sc in sectors)
            {
                if (!sc.getConnections().Contains(this))
                    throw new System.Exception("a sector in a connection does not contain the connection");
            }
        }
    }
}