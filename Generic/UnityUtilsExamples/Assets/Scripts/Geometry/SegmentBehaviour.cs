using UnityEngine;
using System.Collections.Generic;

namespace Geometry
{
    public class SegmentBehaviour : MonoBehaviour
    {
        public static event extendedSegmentMethod clicked;
        public static event segmentMethod over;

        private static SegmentBehaviour everySegment = null;
        private List<IPlaneSegment> segments = new List<IPlaneSegment>();
        private List<Material> materials = new List<Material>();

        public static SegmentBehaviour getSegmentBehaviour()
        {
            if (everySegment == null)
            {
                GameObject gm = Instantiate(Resources.Load<GameObject>("SegmentPrefab"));
                everySegment = gm.GetComponent<SegmentBehaviour>();

                Mesh currentMesh = new Mesh();
                gm.GetComponent<MeshCollider>().sharedMesh = currentMesh;
                gm.GetComponent<MeshFilter>().sharedMesh = currentMesh;
            }
            return everySegment;
        }

        public bool contains(IPlaneSegment seg)
        {
            return segments.Contains(seg);
        }

        /// <summary>
        /// if you add an already present vertex, it is ignored
        /// </summary>
        /// <param name="segment"></param>
        public void addSegment(IPlaneSegment segment, Material mat)
        {
            if (segments.Contains(segment))
                return;
            segments.Add(segment);
            materials.Add(mat);
            recalculate();
        }

        /// <summary>
        /// if you add an already present vertex, it is ignored
        /// </summary>
        /// <param name="segment"></param>
        public void addSegments(List<IPlaneSegment> segs, List<Material> mats)
        {
            if (mats.Count != mats.Count)
                throw new System.ArgumentException("too many or too few materials");

            for (int a = segs.Count; a > 0; a--)
            {
                if (!segments.Contains(segs[a]))
                {
                    segments.Add(segs[a]);
                    materials.Add(mats[a]);
                }
            }
            recalculate();
        }

        /// <summary>
        /// return the material of a specific segment
        /// if there is no such sement, it return null
        /// </summary>
        /// <param name="seg"></param>
        public Material getMaterial(IPlaneSegment seg)
        {
            if (segments.Contains(seg))
            {
                return materials[segments.IndexOf(seg)];
            }
            return null;
        }

        /// <summary>
        /// set the material of a specified segment.
        /// </summary>
        /// <param name="seg"></param>
        /// <param name="mat"></param>
        public void setMaterial(IPlaneSegment seg, Material mat)
        {
            if (segments.Contains(seg))
            {
                materials.Insert(segments.IndexOf(seg), mat);
                materials.RemoveAt(segments.IndexOf(seg)+1);
            }
            GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
        }
    
        /// <summary>
        /// don't use this too often.
        /// </summary>
        public void recalculate()
        {
            Mesh currentMesh = GetComponent<MeshFilter>().sharedMesh;
            List<Vector3> vertex = new List<Vector3>();
            foreach (Vector3 v in currentMesh.vertices)
                vertex.Add(v);
            currentMesh.subMeshCount = segments.Count;
        
            int subMeshPosition = segments.Count - 1;
                addTrisToList(currentMesh, segments[segments.Count-1], vertex, subMeshPosition);
            

            currentMesh.SetVertices(vertex);
           
                int[] triangles = new int[6];
                triangles[0] = 0 + (subMeshPosition * 4);
                triangles[1] = 1 + (subMeshPosition * 4);
                triangles[2] = 2 + (subMeshPosition * 4);
                triangles[3] = 0 + (subMeshPosition * 4);
                triangles[4] = 2 + (subMeshPosition * 4);
                triangles[5] = 3 + (subMeshPosition * 4);

                currentMesh.SetTriangles(triangles, subMeshPosition);
         
            currentMesh.name = "new mesh";
            currentMesh.RecalculateNormals();

            GetComponent<MeshCollider>().sharedMesh = currentMesh;
            GetComponent<MeshFilter>().sharedMesh = currentMesh;

            GetComponent<MeshRenderer>().sharedMaterials = materials.ToArray();
            
        }

        private void addTrisToList(Mesh mesh, IPlaneSegment seg,List<Vector3> verts,int subMeshPosition)
        {
            Vector2 vectorLenght = seg.getEndingPoint().get2dPosition() - seg.getStartingPoint().get2dPosition();
            if (vectorLenght.magnitude == 0) //no reason to rappresent a zero lenght vector.
                return; // it would ruin my math as well.

            Vector2 orthogonalVector = new Vector2(1,0);
            if (vectorLenght.y == 0) //if the vector has only the y component, it would be dependent from the first one.
                orthogonalVector = new Vector2(0,1);

            //gram-schmidt.
            orthogonalVector = orthogonalVector - project(orthogonalVector, vectorLenght);
            orthogonalVector = (seg.getWidth() / 2) * orthogonalVector.normalized;

            List<Vector2> planePoints = new List<Vector2>();
            planePoints.Add(seg.getStartingPoint().get2dPosition() - (orthogonalVector));
            planePoints.Add(seg.getStartingPoint().get2dPosition() + ((1 - seg.getOffset()) * orthogonalVector));
            planePoints.Add(seg.getEndingPoint().get2dPosition() - ( orthogonalVector));
            planePoints.Add(seg.getEndingPoint().get2dPosition() + ((1 - seg.getOffset()) * orthogonalVector));
            planePoints = Utils.sort2d(planePoints);

            foreach (Vector2 v in planePoints)
            {
                Vector3 vec = new Vector3(v.x, v.y, -0.0001f);
                verts.Add(vec);
            }

            
        }

        private Vector2 project(Vector2 toProject, Vector2 onTo)
        {
            float v = Vector2.Dot(toProject, onTo);
            float q = Vector2.Dot(onTo, onTo);
            return (v / q) * onTo;
        }

        private void OnMouseDown()
        {
            int m = 0;
            if (Input.GetMouseButtonDown(2))
                m = 2;
            int t = getMeshOver();
            if (t >= 0 && clicked != null)
                clicked(segments[t], m);
        }

        private void OnMouseOver()
        {
            
            int t = getMeshOver();
            if (t >= 0 && clicked != null)
                over(segments[t]);
        }

        private int getMeshOver()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Mesh m = GetMesh(gameObject);
                if (m)
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
                                return i;
                            }
                        }
                    }
                }
            }
            return -1;
        }

        static Mesh GetMesh(GameObject go)
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