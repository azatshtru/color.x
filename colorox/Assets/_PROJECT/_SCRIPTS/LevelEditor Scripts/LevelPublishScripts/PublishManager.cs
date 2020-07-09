using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Database;

//Publish Button Cannot be pressed without player name. Interactable = false. (Don't forget to make this to avoid unnamed players.)
public class PublishManager : MonoBehaviour {

    public InputField levelNameField;
    public InputField playerNameField;
    public Button publishButton;

    public TextAsset apiKeyFile;

    public PublishElement[] publishElements;

    DatabaseReference reference;
    Animator anim;

    private string url;

    private string playerKeyName;
    private string visiblePlayerName;
    private string levelName;

    private void Start()
    {
        anim = GetComponent<Animator>();

        url = GetDatabaseUrl();

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(url);
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        playerKeyName = PublishController.GetUniquePlayerName();
        visiblePlayerName = PublishController.GetVisiblePlayerName();
        if(visiblePlayerName != null)
        {
            playerNameField.image.color = Color.black;
            playerNameField.text = visiblePlayerName;
            playerNameField.transform.Find("Text").GetComponent<Text>().color = Color.white;
            playerNameField.interactable = false;
        }

        if(playerKeyName == null)
        {
            publishButton.interactable = false;
        }
    }

#region polymorph upload
    public void UploadData(int objectId, ELEMENT _elementType, COLORCODE _color, float _capacity, float _shootAmount, Vector3 _position, Quaternion _rotation)
    {
        ElementData data = new ElementData();
        data.e_dataType = _elementType;
        data.e_dataColor = _color;
        data.e_dataCapacity = _capacity;
        data.e_dataEnergyToReduce = _shootAmount;
        data.e_dataPosition = _position;
        data.e_dataRotation = _rotation;

        string objectIdentifier = objectId.ToString();
        string json = JsonUtility.ToJson(data);

        reference.Child("levels").Child(playerKeyName).Child(levelName + " by " + visiblePlayerName).Child(objectIdentifier).SetRawJsonValueAsync(json);
    }

    public void UploadData(int objectId, ELEMENT _elementType, COLORCODE _color, float _capacity, float _shootAmount, Vector3 _position, Quaternion _rotation, float _width, Quaternion _wRotation, float _defaultLocation, string _connectId)
    {
        ElementData data = new ElementData();
        data.e_dataType = _elementType;
        data.e_dataColor = _color;
        data.e_dataCapacity = _capacity;
        data.e_dataEnergyToReduce = _shootAmount;
        data.e_dataPosition = _position;
        data.e_dataRotation = _rotation;
        data.e_width = _width;
        data.e_wRotation = _wRotation;
        data.e_defaultLocation = _defaultLocation;
        data.e_connectId = _connectId;

        string objectIdentifier = objectId.ToString();
        string json = JsonUtility.ToJson(data);

        reference.Child("levels").Child(playerKeyName).Child(levelName + " by " + visiblePlayerName).Child(objectIdentifier).SetRawJsonValueAsync(json);
    }
    #endregion

    public void SetPublishElements()
    {
        publishElements = FindObjectsOfType<PublishElement>();
    }

    public void ClosePublishWindow()
    {
        anim.SetTrigger("Down");
    }

    public void SetLevelName()
    {
        levelName = levelNameField.text;
    }

    public void SetPlayerName()
    {
        playerKeyName = PublishController.GetUniquePlayerName();
        if(playerKeyName == null)
        {
            PublishController.SetUniquePlayerName(playerNameField.text);
            playerKeyName = PublishController.GetUniquePlayerName();
            visiblePlayerName = PublishController.GetVisiblePlayerName();
            if(playerNameField.text != null && playerNameField.text != "")
            {
                publishButton.interactable = true;
            }
        }
    }

    public void PublishLevelButton()
    {
        for (int i = 0; i < publishElements.Length; i++)
        {
            if(publishElements[i] != null)
            {
                publishElements[i].objectId = i;
                publishElements[i].WriteElementData();
            }
        }
    }

    private string GetDatabaseUrl()
    {
        string json = apiKeyFile.text;
        APIKEYS keys = JsonUtility.FromJson<APIKEYS>(json);
        string databaseUrlKey = keys.databaseUrl;
        print(databaseUrlKey);

        return databaseUrlKey;
    }
}

public class APIKEYS
{
    public string databaseUrl;
}
