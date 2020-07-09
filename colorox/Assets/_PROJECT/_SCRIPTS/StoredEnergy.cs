using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoredEnergy : MonoBehaviour {

    public ELEMENT elementType;
    public COLORCODE elementColor;

    public float capacity;
    public float currentEnergy;
    public float energyToReduce;

    Image fillImage;
    string colorString;

    public bool canShootAgain;
    public bool canSetColor;

    private void Awake()
    {
        fillImage = GetComponent<Image>();

        canShootAgain = true;
        if(elementType == ELEMENT.WATT)
        {
            canSetColor = true;
        }

        if (elementType == ELEMENT.GENERATOR)
        {
            currentEnergy = capacity;
        }

        colorString = "";
        SetColorString();
        SetColor();
    }

    public void AddEnergy (float energy, string energyColor)
    {
        if(currentEnergy < capacity)
        {
            currentEnergy += energy;
            if(currentEnergy > capacity)
            {
                currentEnergy = capacity;
            }
            if(elementType == ELEMENT.WATT && canSetColor)
            {
                SetColor(energyColor);
            }
            StartCoroutine(AddEnergySmooth());
        }
    }

    public void ReduceEnergy()
    {
        if(currentEnergy > 0 && canShootAgain)
        {
            if(currentEnergy >= energyToReduce)
            {
                currentEnergy -= energyToReduce;
            }
            else if(currentEnergy < energyToReduce)
            {
                currentEnergy -= currentEnergy;
            }
            if (elementType == ELEMENT.WATT && currentEnergy <= 0)
            {
                colorString = "";
            }
            StartCoroutine(ReduceEnergySmooth());
        }
    }

    IEnumerator ReduceEnergySmooth ()
    {
        canShootAgain = false;
        while(fillImage.fillAmount > (currentEnergy / capacity))
        {
            fillImage.fillAmount -= 0.03f;
            yield return new WaitForSeconds(0.000000000001f);
        }
        canShootAgain = true;
    }

    IEnumerator AddEnergySmooth()
    {
        canShootAgain = false;
        while (fillImage.fillAmount < (currentEnergy / capacity))
        {
            fillImage.fillAmount += 0.03f;
            yield return new WaitForSeconds(0.0000000000001f);
        }
        canShootAgain = true;
    }

    public string GetColor()
    {
        string colorToGet = "";
        switch ((int)elementColor)
        {
            case 0:
                colorToGet = "a";
                break;
            case 1:
                colorToGet = "b";
                break;
            case 2:
                colorToGet = "c";
                break;
            case 3:
                colorToGet = "ab";
                break;
            case 4:
                colorToGet = "bc";
                break;
            case 5:
                colorToGet = "ca";
                break;
            case 6:
                colorToGet = colorString;
                break;
        }

        return colorToGet;
    }

    public void SetColor(string code)
    {
        if(elementType == ELEMENT.WATT && colorString != null && code != null)
        {
            if ((colorString.Length < 2 && code.Length <= 1) && colorString != code)
            {
                colorString += code;
            }
            if(colorString.Length <= 0 && code.Length == 2 && colorString != code)
            {
                colorString = code;
            }
            if (currentEnergy <= 0)
            {
                colorString = "";
            }
            if(colorString == "ab" || colorString == "ba")
            {
                colorString = "ab";
            }
            if (colorString == "bc" || colorString == "cb")
            {
                colorString = "bc";
            }
            if (colorString == "ac" || colorString == "ca")
            {
                colorString = "ca";
            }
        }
        fillImage.color = RequiredColorRGBValues.GetColorFromString(colorString);
        canSetColor = false;
    }

    public void SetColor()
    {
        fillImage.color = RequiredColorRGBValues.GetColorFromString(colorString);
    }

    public void SetColorString()
    {
        colorString = GetColor();
    }
}

public enum COLORCODE
{
    RED,
    YELLOW,
    BLUE,
    ORANGE,
    GREEN,
    VIOLET,
    CHANGABLE
}

public enum ELEMENT
{
    GENERATOR,
    WATT,
    COLLECTOR,
    WATTSLIDER
}
