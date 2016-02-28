using UnityEngine;
using System.Collections.Generic;
namespace Game
{
    public class ConnectionBehaviour : MonoBehaviour
    {
        private static ConnectionBehaviour scb;

        // Use this for initialization
        void Start()
        {
            scb = this;
            GetComponent<MeshFilter>().sharedMesh = new Mesh();
            GetComponent<MeshFilter>().sharedMesh.triangles = new int[0];
            GetComponent<MeshFilter>().sharedMesh.vertices = new Vector3[0];
            GetComponent<MeshFilter>().sharedMesh.subMeshCount = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static ConnectionBehaviour getConnectionBehaviour()
        {
            return scb;
        }

        public Material getMaterial(int num)
        {
            return GetComponent<MeshRenderer>().sharedMaterials[num];
        }

        public int addSegment(List<Vector2> vectors, float width = 0.02f)
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.sharedMesh;

            int subMeshTarget = mesh.subMeshCount;
            mesh.subMeshCount += 1;

            generateEdges(vectors,subMeshTarget ,width, mesh);

            filter.sharedMesh = mesh;

            MeshRenderer rend = GetComponent<MeshRenderer>();
            List<Material> mat = new List<Material>();
            foreach (Material m in rend.sharedMaterials)
                mat.Add(m);
            mat.Add(new Material(mat[0]));

            while (mat.Count > subMeshTarget + 1)
                mat.RemoveAt(0);

            rend.sharedMaterials = mat.ToArray();
            return subMeshTarget;
        }

        private void generateEdges(List<Vector2> extremes, int subMesh, float width, Mesh mesh)
        {
            List<Vector3> allVerts = new List<Vector3>();
            foreach (Vector3 v in mesh.vertices)
                allVerts.Add(v);
            List<int> tris = new List<int>();
            for (int a = 0; a < extremes.Count - 1; a++)
            {
                tris.AddRange(generateAndAddEdge(extremes[a], extremes[a + 1], width, allVerts));
            }
            tris.AddRange(generateAndAddEdge(extremes[0], extremes[extremes.Count - 1], width, allVerts));
            mesh.SetVertices(allVerts);
            mesh.SetTriangles(tris, subMesh);
        }

        private List<int> generateAndAddEdge(Vector2 start, Vector2 finish, float width, List<Vector3> allVertsToAdd)
        {
            List<Vector3> diRitorno = generateSegment(start, finish, width);
            List<int> tris = new List<int>();
            tris.Add(allVertsToAdd.Count);
            tris.Add(allVertsToAdd.Count + 1);
            tris.Add(allVertsToAdd.Count + 3);

            tris.Add(allVertsToAdd.Count + 1);
            tris.Add(allVertsToAdd.Count + 2);
            tris.Add(allVertsToAdd.Count + 3);


            allVertsToAdd.AddRange(diRitorno);
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
                Vector3 vec = new Vector3(v.x, v.y, -0.0001f);
                diRitorno.Add(vec);
            }
            return diRitorno;
        }

        private Vector2 project(Vector2 toProject, Vector2 onTo)
        {
            float v = Vector2.Dot(toProject, onTo);
            float q = Vector2.Dot(onTo, onTo);
            return (v / q) * onTo;
        }


    }
}
