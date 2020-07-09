using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StoredEnergy))]
public class Generator : MonoBehaviour {
    
    StoredEnergy energyComponent;
    float sliderChange;

    private void Start()
    {
        energyComponent = GetComponent<StoredEnergy>();
    }
    //Calculate the slider value change, then shoot if change < 0.1 and don't shoot if more than 0.1. This is the way to solve the slider problem.
    private void OnMouseUp()
    {
        if(sliderChange < 0.05f)
        {
            if (energyComponent.currentEnergy > 0 && energyComponent.canShootAgain)
            {
                GameObject energy = (GameObject)Instantiate(Resources.Load("Energy"), transform.position, Quaternion.identity);
                energy.GetComponent<Energy>().SetEnergy(energyComponent.energyToReduce, energyComponent.GetColor(), gameObject);
            }
            energyComponent.ReduceEnergy();
        }
        if (GetComponentInParent<WattSlider>())
        {
            GetComponentInParent<WattSlider>().totalChange = 0;
            GetSliderChange(0);
        }
    }

    public void GetSliderChange(float change)
    {
        sliderChange = change;
    }
}
