using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class ServerBehaviour : NetworkBehaviour {

    public static ServerBehaviour myBehaviour;
    public bool started = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdPlayerUsedAction(string passed)
    {
        List<string> paramter = Tools.Utils.splitList(passed);
        string ruleName = paramter[0];
        paramter.RemoveAt(0);

        int userID = int.Parse(paramter[0]);
        paramter.RemoveAt(0);

        List<int> targetsId = new List<int>();
        while (paramter[0] != "#")
        {
            targetsId.Add(int.Parse(paramter[0]));
            paramter.RemoveAt(0);
        }
        paramter.RemoveAt(0);

        List<string> otherParam = new List<string>();
        while (paramter[0] != "#")
        {
            otherParam.Add(paramter[0]);
            paramter.RemoveAt(0);
        }
        paramter.RemoveAt(0);

        Actions.ActionManager.executeRule(ruleName, userID, targetsId, otherParam);
    }

    [ClientRpc]
    public void RpcSendUpdate(int syncId, string serialized, string typeName)
    {
        List<string> deserialized = Tools.Utils.splitList(serialized);
        Networking.Server.getServer().reciveSyncronizableUpdate(syncId, deserialized, typeName);
    }

    public override void OnStartLocalPlayer()
    {
        myBehaviour = this;
        if (NetworkServer.active)
            Networking.Server.getServer().openServer();
        else
            Networking.Server.getServer().connect();
    }

    [Command]
    public void CmdAskForEverything()
    {
        Networking.Server.getServer().sendAll();
    } 

    public bool isGameStarted()
    {
        CmdisGameStarted();
        return started;
    }

    [Command]
    public void CmdisGameStarted()
    {
        RpcSetStarted(Game.Game.getGame().isStarted());
    }

    [ClientRpc]
    public void RpcSendStructuralData(string pos, string names)
    {
        if (Networking.Server.getServer().isServer())
            return;
        List<string> nms = Tools.Utils.splitList(names);

        List<Vector2> vect = new List<Vector2>();
        List<string> unitePos = Tools.Utils.splitList(pos);
        foreach (string str in unitePos)
        {
            string[] values = str.Split('_');
            Debug.Log(str+"| "+str[0]+" "+str[1]+" "+str[2]);
            vect.Add(new Vector2(float.Parse(values[0]), float.Parse(values[2])));
        }
        Networking.Server.getServer().setStructuralData(vect, nms);
    }

    [Command]
    public void CmdAskForStructuralData()
    {
        Networking.Server.getServer().sendAllStructuralData();
    }

    [ClientRpc]
    public void RpcSetStarted(bool b)
    {
        started = b;
    }
}
