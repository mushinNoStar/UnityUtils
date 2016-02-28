using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ConnectionVisualization : Visualization
    {
        public readonly Connection connection;
        private static List<ConnectionVisualization> vis = new List<ConnectionVisualization>();
        private int myNumber = -1;
        public readonly List<Vector2> extremes;
        public bool visible = true;

        public ConnectionVisualization(Connection con) : base ()
        {
            extremes = new List<Vector2>();
            extremes.Add(con.sectors[0].get2dPosition());
            extremes.Add(con.sectors[1].get2dPosition());
            connection = con;
            vis.Add(this);
            myNumber = ConnectionBehaviour.getConnectionBehaviour().addSegment(extremes);
        }

        public static List<ConnectionVisualization> getVisualization()
        {
            return vis;
        }

        public override void hide()
        {
            visible = false;
            ConnectionBehaviour.getConnectionBehaviour().getMaterial(myNumber).color = (Color.clear);
        }

        public override bool isVisible()
        {
            return visible;
        }

        public override void show()
        {
            visible = true;
            ConnectionBehaviour.getConnectionBehaviour().getMaterial(myNumber).color = Color.blue;
        }

        public override void update()
        {
            //throw new NotImplementedException();
        }
    }
}

