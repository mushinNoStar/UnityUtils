using System.Collections.Generic;
using System.Collections.ObjectModel;
using Plane.Internal;

namespace Plane
{
    /// <summary>
    /// A plane area is an abstract rappresentation of a set of points.
    /// You can later access the single points, it calculates the outermost points that creates
    /// a convex figure that contains every point.
    /// </summary>
    public class PlaneArea
    {
        private List<IVertex> vertices = new List<IVertex>();
        private List<IVertex> outermostVertices = new List<IVertex>();
        private PlaneAreaRappresentation rappresentation;

        public PlaneArea(List<IVertex> verticesList)
        {
            setVerices(verticesList);
            rappresentation = new PlaneAreaRappresentation(this);
        }

        public PlaneArea()
        {
            rappresentation = new PlaneAreaRappresentation(this);
        }

        public void setVerices(List<IVertex> verticesList)
        {
            vertices = verticesList;
            notifyChange();
        }

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
            calculateOutermostVertices();
            rappresentation.update();
        }

        private void calculateOutermostVertices()
        {
            throw new System.NotImplementedException();
        }
    }
}