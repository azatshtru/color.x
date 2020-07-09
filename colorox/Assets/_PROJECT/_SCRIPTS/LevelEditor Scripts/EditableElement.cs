using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditableElement : MonoBehaviour {

    public COLORCODE e_ElementColor;
    public ELEMENT e_type;
    public StoredEnergy e_ElementType;

    public float e_Capacity;
    public float e_EnergyToReduce;
    public string e_connectID;

    LevelCreator levelCreator;
    StoredEnergy correspondingElement;

    private float defaultSliderValue;

    private void Start()
    {
        levelCreator = FindObjectOfType<LevelCreator>();
    }

    public void SpawnPlayableElements()
    {
        StoredEnergy elementGO = Instantiate(e_ElementType, transform.position, transform.rotation);
        elementGO.transform.SetParent(levelCreator.elementHolder.transform);

        #region special element conditions

        elementGO.capacity = e_Capacity;
        if(elementGO.elementType == ELEMENT.GENERATOR)
        {
            elementGO.currentEnergy = elementGO.capacity;
        }
        if(elementGO.elementType != ELEMENT.COLLECTOR)
        {
            elementGO.energyToReduce = e_EnergyToReduce;
        }
        if(elementGO.elementType != ELEMENT.WATT)
        {
            elementGO.elementColor = e_ElementColor;
            elementGO.SetColorString();
            elementGO.SetColor();
        }
        if(elementGO.elementType == ELEMENT.WATTSLIDER)
        {
            elementGO.GetComponent<RectTransform>().sizeDelta = GetComponent<RectTransform>().sizeDelta;
            elementGO.GetComponentInChildren<Generator>().transform.rotation = GetComponentInChildren<Generator>().transform.rotation;
            defaultSliderValue = GetComponent<Slider>().value;
            elementGO.GetComponent<WattSlider>().SetDefaultPosition(defaultSliderValue);
            elementGO.GetComponent<WattSlider>().SetConnectID(e_connectID);
        }

#endregion

        correspondingElement = elementGO;

        gameObject.SetActive(false);
    }

    public void RemoveCorrespondingElement()
    {
        if(correspondingElement != null)
        {
            Destroy(correspondingElement.gameObject);
        }
    }

    private void OnMouseUp()
    {
        if(levelCreator.CheckEditorWindowClosed() == true)
        {
            levelCreator.EnableEditMoveMenu(this);

            levelCreator.SetElementEditor();
        }
    }
}
