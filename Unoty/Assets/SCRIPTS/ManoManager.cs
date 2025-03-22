using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEditor;


public class ManoManager : MonoBehaviour
{

    public static ManoManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void CambiarTurno()
    {
        Constants.Instance.cartaTiradaTurno = !Constants.Instance.cartaTiradaTurno;
        GameManager.Instance.skipTurn = false; // Resetear estado de skip
        Debug.Log("Turno cambiado. skipTurn = " + GameManager.Instance.skipTurn);
    }

    public void OnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Solo permitir robar si es el turno del jugador y no ha robado/tirado carta
            if (!UI_Manager.Instance.isTurnoIA && !Constants.Instance.cartaTiradaTurno)
            {
                if (Constants.Instance.mano.Count < Constants.Instance.posicionesMano.Length)
                {
                    CardSpawner.Instance.DrawCard();
                }
                else
                {
                    //StartCoroutine(UI_Manager.Instance.warningLimiteCartas());
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (!UI_Manager.Instance.isTurnoIA && !Constants.Instance.cartaTiradaTurno)
            {
                GameManager.Instance.ComprobarCarta();
            }
        }
    }

    public void SelectCarta()
    {
        if (Constants.Instance.mano.Count == 0) return;

        Constants.Instance.mano[Constants.Instance.indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            CambiarSeleccionCarta(1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            CambiarSeleccionCarta(-1);
        }
    }

    private void CambiarSeleccionCarta(int direccion)
    {
        Constants.Instance.mano[Constants.Instance.indexCarta].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        Constants.Instance.indexCarta = (Constants.Instance.indexCarta + direccion + Constants.Instance.mano.Count) % Constants.Instance.mano.Count;
        Constants.Instance.mano[Constants.Instance.indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
    }

    public void ReorganizeCards()
    {
        for (int i = 0; i < Constants.Instance.mano.Count; i++)
        {
            Constants.Instance.mano[i].transform.position = Constants.Instance.posicionesMano[i];
        }
    }


    public void ReorganizeCardsEnemiga()
    {
        for (int i = 0; i < Constants.Instance.manoEnemiga.Count; i++)
        {
            Constants.Instance.manoEnemiga[i].transform.position = Constants.Instance.posicionesManoEnemiga[i];
        }
    }


    public IEnumerator perderTurnoManoLlena()
    {

        if (GameManager.Instance.isWiner) yield break;

        bool manoJugadorLlena = Constants.Instance.mano.Count == Constants.Instance.posicionesMano.Length;
        bool manoEnemigaLlena = Constants.Instance.manoEnemiga.Count == Constants.Instance.posicionesManoEnemiga.Length;


        if (manoJugadorLlena)
        {
            GameManager.Instance.winIA.SetActive(true);
            yield return new WaitForSeconds(2f);
            GameManager.Instance.winIA.SetActive(false);

            GameManager.Instance.isWiner = true;
            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.Menu.SetActive(true);

        }
        else if (manoEnemigaLlena) { 
        
        

            GameManager.Instance.winJugador.SetActive(true);
            yield return new WaitForSeconds(2f);
            GameManager.Instance.winJugador.SetActive(false);

            GameManager.Instance.isWiner = true;
            yield return new WaitForSeconds(0.5f);

            GameManager.Instance.Menu.SetActive(true);

        
        
        }
        
    }




    void Start()
    {
        Constants.Instance.mano.Clear();
        Constants.Instance.manoEnemiga.Clear();
        Constants.Instance.Game.Clear();

        CardSpawner.Instance.LoadPrefabs();
        CardSpawner.Instance.InitManos();


    }

    void Update()
    {
        GameManager.Instance.checkWinner();
        SelectCarta();
        OnKeyPress();
        
        if (!GameManager.Instance.isWiner)
        {
            StartCoroutine(perderTurnoManoLlena());
        }
    }
}