using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Database;

public class LevelAssembler : MonoBehaviour {

    public Transform elementHolder;

    [Header("Spawnable Elements")]
    public GameObject generatorPrefabToSpawn;
    public GameObject wattPrefabToSpawn;
    public GameObject collectorPrefabToSpawn;
    public GameObject sliderPrefabToSpawn;

    LevelData levelData;
    int numberOfElements;

    // Use this for initialization
    void Start () {
        string json = File.ReadAllText(Application.persistentDataPath + "/currentLevelData.json");

        levelData = JsonUtility.FromJson<LevelData>(json);

        FirebaseDatabase.DefaultInstance
         .GetReference("levels")
         .GetValueAsync().ContinueWith(task =>
         {
             if (task.IsFaulted)
             {
                 Debug.LogError("Cannot retrieve data.");
             }
             else if (task.IsCompleted)
             {
                 DataSnapshot snapshot = task.Result;

                 long numberOfElementsLong = snapshot.Child(levelData.creatorName).Child(levelData.levelName).ChildrenCount;
                 numberOfElements = (int)numberOfElementsLong;

                 for (int i = 0; i < numberOfElements; i++)
                 {
                     GameObject elementGO = (GameObject)Instantiate(Resources.Load("AssemblyElement"));
                     AssemblyElement element = elementGO.GetComponent<AssemblyElement>();
                     element.elementId = i.ToString();
                     element.creatorPathName = levelData.creatorName;
                     element.levelPathName = levelData.levelName;
                     element.assembler = this;
                 }
             }
         });
    }

    public StoredEnergy SpawnElementToScene (ELEMENT elementToSpawn)
    {
        GameObject elementGO;
        switch ((int)elementToSpawn)
        {
            case 0:
                elementGO = Instantiate(generatorPrefabToSpawn.gameObject);
                break;

            case 1:
                elementGO = Instantiate(wattPrefabToSpawn.gameObject);
                break;

            case 2:
                elementGO = Instantiate(collectorPrefabToSpawn.gameObject);
                break;

            case 3:
                elementGO = Instantiate(sliderPrefabToSpawn.gameObject);
                break;

            default:
                elementGO = Instantiate(generatorPrefabToSpawn.gameObject);
                break;
        }
        StoredEnergy element = elementGO.GetComponent<StoredEnergy>();
        return element;
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
