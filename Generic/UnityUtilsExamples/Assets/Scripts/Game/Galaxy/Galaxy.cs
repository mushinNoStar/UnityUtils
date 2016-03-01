using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ceometric.DelaunayTriangulator;
using ceometric;

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
                Connection cn = new Connection(this,sc, sc2);
                connections.Add(cn);
                sc2.addNeighbour(sc);
                sc.addNeighbour(sc2);
            }
        }

        public void generateStructure(List<Vector2> galaxyStructure, List<string> names)
        {

            List<IVertex> verts = new List<IVertex>();
            foreach (Vector2 vc in galaxyStructure)
            {
                Sector sc = new Sector(this, vc, names[galaxyStructure.IndexOf(vc)]);
                sectors.Add(sc);
                verts.Add(sc);
            }
            structureList = galaxyStructure;
            strctureName = names; 

            generateConnections();
            Debug.Log("generated " + sectors.Count + " sectors, " + connections.Count + " connections");

        }

        public void generateGalaxyData(GalaxyGenerationParameter prm)
        {
            generationParam = prm;
            foreach (Connection cn in connections)
                cn.setConnectionValue(Mathf.FloorToInt(Random.value * 4));
            Nation nt = new Nation("Humans", this, Color.green);
            nt.setOwnedSector(sectors[0]);
        }

        public void checkConsistency()
        {
            foreach (Sector sc in sectors)
                sc.checkConsistency();
            foreach (Connection cn in connections)
                cn.checkConsistency();
        }

    }
}