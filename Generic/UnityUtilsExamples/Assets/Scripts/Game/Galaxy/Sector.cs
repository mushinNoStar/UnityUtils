using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using ceometric;

namespace Game
{
    public class Sector : IVertex
    {
        public readonly string name;
        public readonly Vector2 position;
        public readonly Galaxy galaxy;
        private List<Sector> neighbours = new List<Sector>();
        private List<Connection> connections = new List<Connection>();

        public Vector2 get2dPosition()
        {
            return position;
        }

        public Sector (Galaxy gal, Vector2 pos, string nm)
        {
            name = nm;
            position = pos;
            galaxy = gal;
        }
        
        public void setNeighbours(List<Sector> sectors)
        {
            neighbours = sectors;
        }

        public void setConnection(List<Connection> connection)
        {
            connections = connection;
        }

        public ReadOnlyCollection<Connection> getConnections()
        {
            return connections.AsReadOnly();
        }

        public void checkConsistency()
        {
            if (!galaxy.getSectors().Contains(this))
                throw new Exception("sector is not in galaxy");
            foreach (Sector sc in neighbours)
                if (!sc.neighbours.Contains(this))
                    throw new Exception("near sector has not this sector as near");
            foreach (Connection con in connections)
            {
                if (!con.sectors.Contains(this))
                    throw new Exception("connection is sector is not connected");
                foreach (Connection r in connections)
                {
                    if (r.sectors[0] == con.sectors[0] && r.sectors[1] == con.sectors[1] && r != con)
                        throw new Exception("multiple connections between sectors");
                    if (r.sectors[1] == con.sectors[0] && r.sectors[0] == con.sectors[1] && r != con)
                        throw new Exception("multiple connections between sectors");
                }
            }
            //foreach (Planet pl in planets)
              //  pl.checkConsistency();
        }
    }
}