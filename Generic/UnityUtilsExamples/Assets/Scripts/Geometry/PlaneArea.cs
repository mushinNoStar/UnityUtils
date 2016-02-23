using System.Collections.Generic;
using System.Collections.ObjectModel;
using Geometry.Internal;
using System;

namespace Geometry
{
    /// <summary>
    /// A plane area is an abstract rappresentation of a set of points.
    /// You can later access the single points, it calculates the outermost points that creates
    /// a convex figure that contains every point, and orderes them.
    /// </summary>
    public class PlaneArea
    {
        private List<IVertex> vertices = new List<IVertex>();
        private List<IVertex> outermostVertices = new List<IVertex>();
        private PlaneAreaRappresentation rappresentation;
        
        /// <summary>
        /// You must specify at least 3 vertex create an area.
        /// </summary>
        /// <param name="verticesList"></param>
        public PlaneArea(List<IVertex> verticesList)
        {
            rappresentation = new PlaneAreaRappresentation(this);
            setVerices(verticesList);
        }

        /// <summary>
        /// Overrides every vertex in the plane area. there still must be at least 3 vertex.
        /// </summary>
        /// <param name="verticesList"></param>
        public void setVerices(List<IVertex> verticesList)
        {
            if (verticesList.Count < 3)
                throw new ArgumentException("A 2 point plane is called segment.");
            vertices = verticesList;
            notifyChange();
        }

        /// <summary>
        /// not present vertex are ignored, if the number count drop below zero, a error is thrown.
        /// </summary>
        /// <param name="vertex"></param>
        public void removeVertex(IVertex vertex)
        {
            if (vertices.Contains(vertex))
            {
                if (vertices.Count == 3)
                    throw new ArgumentException("There are only three point left in this plane, hide it instead of removing vertex");
                vertices.Remove(vertex);
                notifyChange();
            }
        }

        public void addVertex(IVertex vertex)
        {
            if (!vertices.Contains(vertex))
            {
                vertices.Add(vertex);
                notifyChange();
            }
        }
        /// <summary>
        /// vertices that already present are not added again.
        /// </summary>
        /// <param name="listVertices"></param>
        public void addVertex(List<IVertex> listVertices)
        {
            for (int a = vertices.Count; a >= 0; a--)
                if (vertices.Contains(listVertices[a]))
                    listVertices.RemoveAt(a);

            if (listVertices.Count > 0)
            {
                vertices.AddRange(vertices);
                notifyChange();
            }
        }

        public ReadOnlyCollection<IVertex> getVertices()
        {
            return vertices.AsReadOnly();
        }

        /// <summary>
        /// return the convex hull of the area created by the plane
        /// </summary>
        /// <returns></returns>
        public ReadOnlyCollection<IVertex> getOutermostVertices()
        {
            return outermostVertices.AsReadOnly();
        }

        public void show()
        {
            rappresentation.show();
        }

        public void hide()
        {
            rappresentation.hide();
        }

        public bool isVisible()
        {
            return rappresentation.isVisible();
        }

        private void notifyChange()
        {
            for (int a = vertices.Count - 1; a > 0; a--)
            {
                IVertex v = vertices[a];
                while (vertices.Contains(v))
                    vertices.Remove(v);
                vertices.Add(v);
                if (a >= vertices.Count)
                    a = vertices.Count - 1;
            }

            if (vertices.Count < 3)
                throw new ArgumentException("A 2 point plane is called segment.");
            calculateOutermostVertices();
            rappresentation.update();
        }

        private void calculateOutermostVertices()
        {

            sort();
            //actuall graham scan
            List<IVertex> fakeStack = new List<IVertex>();
            fakeStack.Add(vertices[0]);
            fakeStack.Add(vertices[1]);
            fakeStack.Add(vertices[2]);
            for (int a = 3; a < vertices.Count; a++)
            {
                while (!isLeftTurn(fakeStack[fakeStack.Count - 2], fakeStack[fakeStack.Count - 1], vertices[a]))
                    fakeStack.RemoveAt(fakeStack.Count - 1);
                fakeStack.Add(vertices[a]);
            }

            outermostVertices = fakeStack;
        }

        private void sort()
        {
            IVertex lowestPoint = vertices[0]; //search for lowest point
            foreach (IVertex v in vertices)
            {
                if (v.get2dPosition().y < lowestPoint.get2dPosition().y)
                    lowestPoint = v;
                if (v.get2dPosition().y == lowestPoint.get2dPosition().y)
                    if (v.get2dPosition().x < lowestPoint.get2dPosition().x)
                        lowestPoint = v;
            }


            List<IVertex> sortedVertex = new List<IVertex>();
            List<IVertex> rightSideVertex = new List<IVertex>();
            List<IVertex> leftSideVertex = new List<IVertex>();
            sortedVertex.Add(lowestPoint);
            vertices.Remove(lowestPoint);

            //insert sort, i'm not planning to have 10 millions points, if you do, change this
            for (int a = 0; a < vertices.Count; a++)
            {
                float currentSlope = sortedVertex[0].getSlope(vertices[a]);

                List<IVertex> currentList = rightSideVertex;
                if (currentSlope >= 0)
                    currentList = rightSideVertex;
                else
                    currentList = leftSideVertex;

                for (int b = 0; b < currentList.Count; b++)
                {
                    float confrontingSlope = sortedVertex[0].getSlope(currentList[b]);
                    if (Math.Abs(currentSlope) > Math.Abs(confrontingSlope))
                    {
                        currentList.Insert(b, vertices[a]);
                        break;
                    }
                }
                if (!currentList.Contains(vertices[a]))
                    currentList.Add(vertices[a]);
            }
            leftSideVertex.Reverse();
            sortedVertex.AddRange(leftSideVertex);
            sortedVertex.AddRange(rightSideVertex);
            vertices = sortedVertex;
        }

        /// <summary>
        /// v1 is the center of the 2 segments, the v1 and v3 can be swappend.
        /// </summary>
        /// <param name="v1">the center of the 2 segments</param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        private bool isLeftTurn(IVertex v1, IVertex v2, IVertex v3)
        {
            float[] x = new float[3] {v2.get2dPosition().x, v1.get2dPosition().x, v3.get2dPosition().x };
            float[] y = new float[3] {v2.get2dPosition().y, v1.get2dPosition().y, v3.get2dPosition().y };
            float val = ((x[1]-x[0]) * (y[2]-y[0])) - ((y[1]-y[0]) * (x[2]-x[0])); //cross product of the

            if (val > 0)
                return true;
            else
                return false;
        }
    }
}