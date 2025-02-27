using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEditor.Rendering;


public class ManoManager : MonoBehaviour
{

    public static ManoManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public void CambiarTurno() {
        
        Constants.Instance.cartaTiradaTurno= !Constants.Instance.cartaTiradaTurno;
    }

    public void OnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Solo permitir robar una carta si aún no se ha robado en este turno
            if (Constants.Instance.mano.Count < Constants.Instance.posicionesMano.Length)
            {
                CardSpawner.Instance.SpawnCard();
                
            }
            else if (Constants.Instance.mano.Count >= Constants.Instance.posicionesMano.Length)
            {
                StartCoroutine(UI_Manager.Instance.warningLimiteCartas());
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            // Solo permitir tirar una carta si aún no se ha tirado una en este turno
            if (!Constants.Instance.cartaTiradaTurno)
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
    }
}