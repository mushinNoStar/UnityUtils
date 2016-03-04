using UnityEngine;
using System.Collections;
using Actions;
using System;
using System.Collections.Generic;
using ceometric;

namespace Game
{
    public sealed class SubSector : Target, IVertex
    {
        public readonly Galaxy galaxy;
        public readonly Sector sector;
        private string name;
        private Vector2 position;
        private List<Connection> connections = new List<Connection>();
        private List<SubSector> neighbours = new List<SubSector>();

        private float dijastraDistance;
        private bool dijastraVisited;
        private SubSector dijastraPrevious;

        private List<Fleet> currentMovables = new List<Fleet>();

        public SubSector(Galaxy gal, Sector sc, string nm, Vector2 pos) : base()
        {
            galaxy = gal;
            sector = sc;

            gal.everySubSector.Add(this);
            sc.addSubSector(this);

            name = nm;
            position = pos;
        }

        public void removeMovable(Movable movable)
        {
            if (movable is Fleet)
                currentMovables.Remove(((Fleet)movable));
        }

        public List<Fleet> getFleet()
        {
            return currentMovables;
        }

        public void addMovable(Movable movable)
        {
            if (movable is Fleet)
                currentMovables.Add((Fleet)movable);
        }

        public List<Connection> getConnections()
        {
            return connections;
        }

        public string getName()
        {
            return name;
        }

        public void tick()
        {
            
        }

        public void addConnection(Connection con)
        {
            connections.Add(con);
            if (con.subSectors[0] == this)
                neighbours.Add(con.subSectors[1]);
            else
                neighbours.Add(con.subSectors[0]);
        }

        public Vector2 getPosition()
        {
            return position;
        }


        public List<SubSector> getNeighbours()
        {
            return neighbours;
        }

        public void checkConsistency()
        {
            foreach (Connection cn in connections)
                if (cn.subSectors[0] != this && cn.subSectors[1] != this)
                    throw new Exception("connection is not a connection");
            foreach (SubSector neighbour in neighbours)
                if (!neighbour.neighbours.Contains(this))
                    throw new Exception("sub sector is and is not a neighbour");
        }

        public static List<SubSector> getPathing(SubSector from, SubSector destination, Galaxy gal, int Conlvl)
        {
            initializeDijastraSearch(gal, from);
            List<SubSector> Q = new List<SubSector>();
            foreach (SubSector sbs in gal.everySubSector)
                Q.Add(sbs);

            SubSector current = from;
            Q.Remove(current);

            while (current != destination)
            {
                foreach (Connection v in current.connections)
                {
                    if (v.getConnectionLevel() > Conlvl)
                        continue;

                    SubSector sbs = null;

                    if (v.subSectors[0] == current)
                        sbs = v.subSectors[1];
                    else
                        sbs = v.subSectors[0];
                    
                    if (!sbs.dijastraVisited)
                        relaxDijastra(current, sbs);
                }  
                current = extractMinDijastra(Q, current);
                Q.Remove(current);
            }

            List<SubSector> diRitorno = new List<SubSector>();
            while (current != from && current.dijastraPrevious != null)
            {
                diRitorno.Add(current);
                current = current.dijastraPrevious;
            }
            //diRitorno.Add(current);
            diRitorno.Reverse();
            return diRitorno;
        }

        private static SubSector extractMinDijastra(List<SubSector> Q, SubSector current)
        {
            SubSector s = Q[0];

            foreach (SubSector sbs in Q)
            {
                if (current.dijastraDistance + 1 == sbs.dijastraDistance)
                    return sbs;
                if (s.dijastraDistance > sbs.dijastraDistance)
                    s = sbs;
            }
            return s;
        }

        private static void initializeDijastraSearch(Galaxy gal, SubSector start)
        {
            foreach (SubSector sbs in gal.everySubSector)
            {
                sbs.dijastraDistance = 1000000;
                sbs.dijastraPrevious = null;
                sbs.dijastraVisited = false;
            }
            start.dijastraDistance = 0;
            start.dijastraVisited = true;
        }

        private static void relaxDijastra(SubSector u, SubSector v)
        {
            if (v.dijastraDistance > u.dijastraDistance + 1)
            {
                v.dijastraDistance = u.dijastraDistance + 1;
                v.dijastraPrevious = u;
                v.dijastraVisited = true;
            }
        }

        public Vector2 get2dPosition()
        {
            return getPosition();
        }

        [Serializable]
        public class SubSectorSerializable
        {
            public string name;
            public float[] position;

            public SubSectorSerializable() { }

            public SubSectorSerializable(SubSector sect)
            {
                name = sect.getName();
                position = new float[] { sect.getPosition().x, sect.getPosition().y };
            }

            public void setUpSubSector(SubSector sect)
            {
                
            }
        }
    }
}