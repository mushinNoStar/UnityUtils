using System.Collections.Generic;
using UnityEngine;
using System;

namespace Geometry
{
    public delegate void segmentMethod(IPlaneSegment segment);
    public delegate void extendedSegmentMethod(IPlaneSegment segment, int num);

    public class Utils
    {

        /// <summary>
        /// sort clockwise, lowest point first.
        /// </summary>
        /// <param name="vertices"></param>
        /// <returns></returns>
        public static List<IVertex> sort2d(List<IVertex> vertices)
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
                float currentSlope = getSlope(sortedVertex[0], vertices[a]);

                List<IVertex> currentList = rightSideVertex;
                if (currentSlope >= 0)
                    currentList = rightSideVertex;
                else
                    currentList = leftSideVertex;

                for (int b = 0; b < currentList.Count; b++)
                {
                    float confrontingSlope = getSlope(sortedVertex[0], currentList[b]);
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
            return sortedVertex;
        }

        public static List<Vector2> sort2d(List<Vector2> vertices)
        {
            Vector2 lowestPoint = vertices[0]; //search for lowest point
            foreach (Vector2 v in vertices)
            {
                if (v.y < lowestPoint.y)
                    lowestPoint = v;
                if (v.y == lowestPoint.y)
                    if (v.x < lowestPoint.x)
                        lowestPoint = v;
            }


            List<Vector2> sortedVertex = new List<Vector2>();
            List<Vector2> rightSideVertex = new List<Vector2>();
            List<Vector2> leftSideVertex = new List<Vector2>();
            sortedVertex.Add(lowestPoint);
            vertices.Remove(lowestPoint);

            //insert sort, i'm not planning to have 10 millions points, if you do, change this
            for (int a = 0; a < vertices.Count; a++)
            {
                float currentSlope = getSlope(sortedVertex[0], vertices[a]);

                List<Vector2> currentList = rightSideVertex;
                if (currentSlope >= 0)
                    currentList = rightSideVertex;
                else
                    currentList = leftSideVertex;

                for (int b = 0; b < currentList.Count; b++)
                {
                    float confrontingSlope = getSlope(sortedVertex[0], currentList[b]);
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
            return sortedVertex;
        }

        public static float getSlope(Vector3 v1, Vector3 v2)
        {
            return (v1.y - v2.y) / (v1.x - v2.x);
        }

        public static float getSlope(IVertex v1, IVertex v2)
        {
            return (v1.get2dPosition().y - v2.get2dPosition().y) / (v1.get2dPosition().x - v2.get2dPosition().x);
        }
}
}