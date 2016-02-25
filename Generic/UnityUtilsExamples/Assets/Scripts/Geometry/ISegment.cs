using UnityEngine;
namespace Geometry
{
    public interface IPlaneSegment
    {
        IVertex getStartingPoint();
        IVertex getEndingPoint();
        float getWidth();
    }
}