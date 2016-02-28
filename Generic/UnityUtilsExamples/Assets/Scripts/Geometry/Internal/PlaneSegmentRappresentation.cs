using UnityEngine;
using Rappresentation;
using System;

namespace Geometry
{
    namespace Internal
    {
        public class PlaneSegmentRappresentation : RappresentatioObject
        {
            private IPlaneSegment segment;
            private SegmentBehaviour segBehavipur = null;

            public PlaneSegmentRappresentation(IPlaneSegment seg, Material mat) : base(mat)
            {
                segment = seg;
            }

         

            public override void show()
            {
                if (isVisible())
                    return;
                segBehavipur = SegmentBehaviour.getSegmentBehaviour();
                segBehavipur.addSegment(segment, getRappresentationData().getMaterial());

                SegmentBehaviour.clicked += SegmentBehaviour_clicked;
                SegmentBehaviour.over += SegmentBehaviour_over;
                segBehavipur.setMaterial(segment, getRappresentationData().getMaterial());
            }

            private void SegmentBehaviour_over(IPlaneSegment segme)
            {
                if (segme == segment)
                    over();
            }

            private void SegmentBehaviour_clicked(IPlaneSegment segme, int mouseButton)
            {
                if (segme == segment)
                    clicked(mouseButton);
            }

            public override void hide()
            {
                if (!isVisible())
                    return;
                SegmentBehaviour.clicked -= SegmentBehaviour_clicked;
                SegmentBehaviour.over -= SegmentBehaviour_over;
                Material m = new Material(getRappresentationData().getMaterial());
                m.color = Color.clear;
                segBehavipur.setMaterial(segment, m);
                segBehavipur = null;
            }

            public override bool isVisible()
            {
               return (segBehavipur != null);
            }

            /*private void clicked(int mouseButton)
            {
                Debug.Log("clicked");
            }

            private void over()
            {

            }*/

            public override void update()
            {
                SegmentBehaviour.getSegmentBehaviour().recalculate();
            }

        }
    }
}
