using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void voidMethod();
public class FleetNumberBehaviour : MonoBehaviour {

    public List<Game.FleetClone> fleet = new List<Game.FleetClone>();
    public List<Game.FleetClone> currentShowing = null;
    public GUIStyle style;
    public Vector2 offset;
    public static Vector2 size = new Vector2(20, 20);
    public event voidMethod OnClicked;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void clear()
    {
        fleet.Clear();
        currentShowing = null;
    }

    public void OnGUI()
    {

        if (fleet.Count == 0)
            return;

        Vector3 thisPosition = Camera.main.WorldToScreenPoint(transform.parent.position);
        Vector2 screenPos = (new Vector2(thisPosition.x, Screen.height-thisPosition.y)) + offset;
       
        Rect rect = new Rect(screenPos.x-25, screenPos.y, 50,20);
        int quantity = 0;

        style.normal.textColor = Color.black;

        foreach (Game.FleetClone f in fleet)
        {
            quantity += f.size;
            if (f.isSelected())
                style.normal.textColor = Color.yellow;


        }
        if (quantity > 0)
            if (GUI.Button(rect, quantity + "", style) && OnClicked != null)
                OnClicked();
    }
}
