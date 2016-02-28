using Geometry;
using UnityEngine;
using Geometry.Internal;
using System.Collections.Generic;
using Game;

namespace Galaxy
{
    public class Galaxy
    {
        public PointedArea galRapp;
        private List<Sector> sectors = new List<Sector>();

        public Galaxy(string Path)
        {
            throw new System.Exception("Not implemented");
        }

        public Galaxy()
        {
    
        }

        public void save(string path)
        {
            
        }

        public void generate(GalaxyGenerationParameter param)
        {
            sectors.Clear();
            List<IVertex> verts = new List<IVertex>();
            for (int a = 0; a < param.numberOfSystems; a++)
            {
                float x = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                float y = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                Sector sc = new Sector(x, y);
                sectors.Add(sc);
                verts.Add(sc);
            }

            List<IVertex> bounds = new List<IVertex>();
            bounds.Add(new TestVertex(3f * param.galaxyEdge, 3f * param.galaxyEdge));
            bounds.Add(new TestVertex(-3f * param.galaxyEdge, 3f * param.galaxyEdge));
            bounds.Add(new TestVertex(-3f * param.galaxyEdge, -3f * param.galaxyEdge));
            bounds.Add(new TestVertex(3f * param.galaxyEdge, -3f * param.galaxyEdge));

            galRapp = new PointedArea
                (verts,
                param.connectionsWidth,
                param.planeMaterial,
                param.areasEdgeMaterial,
                param.connectionsMaterial,
                bounds,
                param.extremeBordersMaterial,
                param.extremeBordersMaterial,
                param.areasEdgeWidth);

            foreach (Sector sc in sectors)
            {
                sc.setArea(galRapp.getAreaOfVertex(sc));
            }
            foreach (PlaneSegment pln in galRapp.getVertexConnections())
            {
                ((Sector)pln.getEndingPoint()).addConnection((Sector)pln.getStartingPoint());
                ((Sector)pln.getStartingPoint()).addConnection((Sector)pln.getStartingPoint());
                ((Sector)pln.getEndingPoint()).addSegment(pln);
                ((Sector)pln.getStartingPoint()).addSegment(pln);
            }
            foreach (Sector sc in sectors)
            {
                foreach (Sector near in sc.getNearSectors())
                {
                    foreach (PlaneSegment pln in near.getArea().getSegments())
                    {
                        System.Collections.ObjectModel.ReadOnlyCollection<IVertex> list = sc.getArea().getArea().getVertices();
                        if (list.Contains(pln.getEndingPoint()) || list.Contains(pln.getStartingPoint()))
                            sc.addBorder(pln);
                        
                    }
                }
                foreach (PlaneSegment pln in sc.getArea().getSegments())
                {
                    sc.addBorder(pln);
                }
            }

        }
    }
}