
namespace Game
{
    public class Game
    {
        private static Game currentGame = null;

        public readonly Galaxy galaxy = new Galaxy();
        //private RulesManager rules = new RulesManager(); 
        private Rappresentation rappresentation;
        //private NetworkManager network = new NetworkManager();

        private Game()
        {
            
        }

        public Rappresentation getRappresentation()
        {
            return rappresentation;
        }

        public void start(string path)
        {
            if (path.Length == 0)
            {
                galaxy.generate(new GalaxyGenerationParameter());
                rappresentation = new Rappresentation(galaxy);
            }
        }

        public static Game getGame()
        {
            if (currentGame == null)
                currentGame = new Game();
            return currentGame;
        }
    }
}