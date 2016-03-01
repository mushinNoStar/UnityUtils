using UnityEngine;
namespace Game
{
    public class SectorClone
    {
        public readonly Color landColor;
        public readonly Color borderColor;
        public readonly string name;
        public readonly bool isKnown;
        public readonly bool owns;
        public readonly float infoDelay;

        public SectorClone(Sector sector, Player player)
        {
            if (sector.getOwner() == null)
            {
                landColor = Color.gray;
                landColor.a = 0.3f;
                borderColor = Color.black;
                borderColor.a = 0.2f;
            }
            else
            {
                landColor = sector.getOwner().getColor();
                borderColor = Color.black;
                borderColor.a = 0.2f;
            }
            name = sector.getName();
            if (player.getObservingNation() == null)
            {
                isKnown = true;
                owns = false;
                infoDelay = 0;
            }
            else
            {
                isKnown = player.getObservingNation().knowsSector(sector);
                owns = player.getObservingNation().ownsSector(sector);
                infoDelay = player.getObservingNation().getSectorInfoDelay(sector);
            }
        }   
    }
}
