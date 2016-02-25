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

            public PlaneSegmentRappresentation(IPlaneSegment seg, Material mat) : base(mat)
            {
                segment = seg;
            }

            public override void show()
            {
                if (isVisible())
                    return;
                SegmentBehaviour.getSegmentBehaviour().addSegment(segment, getRappresentationData().getMaterial());
                SegmentBehaviour.clicked += SegmentBehaviour_clicked;
                SegmentBehaviour.over += SegmentBehaviour_over;
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
                SegmentBehaviour.getSegmentBehaviour().removeSegment(segment);
            }

            public override bool isVisible()
            {
               return SegmentBehaviour.getSegmentBehaviour().contains(segment);
            }

            private void clicked(int mouseButton)
            {
                Debug.Log("clicked");
            }

            private void over()
            {

            }

            public override void update()
            {
                SegmentBehaviour.getSegmentBehaviour().recalculate();
            }

        }
    }
}
