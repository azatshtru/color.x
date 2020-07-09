using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour {

    float energyAmount;
    string energyColor;

    GameObject author;
    Vector3 direction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collector>() && other.gameObject != author)
        {
            if(other.GetComponent<Collector>().CheckFilled() == false && other.GetComponent<Collector>().CheckColorMatch(energyColor) == true)
            {
                other.GetComponent<Collector>().CollectEnergy(energyAmount, energyColor);
                Destroy(gameObject);
            }
        }
    }

    private void Update()
    {
        transform.Translate(direction * Time.deltaTime * 6f);
        //Remember to write shoot amount on arrow of direction of elements to give clear direction about shootage, shoot amount = reduce amount.
    }

    public void SetEnergy (float energy, string _color, GameObject authority)
    {
        energyAmount = energy;
        energyColor = _color;

        GetComponent<Renderer>().material.color = RequiredColorRGBValues.GetColorFromString(energyColor);

        author = authority;
        direction = (author.transform.GetChild(0).position - transform.position).normalized;
    }
}
