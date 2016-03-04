using UnityEngine;
using System.Collections;
using Game;

public class GalaxyTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public void tick()
    {
        Game.Game.getGame().checkConsistency();
        Game.Game.getGame().tick();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {

            Game.Game gm = Game.Game.getGame();
            gm.start("");

            Tools.TimeManager.subscribe(tick);
            Fleet f = new Fleet(Game.Game.getGame().galaxy.getSectors()[0].getSubSectors()[0], Nation.getNation("Humans"), 100);
            Nation n = new Nation("aliens", Game.Game.getGame().galaxy, Color.red);
            Fleet f2 = new Fleet(Game.Game.getGame().galaxy.getSectors()[0].getSubSectors()[0], Nation.getNation("aliens"), 100);

        }

        if (Input.GetKeyDown(KeyCode.O))
        {

            //f.setTargetOfMovement(Game.Game.getGame().galaxy.everySubSector[10]);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {

            Game.Game gm = Game.Game.getGame();
            gm.start(Application.persistentDataPath + "/TestSave.txt");

            Tools.TimeManager.subscribe(tick);

        }

        if (Input.GetKeyDown(KeyCode.B))
        {

            Game.Game.getGame().save(Application.persistentDataPath + "/TestSave.txt");
            Debug.Log(Application.persistentDataPath + "/TestSave.txt");

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Game.Game.getGame().getRappresentation().setScene(Scene.getScene("GalaxyScene"));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Game.Game.getGame().getRappresentation().setScene();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            if (Game.Game.getGame().getRappresentation().getObservingPlayer().getObservingNation() != null) 
                 Game.Game.getGame().getRappresentation().getObservingPlayer().setObservinNation(null);
            else
                Game.Game.getGame().getRappresentation().getObservingPlayer().setObservinNation(Nation.getNation("Humans"));

        }

    }
}
