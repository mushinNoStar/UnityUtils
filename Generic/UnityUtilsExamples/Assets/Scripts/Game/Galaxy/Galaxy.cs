using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ceometric.DelaunayTriangulator;
using ceometric;

namespace Game
{
    public class Galaxy
    {
        private List<Sector> sectors = new List<Sector>();
        private List<Connection> connections = new List<Connection>();
        public GalaxyGenerationParameter generationParam;

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
            }
        }

        public void generate(GalaxyGenerationParameter param)
        {
            generationParam = param;
            //throw new System.Exception("not implemented");

            List<IVertex> verts = new List<IVertex>();
            for (int a = 0; a < param.numberOfSystems; a++)
            {
                float x = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                float y = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                Vector2 vec = new Vector2(x,y);
                Sector sc = new Sector(this, vec, Tools.Utils.randomName());
                sectors.Add(sc);
                verts.Add(sc);
            }

            generateConnections();

            Debug.Log("generated " + sectors.Count + " sectors, " + connections.Count+" connections");
           /* foreach (Connection c in connections)
            {
                Vector3 start = new Vector3(c.sectors[0].get2dPosition().x, c.sectors[0].get2dPosition().y);
                Vector3 end = new Vector3(c.sectors[1].get2dPosition().x, c.sectors[1].get2dPosition().y);

                Debug.DrawLine(start, end, Color.red, 10);
            }*/
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