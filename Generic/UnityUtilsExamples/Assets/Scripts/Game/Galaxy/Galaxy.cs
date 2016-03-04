using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ceometric.DelaunayTriangulator;
using ceometric;
using System;
using System.Xml.Serialization;
using System.IO;

namespace Game
{
    /// <summary>
    /// this should contains every data related to game mechanics, except for Actions.
    /// </summary>
    public class Galaxy
    {
        private List<Sector> sectors = new List<Sector>();
        private List<Connection> connections = new List<Connection>();
        public GalaxyGenerationParameter generationParam;
        public List<SubSector> everySubSector = new List<SubSector>();

        public List<Vector2> structureList = new List<Vector2>();
        public List<string> strctureName = new List<string>();

        public ReadOnlyCollection<Sector> getSectors()
        {
            return sectors.AsReadOnly();
        }

        public ReadOnlyCollection<Connection> getConnection()
        {
            return connections.AsReadOnly();
        }

        public List<Triangle> triangulate(List<Point> points)
        {
            DelaunayTriangulation2d d = new DelaunayTriangulation2d();
            List<Triangle> tris = d.Triangulate(points);
            return tris;
        }

        private void generateConnections()
        {

            //triangolate
            List<Point> points = new List<Point>();
            foreach (IVertex v in getSectors())
                points.Add(new Point(v.get2dPosition().x, v.get2dPosition().y, 0));

            List<Triangle> tris = triangulate(points);

            List<int> extremes = Tools.Utils.transformInToListOfUniqueVectors(points, tris); //remove double connection, return list of pairs of indexes to sectors

            for (int a = 0; a < extremes.Count; a += 2)
            {

                Sector sc = sectors[extremes[a]];
                Sector sc2 = sectors[extremes[a + 1]];
                new Connection(this, sc.getNearestSubSector(sc2.get2dPosition()), sc2.getNearestSubSector(sc.get2dPosition()));
               
            }

            
        }

        public void generateStructure(List<Vector2> galaxyStructure, List<string> names, GalaxyGenerationParameter prm)
        {
            generationParam = prm;
            List<IVertex> verts = new List<IVertex>();
            foreach (Vector2 vc in galaxyStructure)
            {
                Sector sc = new Sector(this, vc, names[galaxyStructure.IndexOf(vc)], true);
                verts.Add(sc);
            }
            structureList = galaxyStructure;
            strctureName = names;

            generateConnections();
            Debug.Log("generated " + sectors.Count + " sectors, " + everySubSector.Count + " subs " + connections.Count + " connections");

        }

        public void addSector(Sector sc)
        {
            sectors.Add(sc);
        }

        public void generateGalaxyData()
        {
            foreach (Connection cn in connections)
                if (!cn.internalConnection)
                    cn.setConnectionValue(Mathf.FloorToInt(UnityEngine.Random.value * 4));
            Nation nt = new Nation("Humans", this, Color.green);
            for (int a = 0; a < generationParam.numberOfPlayer; a++)
                new Nation(Tools.Utils.randomName(), this, Tools.Utils.randomColor());
            nt.setOwnedSector(sectors[0]);
        }

        public void checkConsistency()
        {
            foreach (Sector sc in sectors)
                sc.checkConsistency();
            foreach (Connection cn in connections)
                cn.checkConsistency();
            foreach (Nation nt in Nation.getNations())
                nt.checkConsistency();
        }

        public void save(string path)
        {
            var serializer = new XmlSerializer(typeof(SerializableGalaxy));
            var stream = new FileStream(path, FileMode.Create);
            serializer.Serialize(stream, new SerializableGalaxy(this));
            stream.Close();
        }

        public void load(string path)
        {
            var serializer = new XmlSerializer(typeof(SerializableGalaxy));
            var stream = new FileStream(path, FileMode.Open);
            var data = serializer.Deserialize(stream) as SerializableGalaxy;
            stream.Close();

            data.setUpGalaxy(this);
            Debug.Log("generated " + sectors.Count + " sectors, " + everySubSector.Count + " subs " + connections.Count + " connections");

        }

        public void tick()
        {
            foreach (Sector sc in sectors)
                sc.tick();
            foreach (Nation n in Nation.getNations())
                n.tick();
        }

        public void addConnection(Connection con)
        {
            connections.Add(con);
        }

        [Serializable]
        [XmlRoot("Galaxy")]
        public class SerializableGalaxy
        {
            public List<Sector.SerializableSector> sectors = new List<Sector.SerializableSector>();
            public List<Connection.SerializableConnection> connections = new List<Connection.SerializableConnection>();
            public List<Nation.SerializableNation> nations = new List<Nation.SerializableNation>();
            public List<Player.SerializablePlayer> players = new List<Player.SerializablePlayer>();
            public GalaxyGenerationParameter genParm;

            public SerializableGalaxy()
            { }

            public SerializableGalaxy(Galaxy toSerialize)
            {
                foreach (Sector sc in toSerialize.sectors)
                    sectors.Add(new Sector.SerializableSector(sc));
                foreach (Nation nt in Nation.getNations())
                    nations.Add(new Nation.SerializableNation(nt));
                foreach (Connection connection in toSerialize.connections)
                    connections.Add(new Connection.SerializableConnection(connection));
                foreach (Player pl in Player.getPlayers())
                    players.Add(new Player.SerializablePlayer(pl));
                genParm = toSerialize.generationParam;
            }

            public void setUpGalaxy(Galaxy gal)
            {
                gal.generationParam = genParm;
                foreach (Sector.SerializableSector sc in sectors)
                {
                    Sector sect = new Sector(gal, new Vector2(sc.pos[0], sc.pos[1]), sc.name, false);
                    sc.setUpSector(sect);
                }
                foreach (Connection.SerializableConnection cn in connections)
                {

                    SubSector sb1 = gal.sectors[cn.sector[0]].getSubSectors()[cn.sectorsConnected[0]];
                    SubSector sb2 = gal.sectors[cn.sector[1]].getSubSectors()[cn.sectorsConnected[1]];
                    Connection c = new Connection(gal,sb1,sb2);
                    cn.setUpConnection(c);
                }
                foreach (Nation.SerializableNation nt in nations)
                {
                    Color col = new Color(nt.color[0], nt.color[1], nt.color[2], nt.color[3]);
                    Nation n = new Nation(nt.name, gal, col);
                    nt.setUpNation(n);
                }
                foreach (Player.SerializablePlayer pl in players)
                {
                    Player p = new Player(pl.name);
                    pl.setUpPlayer(p);
                }
            }
        }
    }
}