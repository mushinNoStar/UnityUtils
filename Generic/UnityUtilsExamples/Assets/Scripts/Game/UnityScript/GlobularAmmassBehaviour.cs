using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GlobularAmmassBehaviour : MonoBehaviour
{
    public FleetNumberBehaviour allied;
    public FleetNumberBehaviour enemy;
    public FleetNumberBehaviour neutral;
    public FleetNumberBehaviour owned;
    public event voidMethod OnOwnedClicked;
    public event Game.mouseClickMethod OnClicked;
    public GameObject halo;

    // Use this for initialization
    void Start()
    {

        ParticleSystem prs = GetComponent<ParticleSystem>();
        List<ParticleSystem.Particle> paricles = new List<ParticleSystem.Particle>();
        for (int a = 0; a < 1000; a++)
        {
            ParticleSystem.Particle part = new ParticleSystem.Particle();
            part.startColor = Color.yellow;
            part.position = randomPos();
            part.lifetime = 1000;
            part.startSize = 0.03f;
            part.startLifetime = 1000;
            paricles.Add(part);
        }
        prs.startLifetime = 1000;
        prs.startSize = 0.1f;
        prs.SetParticles(paricles.ToArray(), paricles.Count);
        owned.OnClicked += onOwnedClicked; 
    }

    private void onOwnedClicked()
    {
        if (OnOwnedClicked != null)
            OnOwnedClicked();
    }

    public Vector3 randomPos()
    {
        Vector3 displacement = new Vector3(Random.value-0.5f, Random.value - 0.5f, Random.value - 0.5f);
        float f = (Random.value) * 2;
        displacement = displacement / (f*f);
        return displacement;
    }

    public void clearFleets()
    {
        allied.clear();
        owned.clear();
        neutral.clear();
        enemy.clear();
    }

    public void addAlliedFleet(Game.FleetClone fl)
    {
        allied.fleet.Add(fl);
    }
    public void addOwnedFleet(Game.FleetClone fl)
    {
        owned.fleet.Add(fl);
    }
    public void addEnemyFleet(Game.FleetClone fl)
    {
        enemy.fleet.Add(fl);
    }
    public void addNeutraFleet(Game.FleetClone fl)
    {
        neutral.fleet.Add(fl);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnMouseDown()
    {
        bool shift = false;
        if (Input.GetKey(KeyCode.LeftShift))
            shift = true;
        int button = 0;
        if (Input.GetMouseButton(2))
            button = 2;
        if (OnClicked != null)
            OnClicked(0, shift, button);
    }

    public void OnMouseOver()
    {
        bool shift = false;
        if (Input.GetKey(KeyCode.LeftShift))
            shift = true;
        if (Input.GetMouseButton(1))
            if (OnClicked != null)
               OnClicked(0, shift, 2);
    }

    public void setHalo(bool active)
    {
        halo.SetActive(active);
    }
}
