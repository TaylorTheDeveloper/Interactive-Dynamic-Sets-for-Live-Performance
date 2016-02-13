using UnityEngine;
using System.Collections;

public class ParticleEffectController : MonoBehaviour
{

    private GameObject[] _dancers;
    public int UpdateFrequency = 3;
    public int Delay = 1;


    public bool StopTime;
    public bool ReverseGravity;
    public bool Both;
    public bool ConfirmSet;
    private int _lastUpdate;
    [Range(0, 20)] public int Speed = 10;
    private float _current = 0;
    private float _delayCurrent = 5;
	// Use this for initialization
	void Start ()
	{
        _dancers = GameObject.FindGameObjectsWithTag("Dancer");
	    _lastUpdate = Speed;
    }

    // Update is called once per frame
    void Update()
    {
        _current += Time.deltaTime;
        if (_current >= UpdateFrequency)
        {
            CheckUpdateDancers();
            _current = 0;
        }
        if (ConfirmSet)
        {
            CheckEffects();
            ConfirmSet = false;
        }
        if (_delayCurrent < Delay)
        {
            _delayCurrent += Time.deltaTime;
            if (_delayCurrent >= Delay)
            {
                DisableOtherSystem();
            }
        }
        if (_lastUpdate != Speed)
        {
            UpdateSpeed();
        }   
	}

    void UpdateSpeed()
    {
        foreach (var dancer in _dancers)
        {
            var reg = dancer.transform.GetChild(0).GetComponent<ParticleSystem>();
            var reverse = dancer.transform.GetChild(1).GetComponent<ParticleSystem>();
            reg.playbackSpeed = Speed * .1f;
            reverse.playbackSpeed = Speed * .1f;
        }
    }

    void CheckUpdateDancers()
    {
        var currentDancers = GameObject.FindGameObjectsWithTag("Dancer");

        if (_dancers.Length < currentDancers.Length)
        {
            _dancers = currentDancers;
        }
    }

    void CheckEffects()
    {
        foreach (var dancer in _dancers)
        {
            var reg = dancer.transform.GetChild(0).GetComponent<ParticleSystem>();
            var reverse = dancer.transform.GetChild(1).GetComponent<ParticleSystem>();
            var regEm = reg.emission;
            var revEm = reverse.emission;

            if (ReverseGravity)
            {
                revEm.enabled = true;
                _delayCurrent = 0;
            }
            else
            {
                regEm.enabled = true;
                _delayCurrent = 0;
            }
            if (Both)
            {
                revEm.enabled = true;
                regEm.enabled = true;
                _delayCurrent = 5;
            }
            else
            {
                _delayCurrent = 0;
            }
            if (StopTime)
            {
                reg.GetComponent<ParticleSystem>().Pause();
                reverse.GetComponent<ParticleSystem>().Pause();
            }
            else
            {
                reg.GetComponent<ParticleSystem>().Play();
                reverse.GetComponent<ParticleSystem>().Play();
            }
        }   
    }

    void DisableOtherSystem()
    {
        foreach (var dancer in _dancers)
        {
            var reg = dancer.transform.GetChild(0).GetComponent<ParticleSystem>();
            var reverse = dancer.transform.GetChild(1).GetComponent<ParticleSystem>();
            var regEm = reg.emission;
            var revEm = reverse.emission;

            if (ReverseGravity)
            {
                regEm.enabled = false;
            }
            else
            {
                revEm.enabled = false;
            }
        }
    }

}
