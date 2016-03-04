using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlesSystem : MonoBehaviour {
    public bool started = false;
    private float timer = 1000;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer < 0)
            started = false;

        if (started)
            return;

        if (Game.Game.getGame().getRappresentation() != null)
        {
            ParticleSystem prs = GetComponent<ParticleSystem>();
            List<ParticleSystem.Particle> paricles = new List<ParticleSystem.Particle>();
            foreach (Game.Sector sc in Game.Game.getGame().galaxy.getSectors())
            {
                ParticleSystem.Particle part = new ParticleSystem.Particle();
                part.startColor = Color.cyan;
                part.position = sc.get2dPosition();
                part.lifetime = 1000;
                part.startSize = 0.1f;
                part.startLifetime = 1000;
                paricles.Add(part);
            }
            prs.startLifetime = 1000;
            prs.startSize = 0.1f;
            prs.SetParticles(paricles.ToArray(), paricles.Count);
            timer = 1000;
            started = true;
        }
       
	}
}
