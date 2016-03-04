using System;
using Vision;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Game
{
    public class SubSpaceVisualization : Visualization, ISelectable
    {
        private GlobularAmmassBehaviour behaviour;
        private SubSector subSector;
        private SubSectorClone previousKnowIntel = null;
        private SubSectorClone lastKnownIntels;
        private GameObject gm;
        private List<TwoPointBeaviour> inConnection = new List<TwoPointBeaviour>();

        public static List<SubSpaceVisualization> listOfVisualization = new List<SubSpaceVisualization>(); 

        public SubSpaceVisualization(SubSector targetSector, Player observingPlayer) : base()
        {
            gm = GameObject.Instantiate(Resources.Load<GameObject>("Text")); //create the name on the screem
            gm.GetComponent<DisapearBehaviour>().distance = 0;
            gm.transform.SetParent(FloatingCanvas.canvas.gameObject.transform, false);
            gm.GetComponent<Text>().text = targetSector.getName();
            gm.GetComponent<RectTransform>().anchoredPosition3D = (new Vector3(0, 0.05f, 0.1f))+ ((Vector3)targetSector.getPosition());
            gm.GetComponent<RectTransform>().sizeDelta = new Vector3(1f, 0.2f);
            gm.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            gm.SetActive(false);

            subSector = targetSector;
            lastKnownIntels = new SubSectorClone(targetSector, observingPlayer);

            listOfVisualization.Add(this);
        }

       

        public override void hide()
        {
            GameObject.Destroy(behaviour.gameObject);
            behaviour.OnOwnedClicked -= fleetClicked;
            behaviour.OnClicked -= clicked;
            behaviour = null;
            gm.SetActive(false);
            foreach (TwoPointBeaviour con in inConnection)
                GameObject.Destroy(con.gameObject);
            inConnection.Clear();
        }

        public override bool isVisible()
        {
            return (behaviour != null);
        }

        public void OnSelectEnd()
        {
            behaviour.setHalo(false);
        }

        public void OnSelectStart()
        {
            behaviour.setHalo(true);
        }

        public override void show()
        {
            behaviour = GameObject.Instantiate(Resources.Load<GameObject>("GlobularAmmass")).GetComponent<GlobularAmmassBehaviour>();
            behaviour.transform.position = subSector.getPosition();
            behaviour.OnOwnedClicked += fleetClicked;
            behaviour.OnClicked += clicked;

            setLastIntel();

            foreach (Connection con in subSector.getConnections())
            {
                Player obsp = Game.getGame().getRappresentation().getObservingPlayer();
                if (con.subSectors[0] == subSector || !con.internalConnection)
                {
                    GameObject gm = GameObject.Instantiate(Resources.Load<GameObject>("2PointConnection"));
                    TwoPointBeaviour behav = gm.GetComponent<TwoPointBeaviour>();

                    if (obsp.getObservingNation() == null || obsp.getObservingNation().connectionLevel >= con.getConnectionLevel())
                        behav.color = Definitions.connectionColor[con.getConnectionLevel()];
                    else
                        behav.color = Color.clear;

                    if (con.internalConnection)
                    {
                        behav.extreme1 = con.subSectors[0].getPosition();
                        behav.extreme2 = con.subSectors[1].getPosition();
                    }
                    else
                    {
                        behav.extreme1 = subSector.getPosition();
                        if (con.subSectors[0] == subSector)
                            behav.extreme2 = con.subSectors[1].sector.get2dPosition();
                        else
                            behav.extreme2 = con.subSectors[0].sector.get2dPosition();
                    }
                    if (con.internalConnection)
                        behav.size = 0.004f;
                    else
                        behav.size = 0.008f;
                    inConnection.Add(behav);

                }
            }
        }

        public void clicked(int num, bool shift, int mouseButton)
        {
            if (mouseButton == 0)
            {
                if (!shift)
                    SelectionManger.select(this);
                else
                    SelectionManger.addSelected(this);
            }
            else
            {
                SelectionManger.addTarget(subSector);
            }
        }

        public void fleetClicked()
        {
            foreach (FleetClone f in lastKnownIntels.fleets)
            {
                if (f.fleet.nation == Game.getGame().getRappresentation().getObservingPlayer().getObservingNation())
                {
                    Vision.SelectionManger.select(new SelectableFleet(f.fleet));
                    return;
                }
            }
        }

        public override void update()
        {
            previousKnowIntel = lastKnownIntels;
            lastKnownIntels = new SubSectorClone(subSector, Game.getGame().getRappresentation().getObservingPlayer());
            setLastIntel();
        }

        public void setLastIntel()
        {
            gm.SetActive(true);
            behaviour.clearFleets();
            foreach (FleetClone cl in lastKnownIntels.fleets)
            {
                if (cl.owned)
                    behaviour.addOwnedFleet(cl);
                else if (cl.enemy)
                    behaviour.addEnemyFleet(cl);
                else if (cl.allied)
                    behaviour.addAlliedFleet(cl);
                else
                    behaviour.addNeutraFleet(cl);

            }
          

            setVisualizationDelay(lastKnownIntels.infoDelay);
        }

        public bool selectionPersingTroughScenes()
        {
            return false;
        }

        public void reciveTarget(object ogg)
        {
        }
    }
}
