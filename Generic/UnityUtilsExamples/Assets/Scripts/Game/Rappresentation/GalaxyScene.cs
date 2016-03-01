using UnityEngine;
using System.Collections;
using System;

namespace Game
{
    public class GalaxyScene : Scene
    {
        /// <summary>
        /// set invisble every sector and inbetween sector connections
        /// </summary>
        public override void OnEnd()
        {
            foreach (SectorVisualization vis in SectorVisualization.getVisualizations())
                vis.hide();

            foreach (ConnectionVisualization con in ConnectionVisualization.getVisualization())
                con.hide();
        }

        /// <summary>
        /// set visible every sector and between sectors connections.
        /// </summary>
        public override void OnStart()
        {
            foreach (SectorVisualization vis in SectorVisualization.getVisualizations())
                vis.show();
            foreach (ConnectionVisualization con in ConnectionVisualization.getVisualization())
                con.show();
        }

        public override void tick()
        {
            
        }
    }
}