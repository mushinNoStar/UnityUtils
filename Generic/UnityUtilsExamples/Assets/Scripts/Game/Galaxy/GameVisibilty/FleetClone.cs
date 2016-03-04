namespace Game
{
    public class FleetClone
    {
        public readonly int size;
        public readonly Fleet fleet;
        public readonly bool enemy = false;
        public readonly bool allied = false;
        public readonly bool owned = false;

        public FleetClone(Fleet fl, Player player)
        {
            fleet = fl;
            if (player.getObservingNation() == null)
            {
                size = fleet.size();
            }
            else
            {
                if (player.getObservingNation() == fleet.nation)
                {
                    size = fleet.size();
                    owned = true;
                }
                else
                    size = 1;
            }
        }

        public bool isSelected()
        {
            foreach (Vision.ISelectable sel in Vision.SelectionManger.getCurrentSelected())
                if (sel is SelectableFleet)
                    if (((SelectableFleet)sel).targetingFleet == fleet)
                        return true;
            return false;
        }
    }
}