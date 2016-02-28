using UnityEngine;
using System.Collections.Generic;

public delegate void VoidMethod();
public delegate void IntMethod(int number);
public delegate void DoubleIntMethod(int number, int numb);

namespace Geometry
{
    public class PlaneBehaviour : MonoBehaviour
    {
        public event DoubleIntMethod OnClicked;
        /// <summary>
        /// this happens one each frame.
        /// </summary>
        public event IntMethod OnOver;
        private static PlaneBehaviour plb = null;
        private List<Vector2> center = new List<Vector2>();
        private List<object> ls = new List<object>();

        public int getMyNumber(object g)
        {
            if (ls.Contains(g))
                return ls.IndexOf(g);
            ls.Add(g);
            return ls.Count - 1;
        }

        private void init()
        {
            GetComponent<MeshFilter>().sharedMesh.subMeshCount = 1;
            GetComponent<MeshFilter>().sharedMesh.triangles = new int[0];
            GetComponent<MeshFilter>().sharedMesh.SetVertices(new List<Vector3>());
        }

        /// <summary>
        /// creates a new object on the screen. 
        /// </summary>
        /// <returns></returns>
        public static PlaneBehaviour loadOne()
        {
            if (plb == null)
            {
                plb = Instantiate(Resources.Load<GameObject>("Plane")).GetComponent<PlaneBehaviour>();
                plb.init();
            }
            return plb;
        }

        public void setMaterial(Material mat, int target)
        {
            MeshRenderer rnd = GetComponent<MeshRenderer>();
            List<Material> mats = new List<Material>();
            foreach (Material mart in rnd.sharedMaterials)
                mats.Add(mart);
            if (target == mats.Count)
                mats.Add(mat);
            else
                mats[target] = mat;
            rnd.sharedMaterials = mats.ToArray();
        }

        /// <summary>
        /// Set the vertices of the plane, the vertices must be ordered first.
        /// Consecutive vertices will be connected to the center to make a triangle.
        /// The last one and the first one
        /// will be used to create a traingle too.
        /// </summary>
        /// <param name="vertices"></param>
        public void setVertices(List<Vector2> vertices, int target)
        {
          
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh currentMesh = meshFilter.sharedMesh;
            if (vertices.Count < 3)
                throw new System.ArgumentException("Can't convert a vector of 2 elements in a plane");
            if (target > GetComponent<MeshFilter>().sharedMesh.subMeshCount)
                throw new System.ArgumentException("No such sub mesh");

            if (target == currentMesh.subMeshCount)
                currentMesh.subMeshCount = target + 1;

            
            //vertices
            Vector2 centercalc = Vector2.zero; //finds the center
            foreach (Vector2 vertex in vertices)
                centercalc += vertex;
            centercalc = centercalc / vertices.Count;

            if (target == meshFilter.sharedMesh.subMeshCount - 1)
                center.Add(centercalc);
            else
                center[target] = centercalc;
            //this should be the only place that set the center.

            List<Vector3> meshVertices = new List<Vector3>(); //creates a list of vector3, center in front
            int startPos = currentMesh.vertexCount;

            foreach (Vector3 vert in currentMesh.vertices)
                meshVertices.Add(vert);
            List<Vector3> newVerts = new List<Vector3>();
            Vector3 o = new Vector3(center[target].x, center[target].y, 0);
            meshVertices.Add(o);
            newVerts.Add(o);
            foreach (Vector2 vertex in vertices)
            {
                Vector3 v = new Vector3(vertex.x, vertex.y, 0);
                meshVertices.Add(v);
                newVerts.Add(v);
            }
            currentMesh.SetVertices(meshVertices);
            //triangles.
            int[] newTriangles = new int[newVerts.Count * 3]; //triangles have 3 vertices each
            for (int a = 1; a < newVerts.Count - 1; a++) //the first item of the vertices list is the center. so it will be skipped.
            {
                newTriangles[a * 3] = a + startPos; //a point
                newTriangles[(a * 3) + 1] = a + 1 + startPos; //the consecutive one
                newTriangles[(a * 3) + 2] = startPos; //the center.
            }

            newTriangles[0] = newVerts.Count - 1 + startPos; //the last vertex
            newTriangles[1] = 1 + startPos; //the fist one
            newTriangles[2] = startPos; //the center

            //currentMesh.vertices = (meshVertices.ToArray());

            currentMesh.SetTriangles(newTriangles, target);
            meshFilter.sharedMesh = currentMesh;


            Destroy(GetComponent<MeshCollider>());
            gameObject.AddComponent<MeshCollider>();
            GetComponent<MeshCollider>().sharedMesh = currentMesh;

            recalculateNormalAndUv();

        }

        public void recalculateNormalAndUv()
        {

            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            mesh.RecalculateNormals();

            Vector3[] verticeass = mesh.vertices;
            Vector2[] uvs = new Vector2[verticeass.Length];

            for (int i = 0; i < uvs.Length; i++)
            {
                Vector2 pos = new Vector2(verticeass[i].x, verticeass[i].y);

                uvs[i] = new Vector2(pos.x / mesh.bounds.size.x + 0.5f, Mathf.Abs(pos.y) / mesh.bounds.size.y + 0.5f);
            }
            mesh.uv = uvs;
        }

        /// <summary>
        /// the center is the average of the vertex
        /// </summary>
        /// <returns></returns>
        public Vector2 getCenter(int target)
        {
            return center[target];
        }

        /// <summary>
        /// destroy the objects
        /// </summary>
        public void remove(int num)
        {
            
        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0) && OnClicked != null)
                OnClicked(0, getMeshOver());

            if (Input.GetMouseButtonDown(2) && OnClicked != null)
                OnClicked(1, getMeshOver());
        }

        private void OnMouseOver()
        {
            if (OnOver != null)
                OnOver(getMeshOver());
        }

        private int getMeshOver()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                //Mesh m = GetComponent<MeshFilter>().sharedMesh;

                float dist = 10000;
                int target = 0;
                Vector2 point = new Vector2(hit.point.x, hit.point.y);
                for (int a = 0; a < center.Count; a++)
                {
                    if (Vector2.Distance(center[a], point) < dist)
                    {
                        dist = Vector2.Distance(center[a], point);
                        target = a;
                    }
                }
                return target;
               /* for (int i = 0; i< m.subMeshCount; i++)
                {
                    var tr = m.GetTriangles(i);
                    for (var j = 0; j < tr.Length; j += 3)
                    {
                        if (tr[j] == m.triangles[hit.triangleIndex * 3] && tr[j + 1] == m.triangles[hit.triangleIndex * 3 +1] && tr[j + 2] == m.triangles[hit.triangleIndex * 3 +2])
                        {
                            Debug.DrawLine(m.vertices[tr[j+1]], m.vertices[tr[j]], Color.red, 10);
                            //Debug.DrawLine(m.vertices[tr[j+1]], m.vertices[tr[j+2]]);
                            //Debug.DrawLine(m.vertices[tr[j+2]], m.vertices[tr[j]]);
                            return i;
                        }
                    }
                }*/


                /*if (m)
                {
                    int[] hittedTriangle = new int[]
                    {
                        m.triangles[hit.triangleIndex * 3],
                        m.triangles[hit.triangleIndex * 3 + 1],
                        m.triangles[hit.triangleIndex * 3 + 2]
                        
                    };
                    for (int i = 0; i < m.subMeshCount; i++)
                    {
                        int[] subMeshTris = m.GetTriangles(i);
                        for (int j = 0; j < subMeshTris.Length; j += 3)
                        {
                            if (subMeshTris[j] == hittedTriangle[0] &&
                                subMeshTris[j + 1] == hittedTriangle[1] &&
                                subMeshTris[j + 2] == hittedTriangle[2])
                            {
                                if (Input.GetMouseButton(0))
                                {
                                    Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 10, Color.yellow, 10);
                                    Debug.DrawLine(m.vertices[hittedTriangle[0]], m.vertices[hittedTriangle[1]], Color.red, 10);
                                    Debug.DrawLine(m.vertices[hittedTriangle[1]], m.vertices[hittedTriangle[2]], Color.red, 10);
                                    Debug.DrawLine(m.vertices[hittedTriangle[0]], m.vertices[hittedTriangle[2]], Color.red, 10);

                                }
                                return i ;
                            }
                        }
                    }
                }*/
            }
            return -1;
        }

        private static Mesh GetMesh(GameObject go)
        {
            if (go)
            {
                MeshFilter mf = go.GetComponent<MeshFilter>();
                if (mf)
                {
                    Mesh m = mf.sharedMesh;
                    if (!m) { m = mf.mesh; }
                    if (m)
                    {
                        return m;
                    }
                }
            }
            return null;
        }
    }
}