using UnityEngine;
using System.Collections;

public class NiceFeatures : MonoBehaviour {
    public GameObject Stars;
    public GameObject Sectors;
    private static NiceFeatures feat;

	// Use this for initialization
	void Start () {
        feat = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public static void setGalaxyFeatureActive(bool active)
    {
        feat.Stars.SetActive(active);
        feat.Sectors.SetActive(active);
        feat.Sectors.GetComponent<ParticlesSystem>().started = false;
    }
}
