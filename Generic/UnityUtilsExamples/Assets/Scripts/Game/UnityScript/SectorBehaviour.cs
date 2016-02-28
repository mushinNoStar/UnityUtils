using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Game
{
    public delegate void intMethod(int num);
    public class SectorBehaviour : MonoBehaviour
    {
        public event intMethod OnClicked;
        private static SectorBehaviour scb;
        private List<List<Vector2>> listOfSectorsExtremes = new List<List<Vector2>>();
        private List<Vector2> listOfCenters = new List<Vector2>();
        private List<Vector2> voronoiPoints = new List<Vector2>();

        // Use this for initialization
        void Start()
        {
            scb = this;
            GetComponent<MeshFilter>().sharedMesh.triangles = new int[0];
            GetComponent<MeshFilter>().sharedMesh.vertices = new Vector3[0];
            GetComponent<MeshFilter>().sharedMesh.subMeshCount = 0;
        }

        public static SectorBehaviour getSectorBehaviour()
        {
            return scb;
        }

        public Material getAreaMaterial(int target)
        {
            return GetComponent<Renderer>().materials[target*2];
        }

        public Material getBorderMaterial(int target)
        {
            return GetComponent<Renderer>().materials[target * 2 + 1];
        }

        // Update is called once per frame
        void Update()
        {
        }

        public int addSector(List<Vector2> extremes, Vector2 voronoiPoint ,float width = 0.02f)
        {
            listOfSectorsExtremes.Add(extremes);
            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.sharedMesh;

            voronoiPoints.Add(voronoiPoint);
            int subMeshTarget = mesh.subMeshCount;
            mesh.subMeshCount += 2;
            generateMainArea(extremes, subMeshTarget);
            generateEdges(extremes, subMeshTarget + 1, width, mesh);

            filter.sharedMesh = mesh;
            recalculateNormalAndUv();

            MeshRenderer rend = GetComponent<MeshRenderer>();
            List<Material> mat = new List<Material>();
            foreach (Material m in rend.sharedMaterials)
                mat.Add(m);
            mat.Add(new Material(mat[0]));
            mat.Add(new Material(mat[0]));

            while (mat.Count > subMeshTarget + 2)
                mat.RemoveAt(0);

            rend.sharedMaterials = mat.ToArray();
            return subMeshTarget/2;
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

        private void generateMainArea(List<Vector2> extremes, int subMesh)
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.sharedMesh;
            Vector2 center = Vector2.zero;
            foreach (Vector2 v in extremes)
            {
                center += v;
            }
            center = center / extremes.Count;
            listOfCenters.Add(center);

            int currentNumberOfVertex = mesh.vertexCount;
            List<int> tris = getTris(extremes, center, currentNumberOfVertex + 1); //plus one because the center will go first.
            List<Vector3> vertices = new List<Vector3>();

            foreach (Vector3 v in mesh.vertices)
                vertices.Add(v);
            vertices.Add(center);
            foreach (Vector3 ext in extremes)
                vertices.Add(ext);

            mesh.SetVertices(vertices);
            mesh.SetTriangles(tris.ToArray(), subMesh);


        }

        private List<int> getTris(List<Vector2> extremes, Vector2 center, int currNumberOfVertex)
        {
            List<int> diRItorno = new List<int>();
            for (int a = 0; a < extremes.Count - 1; a++)
            {
                diRItorno.Add(a + currNumberOfVertex);
                diRItorno.Add(a + 1 + currNumberOfVertex);
                diRItorno.Add(currNumberOfVertex - 1);
            }
            diRItorno.Add(extremes.Count - 1 + currNumberOfVertex);
            diRItorno.Add(currNumberOfVertex);
            diRItorno.Add(currNumberOfVertex - 1);
            return diRItorno;
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

        private Vector2 project(Vector2 toProject, Vector2 onTo)
        {
            float v = Vector2.Dot(toProject, onTo);
            float q = Vector2.Dot(onTo, onTo);
            return (v / q) * onTo;
        }

        void OnMouseDown()
        {
            if (OnClicked != null)
                OnClicked(getMeshOver());
        }

        private int getMeshOver()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {

                float dist = 10000;
                int target = 0;
                Vector2 point = new Vector2(hit.point.x, hit.point.y);
                for (int a = 0; a < voronoiPoints.Count; a++)
                {
                    if (Vector2.Distance(voronoiPoints[a], point) < dist)
                    {
                        dist = Vector2.Distance(voronoiPoints[a], point);
                        target = a;
                    }
                }
                return target;

            }
            return -1;
        }
    }
}