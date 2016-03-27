using UnityEngine;
using System.Collections;


public class MenuManager : MonoBehaviour
{
    //singleton data

    public static readonly int TextureCount = 9;
    public static readonly int OptionMax = 3;
    public readonly int TimeToActivate = 1;
    public int CurrentRange = 0;

    public GameObject Effect;
    public Texture[] TextureList = new Texture[TextureCount];
    public GameObject[] PrefabToActivate = new GameObject[TextureCount];
    private GameObject[] _options = new GameObject[OptionMax];

    private int _newRange = 0;
    private bool _hasChanged = true;

    // Use this for initialization
    void Start ()
	{
	    Effect = GameObject.Find("ActiveEffect");
	    for (var i = 0; i < OptionMax; i++)
	    {
            _options[i] = transform.GetChild(i).gameObject;
	        _options[i].GetComponent<OptionSelector>().id = i;
	    }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if (_hasChanged)
	    {
	        for (var i = 0; i < OptionMax; i++)
	        {
                _options[i].GetComponent<Renderer>().material.mainTexture = TextureList[CurrentRange + i];
            }
            _hasChanged = false;
	    }
	    if (_newRange != CurrentRange)
	    {
	        _hasChanged = true;
            _newRange = CurrentRange;
	    }
	
	}

    public void ActivateElement(int x)
    {
        Debug.Log((x + CurrentRange) + " is activated");
        foreach (Transform child in Effect.transform)
        {
            Destroy(child.gameObject);
        }

        var obj = Instantiate(PrefabToActivate[(x + CurrentRange)]);
        obj.transform.parent = Effect.transform;
    }

}
