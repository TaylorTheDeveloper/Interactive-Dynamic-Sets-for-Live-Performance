using UnityEngine;
using System.Collections;

public class GUITimer : MonoBehaviour
{

    private float current = 0;
	// Use this for initialization
	void Start () {
	
	}
    void OnGUI()
    {
        GUI.Box(
            new Rect(40, 10, 200, 400), 
            string.Format(@"<color={0}>Current Running Time: {1}</color>", "#FFFFFF", current), 
            new GUIStyle() {alignment = TextAnchor.UpperLeft});
    }
    // Update is called once per frame
    void Update ()
	{
	    current += Time.deltaTime;
	}
}
