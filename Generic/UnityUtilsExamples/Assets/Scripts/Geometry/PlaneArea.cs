using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Geometry.Internal;
using Rappresentation;

namespace Geometry
{
    /// <summary>
    /// A plane area is an abstract rappresentation of a set of points.
    /// You can later access the single points, it calculates the outermost points that creates
    /// a convex figure that contains every point, and orderes them.
    /// </summary>
    public class PlaneArea : Rappresentable
    {
        private List<IVertex> vertices = new List<IVertex>();
        private List<IVertex> outermostVertices = new List<IVertex>();
        
        public PlaneArea(List<IVertex> verticesList, UnityEngine.Material mat) 
        {
            setRappresentation(new PlaneAreaRappresentation(this, mat));
            setVerices(verticesList);
        }

        /// <summary>
        /// Overrides every vertex in the plane area.
        /// </summary>
        /// <param name="verticesList"></param>
        public void setVerices(List<IVertex> verticesList)
        {
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
            {
                outermostVertices = vertices;
                return;
            }
           
            calculateOutermostVertices();
            update();
        }

        private void calculateOutermostVertices()
        {

            vertices = Utils.sort2d(vertices);
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


        /// <summary>
        /// v1 is the center of the 2 segments, the v1 and v3 can be swappend.
        /// </summary>
        /// <param name="v1">the center of the 2 segments</param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        /// <returns></returns>
        public static bool isLeftTurn(IVertex v1, IVertex v2, IVertex v3)
        {
            float[] x = new float[3] {v2.get2dPosition().x, v1.get2dPosition().x, v3.get2dPosition().x };
            float[] y = new float[3] {v2.get2dPosition().y, v1.get2dPosition().y, v3.get2dPosition().y };
            float val = ((x[1]-x[0]) * (y[2]-y[0])) - ((y[1]-y[0]) * (x[2]-x[0])); //cross product of the

            if (val > 0)
                return true;
            else
                return false;
        }

        public static bool isLeftTurn(UnityEngine.Vector2 v1, UnityEngine.Vector2 v2, UnityEngine.Vector2 v3)
        {
            float[] x = new float[3] { v2.x, v1.x, v3.x };
            float[] y = new float[3] { v2.y, v1.y, v3.y };
            float val = ((x[1] - x[0]) * (y[2] - y[0])) - ((y[1] - y[0]) * (x[2] - x[0])); //cross product of the

            if (val > 0)
                return true;
            else
                return false;
        }
    }
}