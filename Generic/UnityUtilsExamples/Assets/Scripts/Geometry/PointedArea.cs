using System.Collections.Generic;
using ceometric.DelaunayTriangulator;
using UnityEngine;
using Geometry.Internal;
using System.Collections.ObjectModel;

namespace Geometry
{
    public class PointedArea
    {
        private OutlinedConvexArea boundArea;
        private List<IVertex> vertx = new List<IVertex>();
        private List<PlaneSegment> segments = new List<PlaneSegment>(); //this is bewteen to points
        private List<OutlinedConvexArea> areas = new List<OutlinedConvexArea>();
        private List<bool> markedAreas = new List<bool>();
        private Material planesMaterial;
        private Material segmentsMaterial;
        private Material bordMaterial;
        private float segmentsWidth;
        private float areaBordWidth;

        private List<Triangle> tris;

        public PointedArea(
            List<IVertex> vertices,
            float segWidth,
            Material planeMat,
            Material planetBorderMaterial, 
            Material conMat, 
            List<IVertex> bounds, 
            Material boundMat, 
            Material extBorded,
            float areasBorderWidth)
        {
            foreach (IVertex v in bounds)
                vertices.Insert(0,v);
            boundArea = new OutlinedConvexArea(bounds, new Material(boundMat),new Material(extBorded), segWidth );
            vertx = vertices;
            areaBordWidth = areasBorderWidth;
            planesMaterial = planeMat;
            segmentsMaterial = conMat;
            segmentsWidth = segWidth;
            bordMaterial = planetBorderMaterial;
            generate();
        }

        public void show()
        {
            foreach (PlaneSegment sgm in segments)
            {
                if (!boundArea.contains(sgm.getEndingPoint()) && !boundArea.contains(sgm.getStartingPoint()))
                    sgm.show();
            }
            for (int a = 0; a < markedAreas.Count; a++)
                if (markedAreas[a])
                    areas[a].show();
        }

        public void hide()
        {
            foreach (PlaneSegment sgm in segments)
               sgm.hide();
            
            for (int a = 0; a < markedAreas.Count; a++)
                if (markedAreas[a])
                    areas[a].hide();
        }

        public ReadOnlyCollection<IVertex> getVertices()
        {
            return vertx.AsReadOnly();
        }

        public ReadOnlyCollection<PlaneSegment> getVertexConnections()
        {
            return segments.AsReadOnly();
        }

        public ReadOnlyCollection<OutlinedConvexArea> getAreas()
        {
            List<OutlinedConvexArea> diRitorno = new List<OutlinedConvexArea>();
            for (int a = 0; a < areas.Count; a++)
            {
                if (markedAreas[a])
                    diRitorno.Add(areas[a]);
            }
            return diRitorno.AsReadOnly();
        }

        public OutlinedConvexArea getAreaOfVertex(IVertex v)
        {
            return areas[vertx.IndexOf(v)];
        }
        

        private void generate()
        {
            List<Point> points = new List<Point>();

            foreach (IVertex v in vertx)
                points.Add(new Point(v.get2dPosition().x, v.get2dPosition().y, 0));

            DelaunayTriangulation2d d = new DelaunayTriangulation2d();
            tris = d.Triangulate(points);
            generateSegments(points);
            foreach (Point v in points)
                generatePointArea(v, points.IndexOf(v));
        }

        private void generatePointArea(Point v, int index)
        {
            List<Triangle> nearTris = new List<Triangle>();
            foreach (Triangle t in tris)
            {
                if (t.Vertex1 == v || t.Vertex2 == v || t.Vertex3 == v)
                    nearTris.Add(t);
            }
            List<IVertex> areaPos = new List<IVertex>();
            if (nearTris.Count > 2)
            {
                foreach (Triangle t in nearTris)
                    areaPos.Add(new TestVertex(t.getCenter().x, t.getCenter().y));
                areas.Add(new OutlinedConvexArea(areaPos,new Material(planesMaterial),new Material(bordMaterial), areaBordWidth));
                markedAreas.Add(true);
                return;
            }
            else
            {
                areas.Add(null);
                markedAreas.Add(false);
            }
        }

        private void generateSegments(List<Point> points)
        {
            List<int> segs = new List<int>();
            foreach (Triangle t in tris)
            {
                segs.Add(points.IndexOf(t.Vertex1));
                segs.Add(points.IndexOf(t.Vertex2));
                segs.Add(points.IndexOf(t.Vertex2));
                segs.Add(points.IndexOf(t.Vertex3));
                segs.Add(points.IndexOf(t.Vertex1));
                segs.Add(points.IndexOf(t.Vertex3));
            }

            for (int a = 0; a < segs.Count; a += 2)
            {
                for (int b = a + 2; b < segs.Count; b += 2)
                {
                    if ((segs[a] == segs[b] && segs[a + 1] == segs[b + 1])
                        || (segs[a + 1] == segs[b] && segs[a] == segs[b + 1]))
                    {
                        segs.RemoveAt(a);
                        segs.RemoveAt(a);
                        a -= 2;
                        b -= 2;
                        break;
                    }
                }
            }

            for (int a = 0; a < segs.Count; a += 2)
            {
                IVertex v = vertx[segs[a]];
                IVertex v2 = vertx[segs[a + 1]];
                if (boundArea.getArea().getVertices().Contains(v) || boundArea.getArea().getVertices().Contains(v2))
                    continue;
                segments.Add(new PlaneSegment(v, v2, segmentsWidth, new Material(segmentsMaterial)));
            }
        }
    }
}