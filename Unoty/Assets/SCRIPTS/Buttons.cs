using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public void cargarPartida()
    {
        Debug.Log("Cargar partida");
    }

    public void nuevaPartida()
    {
        Debug.Log("Load");
        SceneManager.LoadScene("Game");
    }

    public void salir()
    {
        Debug.Log("Salir");
        Application.Quit();
    }
}