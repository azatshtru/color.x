using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublishElement : MonoBehaviour {

    public int objectId;

    private StoredEnergy dataClass;
    private PublishManager manager;

    // Use this for initialization
    void Start () {
        dataClass = GetComponent<StoredEnergy>();
        manager = FindObjectOfType<PublishManager>();
	}
	
	public void WriteElementData()
    {
        manager.UploadData(objectId, dataClass.elementType, dataClass.elementColor, dataClass.capacity, dataClass.energyToReduce, dataClass.transform.position, dataClass.transform.rotation);
        if(dataClass.elementType == ELEMENT.WATTSLIDER)
        {
            manager.UploadData(objectId, dataClass.elementType, dataClass.elementColor, dataClass.capacity, dataClass.energyToReduce, dataClass.transform.position, dataClass.transform.rotation, dataClass.GetComponent<RectTransform>().rect.width, dataClass.transform.Find("Handle Slide Area").Find("Watt").rotation, dataClass.gameObject.GetComponent<WattSlider>().GetDefaultPosition(), dataClass.GetComponent<WattSlider>().GetConnectID());
        }
    }
}
