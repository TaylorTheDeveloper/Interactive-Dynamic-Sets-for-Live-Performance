using UnityEngine;
using System.Collections;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;

public class ParticleCollide : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnParticleCollision(GameObject c)
    {

        if (c.tag == "InteractiveParticles")
        {

        }

    }
}
