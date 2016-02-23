using UnityEngine;
using System.Collections.Generic;

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
        public class PlaneAreaRappresentation
        {
            private PlaneArea area;
            private PlaneBehaviour planeBehaviour = null;

            public PlaneAreaRappresentation(PlaneArea planeArea)
            {
                area = planeArea;
            }

            public void show()
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
                }
            }

            public void hide()
            {
                if (isVisible())
                {
                    planeBehaviour.OnClicked -= clicked;
                    planeBehaviour.OnOver -= over;
                    planeBehaviour.remove();
                    planeBehaviour = null;
                }
            }

            public bool isVisible()
            {
                return (planeBehaviour != null);
            }

            public void update()
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