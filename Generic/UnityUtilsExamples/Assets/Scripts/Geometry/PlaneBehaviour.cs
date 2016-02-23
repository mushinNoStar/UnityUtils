using UnityEngine;
using System.Collections.Generic;

public delegate void VoidMethod();
public delegate void IntMethod(int number);

namespace Geometry
{
    public class PlaneBehaviour : MonoBehaviour
    {
        public event IntMethod OnClicked;
        /// <summary>
        /// this happens one each frame.
        /// </summary>
        public event VoidMethod OnOver;

        private Vector2 center = Vector2.zero;

        /// <summary>
        /// creates a new object on the screen. 
        /// </summary>
        /// <returns></returns>
        public static PlaneBehaviour loadOne()
        {
            GameObject gm = Instantiate(Resources.Load<GameObject>("Plane"));
            return gm.GetComponent<PlaneBehaviour>();
        }

        /// <summary>
        /// Set the vertices of the plane, the vertices must be ordered first.
        /// Consecutive vertices will be connected to the center to make a triangle.
        /// The last one and the first one
        /// will be used to create a traingle too.
        /// </summary>
        /// <param name="vertices"></param>
        public void setVertices(List<Vector2> vertices)
        {
            if (vertices.Count < 3)
                throw new System.ArgumentException("Can't convert a vector of 2 elements in a plane");
            
            //vertices
            center = Vector2.zero; //finds the center
            foreach (Vector2 vertex in vertices)
                center += vertex;
            center = center / vertices.Count;
            //this should be the only place that set the center.

            List<Vector3> meshVertices = new List<Vector3>(); //creates a list of vector3, center in front
            meshVertices.Add(new Vector3(center.x, center.y, 0));
            foreach (Vector2 vertex in vertices)
                meshVertices.Add(new Vector3(vertex.x, vertex.y, 0));

            Mesh currentMesh = new Mesh(); //finds the mesh, and sets the vertices.

            //triangles.
            int[] newTriangles = new int[vertices.Count * 3]; //triangles have 3 vertices each
            for (int a = 1; a < meshVertices.Count - 1; a++) //the first item of the vertices list is the center. so it will be skipped.
            {
                newTriangles[a * 3] = a; //a point
                newTriangles[(a * 3) + 1] = a + 1; //the consecutive one
                newTriangles[(a * 3) + 2] = 0; //the center.
            }
           
            newTriangles[0] = meshVertices.Count - 1; //the last vertex
            newTriangles[1] = 1; //the fist one
            newTriangles[2] = 0; //the center

            currentMesh.vertices = (meshVertices.ToArray());

            currentMesh.SetTriangles(newTriangles, 0);
            GetComponent<MeshCollider>().sharedMesh = currentMesh;
            GetComponent<MeshFilter>().sharedMesh = currentMesh;

        }

        /// <summary>
        /// the center is the average of the vertex
        /// </summary>
        /// <returns></returns>
        public Vector2 getCenter()
        {
            return center;
        }

        /// <summary>
        /// destroy the objects
        /// </summary>
        public void remove()
        {
            Destroy(gameObject);
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0) && OnClicked != null)
                OnClicked(0);

            if (Input.GetMouseButtonDown(2) && OnClicked != null)
                OnClicked(1);
        }

        private void OnMouseOver()
        {
            if (OnOver != null)
                OnOver();
        }
    }
}