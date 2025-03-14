using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsMenuIngame : MonoBehaviour
{

    public void salirMenuInicial() {

        Debug.Log("Salir a Menu Scene");
        SceneManager.LoadScene("Menu_inicial");
        GameManager.Instance.Menu.SetActive(false);

    }

    public void ResetGame() {

        Debug.Log("Reset game");
        //StartCoroutine(GameManager.Instance.ResetGame());
        GameManager.Instance.ResetGame();
        GameManager.Instance.showMenuButton();
        GameManager.Instance.Menu.SetActive(false);
        ManoManager.Instance.CambiarTurno();
    }

    public void VolverJuego() {
        Debug.Log("Volver al juego");
        GameManager.Instance.showMenuButton();
        GameManager.Instance.Menu.SetActive(false);


    }


}
