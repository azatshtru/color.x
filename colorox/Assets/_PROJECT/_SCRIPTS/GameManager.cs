using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public int collectorsInLevel = 0;
    public int collectorsFilled = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollectorFilled()
    {
        if (collectorsFilled == collectorsInLevel)
        {
            print("levelCompleted"); //Put actual code here with level completing animation.
        }
    }
}
