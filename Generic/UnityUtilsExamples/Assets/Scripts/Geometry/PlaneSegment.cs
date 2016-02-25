using System;
using Rappresentation;
using Geometry.Internal;

namespace Geometry
{
    public class PlaneSegment : Rappresentable, IPlaneSegment
    {
        private IVertex[] verts= new IVertex[2];
        private float width;

        public PlaneSegment (IVertex v1, IVertex v2, float w, UnityEngine.Material mat)
        {
            verts[0] = v1;
            verts[1] = v2;
            setRappresentation(new PlaneSegmentRappresentation(this, mat));
            width = w;
        }

        public IVertex getStartingPoint()
        {
            return verts[0];
        }

        public IVertex getEndingPoint()
        {
            return verts[1];
        }

        public virtual float getWidth()
        {
            return width;
        }

    }
}