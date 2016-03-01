using UnityEngine;
using Vision;
namespace Game
{
    public class ConnectionClone
    {
        public readonly bool isKnow;
        public readonly Color color;
        public readonly float delay;

        public ConnectionClone(Connection con, Player pl)
        {
            if (pl.getObservingNation() == null)
            {
                isKnow = true;
                delay = 0;
                color = Definitions.connectionColor[con.getConnectionLevel()];
                return;
            }

            if (con.getConnectionLevel() <= pl.getObservingNation().connectionLevel)
                color = Definitions.connectionColor[con.getConnectionLevel()];
            else
                color = Color.clear;

            if (pl.getObservingNation().knowsSector(con.sectors[0]) || pl.getObservingNation().knowsSector(con.sectors[1]))
                isKnow = true;
            else
                isKnow = false;
            delay = pl.getObservingNation().getSectorInfoDelay(con.sectors[0]);
            float f = pl.getObservingNation().getSectorInfoDelay(con.sectors[1]);
            if (delay > f)
                delay = f;
            
        }
    }
}