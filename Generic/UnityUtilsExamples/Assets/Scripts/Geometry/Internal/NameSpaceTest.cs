using UnityEngine;
using System.Collections.Generic;
using System;

namespace Geometry
{
    namespace Internal
    {
        internal class NameSpaceTest : MonoBehaviour
        {
            PointedArea area;
            // Use this for initialization
            void Start()
            {
                //PointedAreaTest();
                Game.Game gm = Game.Game.getGame();
                gm.start("");
            }

            public void Update()
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    
                    foreach (OutlinedConvexArea r in area.getAreas())
                    {
                        r.getArea().hide();
                    }

                    

                }
                if (Input.GetKeyDown(KeyCode.Q))
                {

                    foreach (OutlinedConvexArea r in area.getAreas())
                    {
                        r.getArea().show();
                    }


                }

                if (Input.GetKeyDown(KeyCode.G))
                {

                    foreach (OutlinedConvexArea r in area.getAreas())
                    {
                        foreach (PlaneSegment sgm in r.getSegments())
                            sgm.hide();
                    }


                }

                if (Input.GetKeyDown(KeyCode.H))
                {

                    foreach (OutlinedConvexArea r in area.getAreas())
                    {
                        foreach (PlaneSegment sgm in r.getSegments())
                            sgm.show();
                    }


                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    foreach (PlaneSegment sg in area.getVertexConnections())
                    {
                        sg.hide();
                    }
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    foreach (PlaneSegment sg in area.getVertexConnections())
                    {
                        sg.show();
                    }
                }
            }

            private void PointedAreaTest()
            {
                float size = 10;
                List<IVertex> tvertices = new List<IVertex>();
                List<IVertex> bounds = new List<IVertex>();
                for (int a = 0; a < 200; a++)
                {
                    tvertices.Add(new TestVertex(new Vector2((UnityEngine.Random.value * 2 * size) - size, (UnityEngine.Random.value * 2 * size) - size)));
                }
                bounds.Add(new TestVertex(3f*size, 3f*size));
                bounds.Add(new TestVertex(-3f*size, 3f*size));
                bounds.Add(new TestVertex(-3f*size, -3f*size));
                bounds.Add(new TestVertex(3f*size, -3f*size));


                Material m1 = new Material(Resources.Load<Material>("testMat"));
                Material m2 = new Material(Resources.Load<Material>("testMat"));
                Material m3 = new Material(Resources.Load<Material>("testMat"));
                Material m4 = new Material(Resources.Load<Material>("testMat"));
                Material m5 = new Material(Resources.Load<Material>("testMat"));


                m2.color = Color.white;
                m3.color = Color.black;
                m4.color = Color.clear;
                m5.color = Color.clear;

                //area = new PointedArea(tvertices, 0.03f, m1, m2, m3, bounds, m4, m5, 0.02f);
                //area.show();

                List<Material> mat = new List<Material>();
                mat.Add(m1);
                mat.Add(m1);
                mat.Add(m3);
                mat.Add(m4);
                mat.Add(m5);

                Galaxy.GalaxyGenerationParameter param = new Galaxy.GalaxyGenerationParameter(mat);
                Galaxy.Galaxy gal = new Galaxy.Galaxy();
                gal.generate(param);

                area = gal.galRapp;

               // PlaneArea pln = new PlaneArea(tvertices, m1);
                //pln.show();
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

        public class TestVertex : IVertex
        {
            private Vector2 ps;
            public TestVertex(Vector2 pos)
            {
                ps = pos;
               /* GameObject gm = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                gm.transform.position = new Vector3(pos.x, pos.y, 0);
                gm.GetComponent<Renderer>().material.color = Color.red;
                gm.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);*/

            }

            public TestVertex(float x, float y)
            {
                ps = new Vector2(x, y);
            }

            public Vector2 get2dPosition()
            {
                return ps;
            }

        }
    }

    
}

