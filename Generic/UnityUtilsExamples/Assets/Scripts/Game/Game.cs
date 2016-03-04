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

        public void save(string path)
        {
            galaxy.save(path);
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

            if (path.Length == 0)
            {
                GalaxyGenerationParameter param = new GalaxyGenerationParameter();
                generateNewGalaxy(param);

                galaxy.generateGalaxyData();
            }
            else
            {
                galaxy.load(path);
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
                verts.Add(new Vector2(x, y));
                names.Add(Tools.Utils.randomName());
            }
            galaxy.generateStructure(verts, names, param);


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

        public void tick()
        {
            galaxy.tick();
        }
    }
}