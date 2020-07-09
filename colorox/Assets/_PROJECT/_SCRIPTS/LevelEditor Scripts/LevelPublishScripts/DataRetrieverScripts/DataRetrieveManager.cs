using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Firebase;
using Firebase.Database;

public class DataRetrieveManager : MonoBehaviour {

    public Transform levelTemplates;

    Button[] levelTemplateButtons;

    LevelData retrievedData;

    private void Start()
    {
        levelTemplateButtons = levelTemplates.GetComponentsInChildren<Button>();

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

                 int i = 0;
                 foreach (DataSnapshot player in snapshot.Children)
                 {
                     foreach(DataSnapshot levelName in player.Children)
                     {
                         levelTemplateButtons[i].GetComponentInChildren<Text>().text = levelName.Key;
                         levelTemplateButtons[i].GetComponentInChildren<Text>().gameObject.name = player.Key;
                         levelTemplateButtons[i].gameObject.name = levelName.Key;
                         i += 1;
                     }
                 }
             }
         });
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartLevel()
    {
        GameObject pressedButton = EventSystem.current.currentSelectedGameObject;

        retrievedData = new LevelData();
        retrievedData.creatorName = pressedButton.transform.GetChild(0).gameObject.name;
        retrievedData.levelName = pressedButton.name;

        string json = JsonUtility.ToJson(retrievedData);

        if(File.Exists(Application.persistentDataPath + "/currentLevelData.json"))
        {
            File.Delete(Application.persistentDataPath + "/currentLevelData.json");
        }
        File.WriteAllText(Application.persistentDataPath + "/currentLevelData.json", json);

        SceneManager.LoadScene("Level");
    }
}

public class LevelData
{
    public string creatorName;
    public string levelName;
}
