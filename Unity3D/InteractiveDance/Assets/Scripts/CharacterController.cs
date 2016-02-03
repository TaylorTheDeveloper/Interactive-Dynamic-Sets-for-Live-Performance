using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    Transform obj;
    public float speed = .5f;
    public bool ZisOn = false;
    public bool XisOn = true;
	// Use this for initialization
	void Start () {
        obj = gameObject.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        
	    if (Input.GetKey("a") && XisOn)
        {
            obj.position = new Vector3(obj.position.x - speed, obj.position.y, obj.position.z);
        }
        if (Input.GetKey("d") && XisOn)
        {
            obj.position = new Vector3(obj.position.x + speed, obj.position.y, obj.position.z);
        }
        if (Input.GetKey("w") && ZisOn)
        {
            obj.position = new Vector3(obj.position.x, obj.position.y , obj.position.z + speed);
        }
        if (Input.GetKey("s") && ZisOn)
        {
            obj.position = new Vector3(obj.position.x, obj.position.y , obj.position.z - speed);
        }
    }
}
