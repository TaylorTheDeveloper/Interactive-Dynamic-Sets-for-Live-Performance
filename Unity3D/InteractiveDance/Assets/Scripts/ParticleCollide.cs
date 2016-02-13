using UnityEngine;
using System.Collections;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class ParticleCollide : MonoBehaviour {

	// Use this for initialization
    public bool HasWaterfall = false;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject c)
    {
        if (c.tag == "InteractiveParticles")
        {
            TimeSand.Hits += 1;
            if (!HasWaterfall)
            {
                HasWaterfall = true;
            }
        }

    }
}
