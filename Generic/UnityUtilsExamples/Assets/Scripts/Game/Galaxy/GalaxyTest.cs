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
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.N))
        {

            Game.Game gm = Game.Game.getGame();
            gm.start("");

            Tools.TimeManager.OnTick += tick;

        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Game.Game.getGame().getRappresentation().setScene(Scene.getScene("GalaxyScene"));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Game.Game.getGame().getRappresentation().setScene();
        }


    }
}
