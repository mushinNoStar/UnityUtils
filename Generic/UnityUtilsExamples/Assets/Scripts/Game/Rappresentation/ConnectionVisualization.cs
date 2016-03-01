using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// this class should handle what the player see of a connection, what the player is trying to do when clicking around
    /// </summary>
    public class ConnectionVisualization : Visualization
    {
        public readonly Connection connection;
        private static List<ConnectionVisualization> vis = new List<ConnectionVisualization>();
        private int myNumber = -1;
        public readonly List<Vector2> extremes;
        public bool visible = true;
        public ConnectionClone lastIntel;
        public ConnectionClone previousIntel = null;

        public ConnectionVisualization(Connection con, Player pl) : base ()
        {
            extremes = new List<Vector2>();
            extremes.Add(con.sectors[0].get2dPosition());
            extremes.Add(con.sectors[1].get2dPosition());
            connection = con;
            vis.Add(this);
            myNumber = ConnectionBehaviour.getConnectionBehaviour().addSegment(extremes);
            lastIntel = new ConnectionClone(con, pl);
            setIntel();
        }

        public static List<ConnectionVisualization> getVisualization()
        {
            return vis;
        }

        /// <summary>
        /// this sets the visualization invisble. this has nothing to do with the visibility rules
        /// this is related to the scene managment.
        /// </summary>
        public override void hide()
        {
            visible = false;
            ConnectionBehaviour.getConnectionBehaviour().getMaterial(myNumber).color = (Color.clear);
        }

        /// <summary>
        /// return if the connection is visible. this is related to the scene managment
        /// you should check in the Connection class if you want to see if the players are allowed to see it.
        /// </summary>
        public override bool isVisible()
        {
            return visible;
        }

        /// <summary>
        /// this sets the visualization visible. this has nothing to do with the visibility rules
        /// this is related to the scene managment.
        /// </summary>
        public override void show()
        {
            visible = true;
            setIntel();
        }

        public override void update()
        {
            previousIntel = lastIntel;
            lastIntel = new ConnectionClone(connection, Game.getGame().getRappresentation().getObservingPlayer());

            setIntel();
        }

        public void setIntel()
        {
            if (previousIntel !=null && !lastIntel.isKnow && !previousIntel.isKnow)
                return;
            if (!lastIntel.isKnow)
            {
                ConnectionBehaviour.getConnectionBehaviour().getMaterial(myNumber).color = Color.clear;
                return;
            }
            ConnectionBehaviour.getConnectionBehaviour().getMaterial(myNumber).color = lastIntel.color;
            setVisualizationDelay(lastIntel.delay);
        }
    }
}

