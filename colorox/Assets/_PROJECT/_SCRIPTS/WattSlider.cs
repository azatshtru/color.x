using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WattSlider : MonoBehaviour {

    //[HideInInspector]
    public float totalChange;

    private Slider slider;
    private StoredEnergy wattChild;
    
    float oldValue;
    float defaultPosition;

    string connectId;
    bool cansync;

    private void Start()
    {
        slider = GetComponent<Slider>();
        oldValue = slider.value;

        wattChild = transform.Find("Handle Slide Area").Find("Watt").GetComponent<StoredEnergy>();
        wattChild.capacity = GetComponent<StoredEnergy>().capacity;
        wattChild.energyToReduce = GetComponent<StoredEnergy>().energyToReduce;

        bool isEmptyOrWhiteSpace = string.IsNullOrEmpty(connectId) || (connectId.Trim().Length == 0);
        if (!isEmptyOrWhiteSpace)
        {
            Connector.Instance.AddSliderConnection(connectId, slider);
            cansync = true;
        }
        
    }

    public void CalculateChange()
    {
        float currentValue = slider.value;
        float change = Mathf.Abs(currentValue - oldValue);
        oldValue = currentValue;

        totalChange += change;
        GetComponentInChildren<Generator>().GetSliderChange(totalChange);

        if(cansync)
        {
            Connector.Instance.SyncSliders(slider, connectId);
        }
    }

    public void SetDefaultPosition(float _value)
    {
        slider = GetComponent<Slider>();
        defaultPosition = _value;
        slider.value = _value;
        oldValue = slider.value;
    }

    public float GetDefaultPosition()
    {
        return defaultPosition;
    }

    public void SetConnectID (string id)
    {
        connectId = id;
    }

    public string GetConnectID()
    {
        return connectId;
    }
}
