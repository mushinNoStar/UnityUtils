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

        public ConnectionVisualization(Connection con) : base ()
        {
            extremes = new List<Vector2>();
            extremes.Add(con.sectors[0].get2dPosition());
            extremes.Add(con.sectors[1].get2dPosition());
            connection = con;
            vis.Add(this);
            myNumber = ConnectionBehaviour.getConnectionBehaviour().addSegment(extremes);
        }


        public override void hide()
        {
            throw new NotImplementedException();
        }

        public override bool isVisible()
        {
            return false;
            throw new NotImplementedException();
        }

        public override void show()
        {
            throw new NotImplementedException();
        }

        public override void update()
        {
            throw new NotImplementedException();
        }
    }
}

