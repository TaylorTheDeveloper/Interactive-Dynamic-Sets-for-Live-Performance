using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectGenerator : MonoBehaviour
{
    public List<EffectBlock> EffectList;

    private float _fpTolerance = .5f;
    private float _currentTime = 0;
    // Use this for initialization
    void Start()
    {
        EffectList = new List<EffectBlock>();
        var list = transform.GetChild(0).gameObject.transform;
        var production = new List<GameObject>();
        for(var i = 0; i < list.childCount; i++)
        {
            production.Add(list.GetChild(i).gameObject);
        }
        foreach (var effect in production)
        {
            
            var ttl = effect.gameObject.GetComponent<TTL>();
            EffectList.Add(new EffectBlock
            {
                GObject = effect,
                Position = effect.transform.position,
                Rotation = effect.transform.rotation,
                Scale = effect.transform.localScale,
                StartTime = ttl.start,
                EndTime = ttl.end,
                IsAttached =  ttl.attached
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalTimer.RunningTime > _currentTime)
        {
            UpdateEffects();
            _currentTime++;
        }
    }

    void UpdateEffects()
    {
        //Debug.Log(string.Format(@"Updating Effects {0} Remaining", EffectList.Count));
        for (var i = 0; i < EffectList.Count; i++)
        {
            if (EffectList[i].GObject.activeSelf && EffectList[i].EndTime < GlobalTimer.RunningTime)
            {
                Destroy(EffectList[i].GObject);
                EffectList.RemoveAt(i--);
            }
            else if (!EffectList[i].GObject.activeSelf && EffectList[i].StartTime < GlobalTimer.RunningTime)
            {
                var e = EffectList[i];
                if (e.IsAttached)
                {
                    foreach (var dancer in FormController.bodies)
                    {
                        Debug.Log("Attaching Effect");
                        SetCurrentEffect(dancer);
                    }
                }
                else
                {
                    EffectList[i].GObject.SetActive(true);
                }

            }
        }

    }



    public void SetCurrentEffect(Form o)
    {
        foreach (var e in EffectList)
        {
            if (e.IsAttached && e.StartTime < GlobalTimer.RunningTime)
            {
                var obj = Instantiate(e.GObject.GetComponent<TTL>().prefab);
                obj.transform.parent = o.Root.transform;
            }
        }
        Debug.Log("setting effect on this");
    }
}
