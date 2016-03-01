using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public class Server
    {
        private static Server server = null;
        private bool amIServer = true;
        private bool started = false;
        private int thisPlayerID = -1;

        private List<Vector2> positions = null;
        private List<string> names = null;

        private Server()
        {
            server = this;
            UnityEngine.Debug.Log("remember sync, actor, target and actions id system is faulty, savefile!!!");
        }

        public void connect()
        {
            amIServer = false;
            started = true;
            //throw new System.Exception("not implemented yet");
        }

        public void openServer()
        {
            amIServer = true;
            started = true;

        }

        public bool isServer()
        {
            return amIServer;
        }

        public bool serverStarted()
        {
            return started;
        }

        public static Server getServer()
        {
            if (server == null)
                new Server();
            return server;
        }

        /// <summary>
        /// this is called by the actionmanager, this is ok, no reason to change it.
        /// </summary>
        /// <param name="RuleName"></param>
        /// <param name="callerId"></param>
        /// <param name="targetsId"></param>
        /// <param name="otherParam"></param>
        public void sendActionRequest(string RuleName, int callerId, List<int> targetsId, List<string> otherParam)
        {
            if (!amIServer) //if i'm not the server, it passes the the request to the real server
            {
                Debug.Log("Sending a request");
                if (callerId != thisPlayerID)
                    throw new System.Exception("You can't call that rule");
                sendRequest(RuleName, callerId, targetsId, otherParam);
            }
            else //if i'm the server, it just execute it.
            {
                Actions.ActionManager.executeRule(RuleName, callerId, targetsId, otherParam); 
            }
        }

        public void sendAllStructuralData()
        {
            if (!amIServer || !started)
                return;

            List<Vector2> generateStructure = Game.Game.getGame().galaxy.structureList;
            List<string> strctureName = Game.Game.getGame().galaxy.strctureName;

            string names = "";
            names = Tools.Utils.uniteList(strctureName);

            string pos = "";
            List<string> posStrings = new List<string>();
            foreach (Vector2 vc in generateStructure)
            {
                Debug.Log(vc.x.ToString("0.000000") + "_" + vc.y.ToString("0.000000"));
            }
            pos = Tools.Utils.uniteList(posStrings);

            ServerBehaviour.myBehaviour.RpcSendStructuralData(pos, names);
            
        }

        public void setStructuralData(List<Vector2> generateStructure, List<string> strctureName)
        {
            if (isServer() || positions != null)
                return;
            positions = generateStructure;
            names = strctureName;
        }

        public void askStructuralData()
        {
            ServerBehaviour.myBehaviour.CmdAskForStructuralData();
        }

        private void sendRequest(string RuleName, int callerId, List<int> targetsId, List<string> otherParam)
        {
            //throw new System.NotImplementedException("not yet implemented the this"); //this should be the thing that handles unity networking.

            List<string> data = new List<string>();
            data.Add(RuleName);
            data.Add(callerId+"");
            foreach (int t in targetsId)
                data.Add(t+"");
            data.Add("#");
            foreach (string s in otherParam)
                data.Add(s);
            data.Add("#");

            string passed = Tools.Utils.uniteList(data);

            ServerBehaviour.myBehaviour.CmdPlayerUsedAction(passed);
        }


        /// <summary>
        /// this is called every time something changes, so every player get the update.
        /// </summary>
        /// <param name="syncId"></param>
        /// <param name="serialized"></param>
        /// <param name="typeName"></param>
        public void sendSincronizableUpdate(int syncId, List<string> serialized, string typeName)
        {
            if (!amIServer || !started)
                return;
            Debug.Log("Sending an update");


            ServerBehaviour.myBehaviour.RpcSendUpdate(syncId, Tools.Utils.uniteList(serialized), typeName);
        }

        /// <summary>
        /// called by unity stuff in the client, 
        /// </summary>
        /// <param name="syncId"></param>
        /// <param name="serialized"></param>
        /// <param name="typeName"></param>
        public void reciveSyncronizableUpdate(int syncId, List<string> serialized, string typeName)
        {
            if (amIServer)
                return;

            Syncronizable sync = Syncronizable.getSyncronizable(syncId);
            Debug.Log(syncId+" "+typeName);
            if (sync != null)
                sync.deserialize(serialized); //if there is already such syncronizable, it updates it
            else
                Syncronizable.createNewSync(syncId, serialized, typeName); //else it creates one
        }

        /// <summary>
        /// this foces the server to send to everyone every data.
        /// </summary>
        public void askForAll()
        {
            if (isServer())
                return;

            ServerBehaviour.myBehaviour.CmdAskForEverything();
        }

        /// <summary>
        /// the server send to everyone every data.
        /// </summary>
        public void sendAll()
        {
            if (!isServer())
                return;
            Syncronizable.updateAll();
        }

        public List<Vector2> getSectorsPostion()
        {
            if (positions == null)
                throw new System.Exception("the game did not started yet.");

            return positions;
        }

        public List<string> getSectorNames()
        {
            if (names == null)
                throw new System.Exception("the game did not started yet.");

            return names;
        }

        /// <summary>
        /// return true if the game started in the server, else return false
        /// </summary>
        /// <returns></returns>
        public bool gameStarted()
        {
            if (isServer())
                return Game.Game.getGame().isStarted();
            else
                return ServerBehaviour.myBehaviour.isGameStarted();
        }

    }
}
