using UnityEngine;
using System.Collections.Generic;
namespace Game
{
    /// <summary>
    /// connection behaviour is the class that rappresent the segments in the screen
    /// use add segment to create a new visual segment.
    /// use the number that add segment returns to access the segment material. 
    /// </summary>
    public class ConnectionBehaviour : MonoBehaviour
    {
        private static ConnectionBehaviour scb;

        void Start()
        {
            scb = this;
            GetComponent<MeshFilter>().sharedMesh = new Mesh(); //create a new mesh, else it is shared
            GetComponent<MeshFilter>().sharedMesh.triangles = new int[0];//remove every triangle
            GetComponent<MeshFilter>().sharedMesh.vertices = new Vector3[0];//remove every vertex
            GetComponent<MeshFilter>().sharedMesh.subMeshCount = 0;//remove every submesh
        }

        /// <summary>
        /// return the unique connection behaviour
        /// </summary>
        /// <returns></returns>
        public static ConnectionBehaviour getConnectionBehaviour()
        {
            return scb;
        }

        /// <summary>
        /// return the material associated to a particular submesh
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public Material getMaterial(int num)
        {
            return GetComponent<MeshRenderer>().sharedMaterials[num];
        }

        /// <summary>
        /// add a segment to the ones that will be rendered
        /// you must destroy this object if you want to get rid of them
        /// return the number that will be used to set rendering setting
        /// </summary>
        /// <param name="vectors">the points that create the segment</param>
        /// <param name="width">the width of the segment</param>
        /// <returns>the number that will be used to set rendering setting</returns>
        public int addSegment(List<Vector2> vectors, float width = 0.02f)
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.sharedMesh; //find the mesh

            int subMeshTarget = mesh.subMeshCount; //find the number that rappresent the segment
            mesh.subMeshCount += 1; //expand the sub mesh aviable by one

            generateEdges(vectors,subMeshTarget ,width, mesh); //add vertices and triangles to the mesh

            filter.sharedMesh = mesh; //reset the mesh

            MeshRenderer rend = GetComponent<MeshRenderer>();
            List<Material> mat = new List<Material>();
            foreach (Material m in rend.sharedMaterials)
                mat.Add(m);
            mat.Add(new Material(mat[0])); //add a material to the list of materials

            while (mat.Count > subMeshTarget + 1) //if there are too many materials, they will removed
                mat.RemoveAt(0);

            rend.sharedMaterials = mat.ToArray(); //reset the materials
            return subMeshTarget; 
        }

        private void generateEdges(List<Vector2> extremes, int subMesh, float width, Mesh mesh)
        {
            List<Vector3> allVerts = new List<Vector3>(); //the list of every vertex that is in the mesh
            foreach (Vector3 v in mesh.vertices)
                allVerts.Add(v);

            List<int> tris = new List<int>(); //the list that will include every tris
            for (int a = 0; a < extremes.Count - 1; a++)
            {
                tris.AddRange(generateAndAddEdge(extremes[a], extremes[a + 1], width, allVerts));
            }
            tris.AddRange(generateAndAddEdge(extremes[0], extremes[extremes.Count - 1], width, allVerts));

            mesh.SetVertices(allVerts); //set the list of vertices
            mesh.SetTriangles(tris, subMesh); //set the list of triangle
        }

        private List<int> generateAndAddEdge(Vector2 start, Vector2 finish, float width, List<Vector3> allVertsToAdd)
        {
            List<Vector3> diRitorno = generateSegment(start, finish, width); //create a list of points
            List<int> tris = new List<int>();

            tris.Add(allVertsToAdd.Count);
            tris.Add(allVertsToAdd.Count + 1); //first triangle
            tris.Add(allVertsToAdd.Count + 3);

            tris.Add(allVertsToAdd.Count + 1);
            tris.Add(allVertsToAdd.Count + 2); //second triangle
            tris.Add(allVertsToAdd.Count + 3);


            allVertsToAdd.AddRange(diRitorno); //add every vertex of this segment to the list of all verteces
            return tris;
        }

        private List<Vector3> generateSegment(Vector2 start, Vector2 finish, float width)
        {
            List<Vector3> diRitorno = new List<Vector3>(); 

            Vector2 vectorLenght = start - finish;

            Vector2 orthogonalVector = new Vector2(1, 0);
            if (vectorLenght.y == 0) //if the vector has only the y component, it would be dependent from the first one.
                orthogonalVector = new Vector2(0, 1);

            //gram-schmidt.
            orthogonalVector = orthogonalVector - project(orthogonalVector, vectorLenght);
            orthogonalVector = (width / 2) * orthogonalVector.normalized;

            List<Vector2> planePoints = new List<Vector2>();
            planePoints.Add(start - (orthogonalVector));
            planePoints.Add(start + (orthogonalVector));
            planePoints.Add(finish - (orthogonalVector));
            planePoints.Add(finish + (orthogonalVector));
            planePoints = Tools.Utils.sort2d(planePoints);

            foreach (Vector2 v in planePoints)
            {
                Vector3 vec = new Vector3(v.x, v.y, -0.0001f); //move them a bit higher, so the are above the galaxy plane
                diRitorno.Add(vec);
            }
            return diRitorno;
        }

        /// <summary>
        /// linear algebra projection
        /// </summary>
        /// <param name="toProject"></param>
        /// <param name="onTo"></param>
        /// <returns></returns>
        private Vector2 project(Vector2 toProject, Vector2 onTo)
        {
            float v = Vector2.Dot(toProject, onTo);
            float q = Vector2.Dot(onTo, onTo);
            return (v / q) * onTo;
        }


    }
}
