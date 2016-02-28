using System.Collections.Generic;

namespace Game
{
    public class Rappresentation
    {
        private Galaxy galaxy;
        private Player currentPlayerProspective;
        private Scene currentScene = null;

        public Rappresentation(Galaxy gal)
        {
            galaxy = gal;
            buildVisualizationOf(gal);
            Tools.TimeManager.OnTick += tick;
        }

        private void buildVisualizationOf(Galaxy gal)
        {
            foreach (Sector sc in galaxy.getSectors())
                new SectorVisualization(sc);
            
            foreach (Connection con in galaxy.getConnection())
                new ConnectionVisualization(con);
        }

        public void setScene(Scene newScene)
        {
            if (currentScene != null)
                currentScene.OnEnd();
            currentScene = newScene;
            newScene.OnStart();
        }

        private void tick()
        {
            if (currentScene != null)
                currentScene.tick();
        }

        public void setPlayerProspective(Player pl)
        {
            currentPlayerProspective = pl;
        }
        
        public Player getObservingPlayer(Player pl)
        {
            return currentPlayerProspective;
        }
    }
}
