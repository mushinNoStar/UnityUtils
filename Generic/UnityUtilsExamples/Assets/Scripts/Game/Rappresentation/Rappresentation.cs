﻿using System.Collections.Generic;

namespace Game
{
    /// <summary>
    /// the rappresentation class handles everything the player sees on the screen.
    /// this does not determin what the player is allowed to see by the rules.
    /// that is handled in the game rules.
    /// </summary>
    public class Rappresentation
    {
        private Galaxy galaxy;
        private Player currentPlayerProspective;
        private Scene currentScene = null;

        public Rappresentation(Galaxy gal)
        {
            galaxy = gal;
            if (Networking.Server.getServer().isServer()) //if it is the server generates the obeserver player, else it look for it.
            {
                currentPlayerProspective = new Player("Observer");
                currentPlayerProspective.setObservinNation(Nation.getNation("Humans"));
            }
            else
                currentPlayerProspective = Player.getPlayer("Observer");

            buildVisualizationOf(gal);
            Tools.TimeManager.OnTick += tick;
            setScene(Scene.getScene("GalaxyScene"));
        }

        /// <summary>
        /// set up the visualizations
        /// </summary>
        /// <param name="gal"></param>
        private void buildVisualizationOf(Galaxy gal)
        {
            foreach (Sector sc in galaxy.getSectors())
                new SectorVisualization(sc, currentPlayerProspective);
            
            foreach (Connection con in galaxy.getConnection())
                new ConnectionVisualization(con, currentPlayerProspective);
        }

        /// <summary>
        /// close the previous scene
        /// and open the new one.
        /// </summary>
        /// <param name="newScene"></param>
        public void setScene(Scene newScene)
        {
            Vision.SelectionManger.setSelectNull();
            if (currentScene != null)
                currentScene.OnEnd();
            currentScene = newScene;
            newScene.OnStart();
        }

        /// <summary>
        /// close the previous scene
        /// </summary>
        public void setScene()
        {
            Vision.SelectionManger.setSelectNull();
            if (currentScene != null)
                currentScene.OnEnd();
            currentScene = null;
        }

        private void tick()
        {
            if (currentScene != null)
                currentScene.tick();
        }

        /// <summary>
        /// set the player that is observing the game.
        /// this is not related to what the player can do in the network.
        /// it simply determin from what prospective it is looking the game.
        /// 
        /// it is used by visualizations.
        /// </summary>
        /// <param name="pl"></param>
        public void setPlayerProspective(Player pl)
        {
            currentPlayerProspective = pl;
        }
        
        public Player getObservingPlayer()
        {
            return currentPlayerProspective;
        }
    }
}
