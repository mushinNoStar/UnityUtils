using UnityEngine;
using System.Collections.Generic;
using Rappresentation;
using System;

namespace Geometry
{
    namespace Internal
    {
        /// <summary>
        /// This is the communication system between unity and the abstract plane area.
        /// 
        /// This is at the player level of abstraction, this class decide what the player actually see,
        /// and what the player my want to do when he interact with the actuall game object.
        /// </summary>
        public class PlaneAreaRappresentation : RappresentatioObject
        {
            private PlaneArea area;
            private PlaneBehaviour planeBehaviour = null;
            private int num = -1;

            public PlaneAreaRappresentation(PlaneArea planeArea, Material mat) : base(mat)
            {
                area = planeArea;
            }

            public override void show()
            {
                if (!isVisible())
                {
                    planeBehaviour = PlaneBehaviour.loadOne();
                    planeBehaviour.OnClicked += clicked;
                    planeBehaviour.OnOver += over;

                    if (num == -1)
                    {
                        List<Vector2> verts = new List<Vector2>();
                        foreach (IVertex v in area.getOutermostVertices())
                            verts.Add(v.get2dPosition());
                        num = planeBehaviour.getMyNumber(this);
                        planeBehaviour.setVertices(verts, num);
                        planeBehaviour.setMaterial(getRappresentationData().getMaterial(), num);
                    }
                    else
                        planeBehaviour.setMaterial(getRappresentationData().getMaterial(), num);
                }
            }

            public override void hide()
            {
                if (isVisible())
                {
                    planeBehaviour.OnClicked -= clicked;
                    planeBehaviour.OnOver -= over;

                    //planeBehaviour.remove(num);
                    Material m = new Material(getRappresentationData().getMaterial());
                   
                    m.color = Color.clear;
                    planeBehaviour.setMaterial(m, num);
                    planeBehaviour = null;
                }
            }

            public override bool isVisible()
            {
                return (planeBehaviour != null);
            }

            public override void update()
            {
                if (isVisible())
                {
                    List<Vector2> verts = new List<Vector2>();
                    foreach (IVertex v in area.getOutermostVertices())
                        verts.Add(v.get2dPosition());
                    int num = planeBehaviour.getMyNumber(this);

                    planeBehaviour.remove(num);
                    planeBehaviour.setVertices(verts, num);
                }
            }

            private void clicked(int mouseButton, int target)
            {

                if (target == num)
                    clicked(mouseButton);
            }

            private void over(int target)
            {
                if (target == num)
                    over();
            }

            
        }
    }
}