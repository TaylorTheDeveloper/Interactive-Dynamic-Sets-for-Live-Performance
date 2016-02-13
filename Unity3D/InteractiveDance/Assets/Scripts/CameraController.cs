using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public bool is2D = true;
    private bool was2D = false;
    public Vector3 camera25D = new Vector3(0, 20.4f, -29.5f);
    public Vector2 camera2D = new Vector2(0, 20.4f); 
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (is2D && !was2D)
        {
            gameObject.GetComponent<Camera>().orthographic = true;
            gameObject.transform.position = camera2D;
        }
        else if (!is2D && was2D)
        {
            gameObject.GetComponent<Camera>().orthographic = false;
            gameObject.transform.position = camera25D;
        }
        was2D = is2D;

    }
}
