using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoPointBeaviour : MonoBehaviour {

    public Color color;
    public Vector2 extreme1;
    public Vector2 extreme2;
    public float size = 0;

    // Use this for initialization
    void Start () {
        List<Vector2> extremes = new List<Vector2>();
        extremes.Add(extreme1);
        extremes.Add(extreme2);
        Mesh m = new Mesh();
        generateEdges(extremes, size, m);
        Material mat = new Material(GetComponent<MeshRenderer>().sharedMaterial);
        GetComponent<MeshFilter>().sharedMesh = m;
        GetComponent<MeshRenderer>().sharedMaterial = mat;
        GetComponent<MeshRenderer>().sharedMaterial.color = color;
        GetComponent<MeshCollider>().sharedMesh = m;
        m.RecalculateNormals();
    }

    // Update is called once per frame
    void Update () {
        GetComponent<MeshRenderer>().sharedMaterial.color = color;

    }

    private void generateEdges(List<Vector2> extremes, float width, Mesh mesh)
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
        mesh.triangles = (tris.ToArray()); //set the list of triangle
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
