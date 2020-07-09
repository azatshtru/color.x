using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connector : MonoBehaviour
{
    public static Connector Instance;

    Dictionary<Slider, string> sliderList = new Dictionary<Slider, string>();

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(Instance);
        }
        Instance = this;
    }

    public void AddSliderConnection (string id, Slider slider)
    {
        if (sliderList.ContainsKey(slider))
        {
            return;
        }
        sliderList.Add(slider, id);
    }

    public void SyncSliders(Slider slider, string id)
    {
        float value = slider.value;

        foreach(Slider s in sliderList.Keys)
        {
            if(sliderList[s] == id)
            {
                s.value = value;
            }
        }
    }
}
