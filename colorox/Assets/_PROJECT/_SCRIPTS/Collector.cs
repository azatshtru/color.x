using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StoredEnergy))]
public class Collector : MonoBehaviour {

    StoredEnergy energyComponent;

    float collectedEnergy;
    string colorOfEnergy;

    bool isFilled;

    private void Start()
    {
        GameManager.instance.collectorsInLevel += 1;
        energyComponent = GetComponent<StoredEnergy>();
    }

    private void Update()
    {
        if (energyComponent.canShootAgain)
        {
            energyComponent.AddEnergy(collectedEnergy, colorOfEnergy);
            collectedEnergy = 0;
        }

        if (energyComponent.currentEnergy >= energyComponent.capacity && !isFilled)
        {
            GameManager.instance.collectorsFilled += 1;
            GameManager.instance.OnCollectorFilled();
            isFilled = true;
        }
    }

    public void CollectEnergy (float energy, string energyColor)
    {
        collectedEnergy += energy;
        colorOfEnergy = energyColor;
        energyComponent.canSetColor = true;
    }

    public bool CheckFilled()
    {
        if (energyComponent.currentEnergy >= energyComponent.capacity)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckColorMatch(string colorString)
    {
        string colorMatchString = energyComponent.GetColor();

        if(colorString != colorMatchString && energyComponent.elementType == ELEMENT.COLLECTOR)
        {
            return false;
        }
        if (colorString.Length >= 2 && colorMatchString.Length == 1 && energyComponent.elementType == ELEMENT.WATT)
        {
            return false;
        }

        return true;
    }
}
