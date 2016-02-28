using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Geometry.Internal;
using Geometry;
using ceometric.DelaunayTriangulator;

namespace Graph
{
    public class GraphTest : MonoBehaviour
    {
        public Graph<IVertex> graph;
        public PlaneArea area = null;
        int num = 0;
        // Use this for initialization
        void Start()
        {
            List<IVertex> list = new List<IVertex>();
            List<int> connections = new List<int>();
            makePlaneGraph(list, connections);
            /*
            for (int a = 0; a < 50; a++)
            {
                connections.Add((int)(Random.value * 20));
                while (a !=0 && connections[a - 1] == connections[a])
                    connections[a] = (int)(Random.value * 20);
            }*/

            Debug.Log(connections.Count +" vertici connessi");
            graph = new Graph<IVertex>(list, connections);
             foreach (GraphEdge<IVertex> edge in graph.edges)
            {
                PlaneSegment pls = new PlaneSegment(edge.extremes[0].element, edge.extremes[1].element, 0.2f, new Material(Resources.Load<Material>("testMat")));
                pls.show();
            }


        }

        public void makePlaneGraph(List<IVertex> list, List<int> connections)
        {
            
            for (int a = 0; a < 50; a++)
                list.Add(new TestVertex(Random.value * 20 - 10, Random.value * 20 - 10));
            List<Point> points = new List<Point>();
            foreach (IVertex v in list)
                points.Add(new Point(v.get2dPosition().x, v.get2dPosition().y, 0));
            DelaunayTriangulation2d triang = new DelaunayTriangulation2d();
            List<Triangle> tris = triang.Triangulate(points);

            foreach (Triangle t in tris)
            {
                connections.Add(points.IndexOf(t.Vertex1));
                connections.Add(points.IndexOf(t.Vertex2));
                connections.Add(points.IndexOf(t.Vertex2));
                connections.Add(points.IndexOf(t.Vertex3));
                connections.Add(points.IndexOf(t.Vertex3));
                connections.Add(points.IndexOf(t.Vertex1));

            }
            /*for (int a = 0; a < list.Count; a++)
            {
                for (int b = a + 1; b < list.Count; b++)
                {
                    float f = list[a].get2dPosition().x - list[b].get2dPosition().x;
                    f = Mathf.Abs(f);
                    f -= Mathf.Abs(list[a].get2dPosition().y - list[b].get2dPosition().y);
                    f = Mathf.Abs(f);
                    if (f < 0.4f)
                    {
                        list.RemoveAt(b);
                        b--;
                    }
                }
            }
            for (int a = 0; a < list.Count; a++)
            {
                for (int b = a+1; b < list.Count; b++)
                {
                    
                    if (Vector2.Distance(list[a].get2dPosition(), list[b].get2dPosition()) < 4)
                    {
                        connections.Add(a);
                        connections.Add(b);
                    }
                }
            }
            */
            for (int a = 0; a < connections.Count; a += 2)
            {
                for (int b = a + 2; b < connections.Count; b += 2)
                {
                    if ((connections[a] == connections[b] && connections[a + 1] == connections[b + 1])
                        || (connections[a + 1] == connections[b] && connections[a] == connections[b + 1]))
                    {
                        Debug.Log("Discarded one " + a);
                        connections.RemoveAt(a);
                        connections.RemoveAt(a);
                        a -= 2;
                        b -= 2;
                        break;
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.N) && graph.cycles.Count>0)
            {
                if (area != null)
                    area.hide();
                GraphCycle<IVertex> f = graph.cycles[num];
                num++;
                if (num >= graph.cycles.Count)
                    num = 0;
                List<IVertex> ls = new List<IVertex>();
                foreach (GraphElement<IVertex> el in f.elements)
                    ls.Add(el.element);
                area = new PlaneArea(ls, Resources.Load<Material>("testMat"));
                area.show();
            }

       
        }
    }
}