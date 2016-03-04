using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisapearBehaviour : MonoBehaviour {

    public float distance = -1.5f;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Camera.main.transform.position.z > distance)
            GetComponent<Text>().enabled = false;
        else
            GetComponent<Text>().enabled = true;

    }
}
