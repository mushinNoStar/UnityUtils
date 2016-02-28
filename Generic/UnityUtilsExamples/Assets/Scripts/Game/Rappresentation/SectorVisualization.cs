using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ceometric.DelaunayTriangulator;
using UnityEngine;
using Vision;

namespace Game
{
    public class SectorVisualization : Visualization, ISelectable
    {
        public readonly Sector sector;
        private static List<SectorVisualization> vis = new List<SectorVisualization>();
        private List<Triangle> trs = null;
        private List<Point> points = null;
        private List<Vector2> extremes = new List<Vector2>();
        private int myNumber = -1;

        public SectorVisualization(Sector targetSector) : base()
        {
            sector = targetSector;
            vis.Add(this);
            generateExtremes();

            myNumber = SectorBehaviour.getSectorBehaviour().addSector(extremes);
        }

        public ReadOnlyCollection<SectorVisualization> getSectorsVisualizations()
        {
            return vis.AsReadOnly();
        }

        public override void hide()
        {
            throw new NotImplementedException();
        }

        public override bool isVisible()
        {
            //throw new NotImplementedException();
            return false;
        }

        public override void show()
        {
            throw new NotImplementedException();
        }

        public override void update()
        {
            throw new NotImplementedException();
        }

        private List<Triangle> getTriangulation()
        {
            if (trs == null)
            {
                trs = Game.getGame().galaxy.triangulate(getPoints());
            }
            return trs;
        }

        private List<Point> getPoints()
        {
            if (points == null)
            {
                Galaxy gl = Game.getGame().galaxy;
                points = new List<Point>();
                foreach (IVertex v in gl.getSectors())
                    points.Add(new Point(v.get2dPosition().x, v.get2dPosition().y, 0));

                float f = gl.generationParam.galaxyEdge;
                points.Add(new Point(f, f, 0));
                points.Add(new Point(-1*f, f, 0));
                points.Add(new Point(-1 * f,-1* f, 0));
                points.Add(new Point(f, -1 * f, 0));


            }
            return points;
        }

        private void generateExtremes()
        {
            int index = sector.galaxy.getSectors().IndexOf(sector);
            
            Point v = getPoints()[index];

            List<Triangle> nearTris = new List<Triangle>();
            foreach (Triangle t in getTriangulation())
            {
                if (t.Vertex1 == v || t.Vertex2 == v || t.Vertex3 == v)
                    nearTris.Add(t);
            }

  
            if (nearTris.Count > 2)
            {
                foreach (Triangle t in nearTris)
                    extremes.Add(new Vector2(t.getCenter().x, t.getCenter().y));
               
            }
            extremes = Tools.Utils.sort2d(extremes);

        }

        public void OnSelectStart()
        {
            throw new NotImplementedException();
        }

        public void OnSelectEnd()
        {
            throw new NotImplementedException();
        }
    }
}