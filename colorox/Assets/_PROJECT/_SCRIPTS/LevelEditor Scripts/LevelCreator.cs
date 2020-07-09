using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelCreator : MonoBehaviour {

    [Header("Menus And Windows")]
    public ElementEditor elementEditor;
    public GameObject dockButton;
    public GameObject elementHolder;
    public GameObject editMoveMenu;
    public GameObject levelCreatorMenu;
    public GameObject levelPublishMenu;
    public GameObject instructionsMenu;
    public GameObject publishWindow;

    public List<EditableElement> currentElements = new List<EditableElement>();

    [Header("Elements")]
    public EditableElement generatorPrefab;
    public EditableElement collectorPrefab;
    public EditableElement wattPrefab;
    public EditableElement wattSliderPrefab;

    EditableElement elementToEdit;
    Plane plane;
    Animator anim;
    bool isMoving;
    bool editorWindowClosed;

    private void Start()
    {
        plane = new Plane(Vector3.forward, transform.position);
        anim = levelCreatorMenu.GetComponent<Animator>();

        CloseEditorWindowCommand();
    }

    private void Update()
    {
        if (isMoving && Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            float enter = 0;
            if (plane.Raycast(ray, out enter))
            {
                elementToEdit.transform.position = ray.GetPoint(enter);
                levelCreatorMenu.SetActive(true);
                instructionsMenu.transform.Find("MoveInstructions Text").gameObject.SetActive(false);
                levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(true);
                isMoving = false;
            }
        }

        if (isMoving && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float enter = 0;
            if (plane.Raycast(ray, out enter))
            {
                elementToEdit.transform.position = ray.GetPoint(enter);
                levelCreatorMenu.SetActive(true);
                instructionsMenu.transform.Find("MoveInstructions Text").gameObject.SetActive(false);
                levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(true);
                isMoving = false;
            }
        }

        dockButton.transform.position = levelCreatorMenu.transform.Find("Dock Button").position;
    }

    #region create element
    public void CreateGenerator()
    {
        EditableElement elementGO = Instantiate(generatorPrefab, Vector3.zero, Quaternion.identity);
        ElementCreator(elementGO);
    }

    public void CreateCollector()
    {
        EditableElement elementGO = Instantiate(collectorPrefab, Vector3.zero, Quaternion.identity);
        ElementCreator(elementGO);
    }

    public void CreateWatt()
    {
        EditableElement elementGO = Instantiate(wattPrefab, Vector3.zero, Quaternion.identity);
        ElementCreator(elementGO);
    }

    public void CreateWattSlider()
    {
        EditableElement elementGO = Instantiate(wattSliderPrefab, Vector3.zero, Quaternion.identity);
        ElementCreator(elementGO);
    }
#endregion

    private void ElementCreator(EditableElement elementGO)
    {
        elementGO.transform.SetParent(elementHolder.transform);

        currentElements.Add(elementGO);

        EnableEditMoveMenu(elementGO);
        SetElementEditor();
        elementEditor.SetEditableElementColor();
    }

    public void SetElementEditor()
    {
        elementEditor.transform.position = elementToEdit.transform.position + SetEditorWindowPosition(7f, 2.35f);
        elementEditor.SetElementEntity(elementToEdit);
    }

    public void EnableEditMoveMenu(EditableElement elementGO)
    {
        elementToEdit = elementGO;
        editMoveMenu.SetActive(true);
        editMoveMenu.transform.position = elementToEdit.transform.position + SetEditorWindowPosition(1f, 1.74f);
        elementEditor.gameObject.SetActive(false);
    }

    public void EditButton()
    {
        elementEditor.gameObject.SetActive(true);
        editMoveMenu.SetActive(false);
        levelCreatorMenu.SetActive(false);
        CloseDockButton();
        editorWindowClosed = false;
        levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(false);
    }

    public void MoveButton()
    {
        isMoving = true;
        editMoveMenu.SetActive(false);
        elementEditor.gameObject.SetActive(false);
        levelCreatorMenu.SetActive(false);
        CloseDockButton();
        levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(false);
        instructionsMenu.transform.Find("MoveInstructions Text").gameObject.SetActive(true);
    }

    public void TestButton()
    {
        for (int i = 0; i < currentElements.Count; i++)
        {
            if (currentElements[i] != null)
            {
                currentElements[i].SpawnPlayableElements();
            }
            else if (currentElements[i] == null)
            {
                currentElements.Remove(currentElements[i]);
            }
        }
        levelCreatorMenu.SetActive(false);
        CloseDockButton();
        levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(false);
        levelPublishMenu.SetActive(true);
    }

    public void EditLevelButton()
    {
        for (int i = 0; i < currentElements.Count; i++)
        {
            if (currentElements[i] != null)
            {
                currentElements[i].gameObject.SetActive(true);
                currentElements[i].RemoveCorrespondingElement();
            }
        }
        levelCreatorMenu.SetActive(true);
        levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(true);
        levelPublishMenu.SetActive(false);
    }

    public void PublishButton()
    {
        publishWindow.SetActive(true);
        publishWindow.GetComponent<Animator>().SetTrigger("Up");
        publishWindow.GetComponent<PublishManager>().SetPublishElements();
    }

    public void BackgroundButton()
    {
        editMoveMenu.SetActive(false);
    }

    public void OpenDockButton()
    {
        anim.SetTrigger("OpenDock");
        dockButton.SetActive(true);
        levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(false);
    }

    public void CloseDockButton()
    {
        anim.SetTrigger("CloseDock");
        dockButton.SetActive(false);
        levelCreatorMenu.transform.Find("Dock Button").gameObject.SetActive(true);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool CheckEditorWindowClosed()
    {
        if(editorWindowClosed == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CloseEditorWindowCommand()
    {
        editorWindowClosed = true;
    }

    public void GetEditMoveClickDown()
    {
        editorWindowClosed = false;
    }

    private Vector3 SetEditorWindowPosition(float a, float b)
    {
        Vector3 pos = new Vector3();

        if(elementToEdit.transform.position.x < 0)
        {
            //pos += elementToEdit.transform.position + (Vector3.right * a);
            pos += Vector3.zero + (Vector3.right * a);
        }
        if (elementToEdit.transform.position.x >= 0)
        {
            //pos += elementToEdit.transform.position - (Vector3.right * a);
            pos -= Vector3.zero + (Vector3.right * a);
        }

        if (elementToEdit.transform.position.y < 0)
        {
            //pos += (Vector3.up * b);
            pos += Vector3.zero + (Vector3.up * b);
        }
        if (elementToEdit.transform.position.y >= 0)
        {
            //pos -= (Vector3.up * b);
            pos -= Vector3.zero + (Vector3.up * b);
        }

        return pos;
    }
}
