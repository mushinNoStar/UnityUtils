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
                    List<Vector2> verts = new List<Vector2>();
                    foreach (IVertex v in area.getOutermostVertices())
                        verts.Add(v.get2dPosition());
                    planeBehaviour.setVertices(verts);
                    planeBehaviour.setMaterial(getRappresentationData().getMaterial());
                }
            }

            public override void hide()
            {
                if (isVisible())
                {
                    planeBehaviour.OnClicked -= clicked;
                    planeBehaviour.OnOver -= over;
                    planeBehaviour.remove();
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
                    planeBehaviour.setVertices(verts);
                }
            }

            private void clicked(int mouseButton)
            {
                Debug.Log("clicked");
            }

            private void over()
            {
                
            }
        }
    }
}