using UnityEngine;
using System.Collections.Generic;

namespace Geometry
{
    namespace Internal
    {
        internal class NameSpaceTest : MonoBehaviour
        {
            // Use this for initialization
            void Start()
            {
                List<Vector2> vertices = new List<Vector2>();
                for (int a = 0; a < 15; a++)
                {
                    vertices.Add(new Vector2(Random.value * 10, Random.value * 10));
                    
                }

                PlaneBehaviour b = PlaneBehaviour.loadOne();
                b.setVertices(vertices);
                b.transform.position = new Vector3(10,0,0);

                List<IVertex> tvertices = new List<IVertex>();
                foreach (Vector2 v in vertices)
                    tvertices.Add(new TestVertex(v));
                PlaneArea plnArea = new PlaneArea(tvertices);
                plnArea.show();

                foreach (IVertex v in plnArea.getVertices())
                {
                    GameObject gm = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    gm.transform.position = new Vector3(v.get2dPosition().x, v.get2dPosition().y, 0);
                    gm.GetComponent<Renderer>().material.color = Color.red;
                    gm.name = plnArea.getVertices().IndexOf(v) + "";
                }
            }

            // Update is called once per frame
            void Update()
            {

            }
        }
    }

    internal class TestVertex : IVertex
    {
        private Vector2 ps;
        public TestVertex(Vector2 pos)
        {
            ps = pos;
        }

        public Vector2 get2dPosition()
        {
            return ps;
        }

        public float getSlope(IVertex v)
        {
            return (get2dPosition().y - v.get2dPosition().y) / (get2dPosition().x - v.get2dPosition().x);
        }

    }
}

