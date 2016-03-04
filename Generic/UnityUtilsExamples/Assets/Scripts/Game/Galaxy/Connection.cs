using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;

namespace Game
{
    public class Connection
    {
        public readonly ReadOnlyCollection<Sector> sectors;
        private int connectionValue = 0;
        public readonly Galaxy galaxy;
        public readonly bool internalConnection = false;
        public readonly List<SubSector> subSectors = new List<SubSector>();

        public Connection(Galaxy gal, SubSector s1, SubSector s2)
        {
            galaxy = gal;
            if (s1 == null)
                throw new Exception(" null system");
            if (s2 == null)
                throw new Exception(" null system");
            if (s1 == s2)
                throw new Exception(" Cyclylca connection");

            subSectors.Add(s1);
            subSectors.Add(s2);
            s1.addConnection(this);
            s2.addConnection(this);

            List<Sector> sec = new List<Sector>();
            if (s1.sector != s2.sector)
            {
                foreach (Connection cn in s1.sector.getConnections())
                {
                    if (cn.sectors[0] == s1.sector && cn.sectors[1] == s1.sector)
                        throw new System.Exception("already existing connection");
                    if (cn.sectors[1] == s1.sector && cn.sectors[0] == s1.sector)
                        throw new System.Exception("already existing connection");
                }
                sec.Add(s1.sector);
                sec.Add(s2.sector);
                sectors = sec.AsReadOnly();


                s1.sector.addConnection(this);
                s2.sector.addConnection(this);

                gal.addConnection(this);
                internalConnection = false;


            }
            else
            {
                internalConnection = true;
                sec.Add(s1.sector);
                sectors = sec.AsReadOnly();

                s1.sector.addInternalConnection(this);
            }
            
            
        }

        public void setConnectionValue(int val)
        {
            if (internalConnection)
                throw new Exception("you can't have internal connection with high level");
            connectionValue = val;
        }

        public int getConnectionLevel()
        {
            return connectionValue;
        }


        public void checkConsistency()
        {
            if (!internalConnection)
                foreach (Sector sc in sectors)
                {
                    if (!sc.getConnections().Contains(this))
                        throw new System.Exception("a sector in a connection does not contain the connection");

                }
            if (internalConnection)
                foreach (Sector sc in sectors)
                {
                    if (!sc.getInternalConnections().Contains(this))
                        throw new System.Exception("a sector in a connection does not contain the connection");
                }

            foreach (SubSector sbs in subSectors)
                if (!sbs.getConnections().Contains(this))
                    throw new System.Exception("a sector contains and not contains a connection");


            if ((subSectors[0]).sector == subSectors[1].sector && !internalConnection)
                throw new Exception("a not internat with same sector");
        }

        [Serializable]
        public class SerializableConnection
        {
            public int connectionValue;
            public int[] sectorsConnected;
            public int[] sector;
            public bool internalConnection;

            public SerializableConnection() { }

            public SerializableConnection(Connection con)
            {
                internalConnection = con.internalConnection;
                connectionValue = con.connectionValue;
                /*if (internalConnection)
                {
                    UnityEngine.Debug.Log("saved an internal connection");

                    sectorsConnected = new int[] {
                       con.subSectors[0].sector.getSubSectors().IndexOf(con.subSectors[0]),
                       con.subSectors[1].sector.getSubSectors().IndexOf(con.subSectors[1])};
                }
                else
                {*/
                    sectorsConnected = new int[] {
                       con.subSectors[0].sector.getSubSectors().IndexOf(con.subSectors[0]),
                       con.subSectors[1].sector.getSubSectors().IndexOf(con.subSectors[1])};

                  
                    sector = new int[] {
                       con.galaxy.getSectors().IndexOf(con.subSectors[0].sector),
                       con.galaxy.getSectors().IndexOf(con.subSectors[1].sector)};
                //}
            }

            public void setUpConnection(Connection cn)
            {
                cn.connectionValue = connectionValue;
            }
        }
    }
}