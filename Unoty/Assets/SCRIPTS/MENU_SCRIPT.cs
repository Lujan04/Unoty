using UnityEngine;
using UnityEngine.UI;  

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Button myButton;

    void Start()
    {
        if (myButton != null)
        {
            myButton.onClick.AddListener(OnButtonClick);  
        }
    }

    void OnButtonClick()
    {
        if (myButton.gameObject.tag == "NewGame")
        {
            NewGame();
        }
        else if (myButton.gameObject.tag == "LoadGame")
        {
            LoadGame();
        }
        else if (myButton.gameObject.tag == "CloseGame")
        {
            CloseGame();
        }
        else{
            Debug.Log("Button sin tag");
        } 
    }

    void NewGame()
    {
        Debug.Log("New Game");
    }

    void LoadGame()
    {
        Debug.Log("Load Game");
    }
     void CloseGame()
    {
        Application.Quit();
        Debug.Log("Close Game");

    }
}