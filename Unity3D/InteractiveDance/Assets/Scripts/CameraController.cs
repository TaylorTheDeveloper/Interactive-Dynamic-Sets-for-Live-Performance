using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public bool is2D = true;
    private bool was2D = false;
    public Vector3 camera25D = new Vector3(0, 20.4f, -29.5f);
    public Vector2 camera2D = new Vector2(0, 20.4f);
    private GameObject _environment;
    // Use this for initialization
    void Start () {
       _environment = GameObject.Find("Environment");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (is2D && !was2D)
        {
            gameObject.GetComponent<Camera>().orthographic = true;
            gameObject.transform.position = camera2D;
            _environment.SetActive(false);
        }
        else if (!is2D && was2D)
        {
            gameObject.GetComponent<Camera>().orthographic = false;
            gameObject.transform.position = camera25D;
            _environment.SetActive(true);
        }
        was2D = is2D;

    }
}
