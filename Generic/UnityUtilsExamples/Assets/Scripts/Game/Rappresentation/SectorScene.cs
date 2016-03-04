using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    public class SectorScene : Scene
    {
        public static SectorVisualization sectorInspected = null;

        /// <summary>
        /// set invisble every sector and inbetween sector connections
        /// </summary>
        public override void OnEnd()
        {
            foreach (SubSpaceVisualization sc in sectorInspected.subSectorVisualization)
                sc.hide();
            //sectorInspected.hide();
            
        }

        /// <summary>
        /// set visible every sector and between sectors connections.
        /// </summary>
        public override void OnStart()
        {
            foreach (SubSpaceVisualization sc in sectorInspected.subSectorVisualization)
                sc.show();
            //sectorInspected.show();

            
        }

        public override void tick()
        {
            if (Camera.main.transform.position.z < -1.5f)
                Game.getGame().getRappresentation().setScene(Scene.getScene("GalaxyScene"));

        }
    }

}
