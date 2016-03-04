using System.Collections.Generic;
using System.Collections.ObjectModel;
using ceometric.DelaunayTriangulator;
using UnityEngine;
using Vision;
using ceometric;
using UnityEngine.UI;
using System;

namespace Game
{
    /// <summary>
    /// this class should handle what the player see of a sector, what the player is trying to do when clicking around
    /// a sector visualization is selectable as well.
    /// </summary>
    public class SectorVisualization : Visualization, ISelectable
    {
        public readonly Sector sector;
        private static List<SectorVisualization> vis = new List<SectorVisualization>();
        private List<Triangle> trs = null;
        private List<Point> points = null;
        private List<Vector2> extremes = new List<Vector2>(); //the point around this sectors.
        private int myNumber = -1;
        private bool visisble = true;
        private SectorClone previousKnowIntel;
        private SectorClone lastKnownIntels;
        private GameObject gm;
        public List<SubSpaceVisualization> subSectorVisualization = new List<SubSpaceVisualization>();

        public SectorVisualization(Sector targetSector, Player observingPlayer) : base()
        {
            gm = GameObject.Instantiate(Resources.Load<GameObject>("Text")); //create the name on the screem
            gm.transform.SetParent(FloatingCanvas.canvas.gameObject.transform, false);
            gm.GetComponent<Text>().text = targetSector.getName();
            gm.GetComponent<RectTransform>().anchoredPosition3D = targetSector.get2dPosition() + new Vector2(0,0.2f);
            gm.GetComponent<RectTransform>().sizeDelta = new Vector3(1.4f, 0.4f);
           
           


                sector = targetSector; //generate the extremes of this sector
            vis.Add(this);
            generateExtremes();

            myNumber = SectorBehaviour.getSectorBehaviour().addSector(extremes, sector.get2dPosition());
            SectorBehaviour.getSectorBehaviour().OnClicked += clicked;

            lastKnownIntels = new SectorClone(sector,observingPlayer);
            previousKnowIntel = null;
            setLastIntel();

            foreach (SubSector sbs in sector.getSubSectors())
            {
                subSectorVisualization.Add(new SubSpaceVisualization(sbs, observingPlayer));
            }
        }

        /// <summary>
        /// when clicked, the player is trying to select it.
        /// </summary>
        /// <param name="num"></param>
        private void clicked(int num, bool shift, int mouseButton)
        {
            if (!lastKnownIntels.isKnown || myNumber != num)
                return;
            if (mouseButton == 0)
            {
                if (shift)
                {
                    if (isVisible())
                        SelectionManger.addSelected(this);
                }
                else
                {
                    if (isVisible())
                    { 
                            SelectionManger.select(this);
                    }
                }
            }
            else
            {
                SectorScene.sectorInspected = this;
                Game.getGame().getRappresentation().setScene(Scene.getScene("SectorScene"));
                CameraMovementManager.target = sector.get2dPosition();
                CameraMovementManager.target -= new Vector3(0, 0, 1);

                Camera.main.transform.position = sector.get2dPosition();
                Camera.main.transform.position -= new Vector3(0, 0, 1);
            }
        }

        /// <summary>
        /// returns every sector visualization instantiated
        /// </summary>
        /// <returns></returns>
        public static ReadOnlyCollection<SectorVisualization> getVisualizations()
        {
            return vis.AsReadOnly();
        }

        /// <summary>
        /// this sets the visualization invisble. this has nothing to do with the visibility rules
        /// this is related to the scene managment.
        /// </summary>
        public override void hide()
        {
            visisble = false;
            SectorBehaviour.getSectorBehaviour().getAreaMaterial(myNumber).color = Color.clear;
            SectorBehaviour.getSectorBehaviour().getBorderMaterial(myNumber).color = Color.clear;
            gm.SetActive(false);
        }

        /// <summary>
        /// return if the sector is visible. this is related to the scene managment
        /// you should check in the Sector class if you want to see if the players are allowed to see it.
        /// </summary>
        /// <returns></returns>
        public override bool isVisible()
        {
            return visisble;
        }

        public bool selectionPersingTroughScenes()
        {
            return false;
        }

        /// <summary>
        /// this sets the visualization visible. this has nothing to do with the visibility rules
        /// this is related to the scene managment.
        /// </summary>
        public override void show()
        {
            Color cl = Color.black;
            cl.a = 0.7f;
            SectorBehaviour.getSectorBehaviour().getAreaMaterial(myNumber).color = cl;
            SectorBehaviour.getSectorBehaviour().getBorderMaterial(myNumber).color = Color.clear;
            visisble = true;
            setLastIntel();

        }

        /// <summary>
        /// called every time this sector should be updated. related to the delay setting
        /// </summary>
        public override void update()
        {
            
            previousKnowIntel = lastKnownIntels;
            lastKnownIntels = new SectorClone(sector, Game.getGame().getRappresentation().getObservingPlayer());
            setLastIntel();

        }

        /// <summary>
        /// this is used to understand how large a sector is in the screen.
        /// </summary>
        /// <returns></returns>
        private List<Triangle> getTriangulation()
        {
            if (trs == null)
            {
                trs = Game.getGame().galaxy.triangulate(getPoints());
            }
            return trs;
        }

        /// <summary>
        /// returns the points of the hull of this sector.
        /// </summary>
        /// <returns></returns>
        private List<Point> getPoints()
        {
            if (points == null)
            {
                Galaxy gl = Game.getGame().galaxy;
                points = new List<Point>();
                foreach (IVertex v in gl.getSectors())
                    points.Add(new Point(v.get2dPosition().x, v.get2dPosition().y, 0));

                float f = gl.generationParam.galaxyEdge;
                points.Add(new Point(2*f, 2*f, 0)); //edges of the galaxy.
                points.Add(new Point(-2 * f, 2*f, 0));
                points.Add(new Point(-2 * f, -2 * f, 0));
                points.Add(new Point(2*f, -2 * f, 0));


            }
            return points;
        }


        public ReadOnlyCollection<Vector2> getExtremes()
        {
            return extremes.AsReadOnly();
        }

        private void generateExtremes()
        {
            int index = sector.galaxy.getSectors().IndexOf(sector);

            Point v = getPoints()[index];

            List<Triangle> nearTris = new List<Triangle>();
            foreach (Triangle t in getTriangulation())
            {
                if (t.Vertex1 == v || t.Vertex2 == v || t.Vertex3 == v)
                    nearTris.Add(t);
            }


            if (nearTris.Count > 2)
            {
                foreach (Triangle t in nearTris)
                {
                    Vector2 extreme = new Vector2(t.getCenter().x, t.getCenter().y);
                    extremes.Add(extreme);
                }
            }
            extremes = Tools.Utils.sort2d(extremes);

        }

        public void OnSelectStart()
        {
            SelectedBehaviour.getSelectedBehaviour().addSelectedSector(this);
        }

        public void OnSelectEnd()
        {
            SelectedBehaviour.getSelectedBehaviour().removeSelectedSector(this);
        }

        public void setLastIntel()
        {
           

           if (previousKnowIntel != null && !previousKnowIntel.isKnown && !lastKnownIntels.isKnown)
                return;

            if (!lastKnownIntels.isKnown)
            {
                Color cl = Color.black;
                cl.a = 0.7f;
                SectorBehaviour.getSectorBehaviour().getAreaMaterial(myNumber).color = cl;
                SectorBehaviour.getSectorBehaviour().getBorderMaterial(myNumber).color = Color.clear;
                gm.SetActive(false);
                return;
            }

            gm.SetActive(true);
            SectorBehaviour.getSectorBehaviour().getAreaMaterial(myNumber).color = lastKnownIntels.landColor;
            SectorBehaviour.getSectorBehaviour().getBorderMaterial(myNumber).color = lastKnownIntels.borderColor;
            
            setVisualizationDelay(lastKnownIntels.infoDelay);

        }

        public void reciveTarget(object ogg)
        {
           
        }
    }
}