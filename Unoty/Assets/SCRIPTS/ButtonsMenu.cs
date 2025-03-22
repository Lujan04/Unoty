using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public void cargarPartida()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("Load");
    }

    public void nuevaPartida()
    {
        Debug.Log("Play");
        SceneManager.LoadScene("Game");
    }

    public void salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}