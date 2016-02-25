using UnityEngine;
using System.Collections.Generic;
using System;

namespace Geometry
{
    namespace Internal
    {
        internal class NameSpaceTest : MonoBehaviour
        {
            // Use this for initialization
            void Start()
            {
                fullTest();
            }

            private void fullTest()
            {
                List<IVertex> tvertices = new List<IVertex>();
                for (int a = 0; a < 50; a++)
                {
                    tvertices.Add(new TestVertex(new Vector2(UnityEngine.Random.value * 10 - 5, UnityEngine.Random.value * 10 - 5)));
                }

                Material m1 = new Material(Resources.Load<Material>("testMat"));
                Material m2 = new Material(Resources.Load<Material>("testMat"));
                m1.color = Color.red;
                m2.color = Color.white;

                OutlinedConvexArea plnArea = new OutlinedConvexArea(tvertices, m1, m2, 0.2f);
                plnArea.show();
                plnArea.hide();
                plnArea.show();

            

            }

        }
    }

    internal class TestVertex : IVertex
    {
        private Vector2 ps;
        public TestVertex(Vector2 pos)
        {
            ps = pos;
            GameObject gm = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gm.transform.position = new Vector3(pos.x, pos.y, 0);
            gm.GetComponent<Renderer>().material.color = Color.red;
            
        }

        public TestVertex(float x, float y)
        {
            ps = new Vector2(x,y);
        }

        public Vector2 get2dPosition()
        {
            return ps;
        }

    }
}

