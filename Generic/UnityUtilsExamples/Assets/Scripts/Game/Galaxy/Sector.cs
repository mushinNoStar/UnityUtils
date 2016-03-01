using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using ceometric;
using Actions;

namespace Game
{
    public class Sector : Target, IVertex
    {
        private string name;
        private Vector2 position;
        public readonly Galaxy galaxy;
        private List<Sector> neighbours = new List<Sector>();
        private List<Connection> connections = new List<Connection>();
        private Nation owner = null;

        public Vector2 get2dPosition()
        {
            return position;
        }

        public Sector(List<string> data, int id) : base(data, id) { }

        public Sector (Galaxy gal, Vector2 pos, string nm) : base()
        {
            name = nm;
            position = pos;
            galaxy = gal;
        }

        public string getName()
        {
            return name;
        }

        public override List<string> serialize()
        {
            List<string> diRitorno = base.serialize();

            diRitorno.Add(name);
            diRitorno.Add(position.x+"");
            diRitorno.Add(position.y+"");
            if (owner != null)
                diRitorno.Add(owner.getName());
            else
                diRitorno.Add("");

            return diRitorno;
        }

        public override void deserialize(List<string> data)
        {
            base.deserialize(data);
            name = data[0];
            data.RemoveAt(0);
            position = new Vector2(float.Parse(data[0]), float.Parse(data[1]));
            data.RemoveAt(0);
            data.RemoveAt(0);
            if (data[0].Length != 0)
                owner = Nation.getNation(data[0]);
            else
                owner = null;
            data.RemoveAt(0);
        }

        public void addNeighbour(Sector sc)
        {
            neighbours.Add(sc);
        }

        public ReadOnlyCollection<Sector> getNeighbours()
        {
            return neighbours.AsReadOnly();
        }

        public void addConnection(Connection c)
        {
            connections.Add(c);
        }

        public ReadOnlyCollection<Connection> getConnections()
        {
            return connections.AsReadOnly();
        }

        public void setOwner(Nation nt)
        {

            if (owner != null)
            {
                Nation nr = owner;
                owner = null;
                nr.removeSector(this);
            }
            owner = nt;
            changed();
        }

        public Nation getOwner()
        {
            return owner;
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