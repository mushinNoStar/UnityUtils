using InputHandler;

namespace Plane
{
    namespace Internal
    {
        /// <summary>
        /// This is the communication system between unity and the abstract plane area.
        /// 
        /// This is at the player level of abstraction, this class decide what the player actually see,
        /// and what the player my want to do when he interact with the rappresentation.
        /// </summary>
        public class PlaneAreaRappresentation : ClickableInputReciver
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
                    planeBehaviour.setVertices(area.getOutermostVertices());
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
                    planeBehaviour.setVertices(area.getOutermostVertices());
            }

            public void clicked(int mouseButton)
            {

            }

            public void over()
            {

            }
        }
    }
}