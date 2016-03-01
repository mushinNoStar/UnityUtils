using UnityEngine;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace Game
{
    public class SelectedBehaviour : MonoBehaviour {

        private static List<SectorVisualization> selectedSector = new List<SectorVisualization>();
        public static float width = 0.02f;
        private static SelectedBehaviour sl;

        // Use this for initialization
        void Start() {
            sl = this;
            GetComponent<MeshFilter>().sharedMesh = new Mesh(); //create a new mesh, else it is shared
            GetComponent<MeshFilter>().sharedMesh.triangles = new int[0];//remove every triangle
            GetComponent<MeshFilter>().sharedMesh.vertices = new Vector3[0];//remove every vertex
            GetComponent<MeshFilter>().sharedMesh.subMeshCount = 0;//remove every submesh
        }

        public static SelectedBehaviour getSelectedBehaviour()
        {
            return sl;
        }


        public void addSelectedSector(SectorVisualization target)
        {
            if (!selectedSector.Contains(target))
                selectedSector.Add(target);
            update();
        }

        public void removeSelectedSector(SectorVisualization target)
        {
            if (selectedSector.Contains(target))
                selectedSector.Remove(target);
            update();
        }

        public void update()
        {
            GetComponent<MeshFilter>().sharedMesh = new Mesh(); //create a new mesh, else it is shared
            GetComponent<MeshFilter>().sharedMesh.triangles = new int[0];//remove every triangle
            GetComponent<MeshFilter>().sharedMesh.vertices = new Vector3[0];//remove every vertex
            GetComponent<MeshFilter>().sharedMesh.subMeshCount = 0;//remove every submesh

            List<Vector3> vec = new List<Vector3>();
            List<int> tris = new List<int>();

            foreach (SectorVisualization t in selectedSector)
            {
                generateEdges(t.getExtremes(), vec, tris);
            }
            Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
            mesh.SetVertices(vec);
            mesh.triangles = tris.ToArray();
        }

        private void generateEdges(ReadOnlyCollection<Vector2> extremes, List<Vector3> allVerts, List<int> tris)
        {
            for (int a = 0; a < extremes.Count - 1; a++)
            {
                tris.AddRange(generateAndAddEdge(extremes[a], extremes[a + 1], width, allVerts));
            }
            tris.AddRange(generateAndAddEdge(extremes[0], extremes[extremes.Count - 1], width, allVerts));

            
        }

        private List<int> generateAndAddEdge(Vector2 start, Vector2 finish, float width, List<Vector3> allVertsToAdd)
        {
            List<Vector3> diRitorno = generateSegment(start, finish);
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

        private List<Vector3> generateSegment(Vector2 start, Vector2 finish)
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