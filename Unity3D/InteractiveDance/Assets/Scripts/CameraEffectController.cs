﻿using UnityEngine;
using System.Collections;

public class CameraEffectController : MonoBehaviour
{

    private CameraController _camera;
    public bool Is2D = true;
    private bool _lastUpdate = true;
    public float[] TransitionTime = new float[5];
    private int _currentTransition;
	// Use this for initialization
	void Start () {
	    _camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
	}
	
	// Update is called once per frame
	void Update () {
	    if (_lastUpdate != Is2D)
	    {
	        _camera.is2D = !_camera.is2D;
	    }
	    _lastUpdate = Is2D;

	    if (_currentTransition < TransitionTime.Length && TransitionTime[_currentTransition] < GlobalTimer.RunningTime)
	    {
	        _currentTransition++;
	        _camera.is2D = !_camera.is2D;
        }
	}
}
