using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SectorPanel : MonoBehaviour {

    public RectTransform mainPannel;
    public Text subSectorTitle;

    public Text colonizedPlanetCount;
    public Text enemyPlanetCount;
    public Text alliedPlanetCount;
    public Text neutraPlanetCount;

    public Text stablePlanetCount;
    public Text poorPlanetCount;
    public Text unstablePlanetCount;
    public Text uncolonizablePlanetCount;

    public Image colonizeStable;
    public Image colonizePoor;
    public Image colonizeUnstable;
    public Image allowPrivateColonization;

    public Image investInPlanetary;
    public Image investInSpace;
    public Image allowPrivateSpaceDevelopment;
    public Image allowPrivatePlanetaryDevelopment;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
