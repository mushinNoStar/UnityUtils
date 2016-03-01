using UnityEngine;
using System.Collections.Generic;

namespace Game
{
    public class Game
    {
        private static Game currentGame = null;
        private bool started = false;

        /// <summary>
        /// There can be only one galaxy clearly.
        /// </summary>
        public readonly Galaxy galaxy = new Galaxy();
        //private RulesManager rules = new RulesManager(); 
        private Rappresentation rappresentation;
        //private NetworkManager network = new NetworkManager();

        private Game()
        {
            
        }

        public bool isStarted()
        {
            return started;
        }

        /// <summary>
        /// return the current rappresentation system.
        /// </summary>
        /// <returns></returns>
        public Rappresentation getRappresentation()
        {
            return rappresentation;
        }

        /// <summary>
        /// if the path lenght is equal to zero, it generates a new game, else it loads one, from the path.
        /// </summary>
        /// <param name="path"></param>
        public void start(string path)
        { 
            if (!Networking.Server.getServer().isServer()) //if i'm not the server
            {
                if (Networking.Server.getServer().gameStarted()) //then if the game starteted in the server
                {
                    askForStructuralData(); //ask for every possible data
                    Networking.Server.getServer().askForAll(); 
                }
                else //wait for the game to start
                    Debug.Log("Game did not started in the server yet, wait for his call");
                return;
            }
            else //but if i'm not the server
            {
                if (path.Length == 0)
                {
                    GalaxyGenerationParameter param = new GalaxyGenerationParameter();
                    generateNewGalaxy(param);

                    Networking.Server srv = Networking.Server.getServer();
                    if (srv.serverStarted()) //if i'm the server
                    {
                        Debug.Log("sent structure");
                        srv.sendAllStructuralData(); //then allert every player that he should start.
                    }
                    galaxy.generateGalaxyData(param);
                }
                else
                {
                    throw new System.Exception("not implemented");
                    //loadStructure(path);
                    //loadGalaxyData(path);
                }
            }

            rappresentation = new Rappresentation(galaxy); //then set up the rappresentation
            
            started = true;
        }

        public void generateNewGalaxy(GalaxyGenerationParameter param)
        {
            List<Vector2> verts = new List<Vector2>();
            List<string> names = new List<string>();
            for (int a = 0; a < param.numberOfSystems; a++)
            {
                float x = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                float y = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                while ((x * x) + ((y * y) * 4) > param.galaxyEdge * 10f)
                {
                    x = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                    y = Random.value * param.galaxyEdge - (param.galaxyEdge / 2);
                }
                verts.Add(new Vector2(x,y));
                names.Add(Tools.Utils.randomName());
            }
            galaxy.generateStructure(verts, names);
            
            
        }

        public void askForStructuralData()
        {
            Networking.Server.getServer().askStructuralData();
            List<Vector2> pos = Networking.Server.getServer().getSectorsPostion();
            List<string> names = Networking.Server.getServer().getSectorNames();
            galaxy.generateStructure(pos, names);
        }

        public static Game getGame()
        {
            if (currentGame == null)
                currentGame = new Game();
            return currentGame;
        }

        public void checkConsistency()
        {
            galaxy.checkConsistency();
        }
    }
}