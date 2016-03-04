using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using ceometric;
using Actions;
using ceometric.DelaunayTriangulator;

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
        private List<SubSector> subSectors = new List<SubSector>();
        private List<Connection> interalConnection = new List<Connection>();

        public Vector2 get2dPosition()
        {
            return position;
        }

        public List<Connection> getInternalConnections()
        {
            return interalConnection;
        }

        public Sector (Galaxy gal, Vector2 pos, string nm, bool spawnSubSector) : base()
        {
            name = nm;
            position = pos;
            galaxy = gal;
            galaxy.addSector(this);

            int subSectorCount = galaxy.generationParam.averageNumberOfStarInSector;
            subSectorCount += (int)(galaxy.generationParam.plusNumberOfStarInSector * UnityEngine.Random.value);

            if (!spawnSubSector)
                return;
            for (int a = 0; a < subSectorCount; a++)
            {
                Vector2 loc = pos + new Vector2(UnityEngine.Random.value * gal.generationParam.subSectorPos, UnityEngine.Random.value * gal.generationParam.subSectorPos);
                loc = loc - new Vector2(gal.generationParam.subSectorPos/2f, gal.generationParam.subSectorPos / 2f);
                new SubSector(gal, this, Tools.Utils.randomName(), loc);
            }
            
            List<Point> points = new List<Point>();
            foreach (SubSector sbs in subSectors)
                points.Add(new Point(sbs.getPosition().x, sbs.getPosition().y, 0));

            List<Triangle> tris = galaxy.triangulate(points);

            List<int> extremes = Tools.Utils.transformInToListOfUniqueVectors(points, tris); //remove double connection, return list of pairs of indexes to sectors
            for (int a = 0; a < extremes.Count; a += 2)
            {
                if (UnityEngine.Random.value < 0.4f)
                    continue;
                SubSector sc = subSectors[extremes[a]];
                SubSector sc2 = subSectors[extremes[a + 1]];
                new Connection(gal, sc, sc2);
            }

            foreach (SubSector sb in subSectors)
            {
                int t = 0;
                if (subSectors.IndexOf(sb) == 0)
                    t = 1;
                if (sb.getNeighbours().Count == 0)
                    new Connection(gal, sb, subSectors[t]);
            }
        }

        public void tick()
        {
            foreach (SubSector sbs in subSectors)
            {
                sbs.tick();
            }
        }

        public void addInternalConnection(Connection cn)
        {
            if (cn.sectors.Count > 1 || cn.sectors[0] != this)
                throw new System.Exception("this is not a internal connection");
            interalConnection.Add(cn);
        }

        public List<SubSector> getSubSectors()
        {
            return subSectors;
        }

        public SubSector getNearestSubSector(Vector2 pos)
        {
            SubSector nearsest = subSectors[0];
            float lastEvaluated = 10000;
            for (int a = 0; a < subSectors.Count; a++)
            {
                if (Vector2.Distance(pos, subSectors[a].getPosition()) < lastEvaluated)
                {
                    lastEvaluated = Vector2.Distance(pos, subSectors[a].getPosition());
                    nearsest = subSectors[a];
                }
            }
            return nearsest;
        }

        public SubSector randomSubSector()
        {
           return Tools.Utils.randomFromList<SubSector>(subSectors);
        }

        public string getName()
        {
            return name;
        }

        public void addSubSector(SubSector sbs)
        {
            subSectors.Add(sbs);
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

            if (c.sectors[0] == this)
                addNeighbour(c.sectors[1]);
            else
                addNeighbour(c.sectors[0]);
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

            foreach (Connection cn in interalConnection)
                cn.checkConsistency();

            foreach (Connection cn in interalConnection)
                if (!cn.internalConnection)
                    throw new Exception("internal connection is not internal");
            foreach (Connection cn in connections)
                if (cn.internalConnection)
                    throw new Exception("not internal connection is internal");
            foreach (Connection cn in connections)
                if (cn.sectors.Count == 1)
                    throw new Exception("not internal connection is internal");

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
            foreach (SubSector sbs in subSectors)
            {
                if (sbs.sector != this)
                    throw new Exception("a sub sector is and is not in a sector");
            }
            foreach (SubSector sbs in subSectors)
                sbs.checkConsistency();
        }

        [Serializable]
        public class SerializableSector
        {
            public float[] pos;
            public string name = "";
            public List<int> neighbours = new List<int>();
            public List<SubSector.SubSectorSerializable> subSectors = new List<SubSector.SubSectorSerializable>();
            public List<Connection.SerializableConnection> internalConnections = new List<Connection.SerializableConnection>();

            public SerializableSector()
            { }

            public SerializableSector(Sector sc)
            {
                name = sc.name;
                pos = new float[] {sc.position.x, sc.position.y};
                foreach (Sector s in sc.neighbours)
                    neighbours.Add(sc.galaxy.getSectors().IndexOf(s));
                foreach (SubSector subSect in sc.subSectors)
                    subSectors.Add(new SubSector.SubSectorSerializable(subSect));
                foreach (Connection cn in sc.getInternalConnections())
                    internalConnections.Add(new Connection.SerializableConnection(cn));
            }

            public void setUpSector(Sector sc)
            {
                foreach (SubSector.SubSectorSerializable sb in subSectors)
                {
                    SubSector sbs = new SubSector(sc.galaxy, sc, sb.name,new Vector2(sb.position[0], sb.position[1]));
                    sb.setUpSubSector(sbs);
                }
                foreach (Connection.SerializableConnection con in internalConnections)
                {
                    SubSector sb1 = sc.galaxy.getSectors()[con.sector[0]].getSubSectors()[con.sectorsConnected[0]];
                    SubSector sb2 = sc.galaxy.getSectors()[con.sector[1]].getSubSectors()[con.sectorsConnected[1]];
                    Connection c = new Connection(sc.galaxy, sb1, sb2);
                    con.setUpConnection(c);
                }
            }
        }
    }
}