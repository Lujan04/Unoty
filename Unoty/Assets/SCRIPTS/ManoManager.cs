using UnityEngine;
using System.Collections.Generic;

public class ManoManager : MonoBehaviour
{
    // Listas de cartas
    public List<GameObject> mano = new List<GameObject>();
    public List<GameObject> manoEnemiga = new List<GameObject>();
    public List<GameObject> Game = new List<GameObject>();
    public List<GameObject> ListaPrefabsCartas = new List<GameObject>();

    // Posiciones de las manos
    private Vector3[] posicionesMano = new Vector3[]
    {
        new Vector3(-3.7f, -3.33f, 3), new Vector3(-2.45f, -3.33f, 3),
        new Vector3(-1.2f, -3.33f, 3), new Vector3(0.05f, -3.33f, 3),
        new Vector3(1.3f, -3.33f, 3), new Vector3(2.55f, -3.33f, 3),
        new Vector3(3.8f, -3.33f, 3)
    };

    private Vector3[] posicionesManoEnemiga = new Vector3[]
    {
        new Vector3(-3.7f, 3.2f, 3), new Vector3(-2.45f, 3.2f, 3),
        new Vector3(-1.2f, 3.2f, 3), new Vector3(0.05f, 3.2f, 3),
        new Vector3(1.3f, 3.2f, 3), new Vector3(2.55f, 3.2f, 3),
        new Vector3(3.8f, 3.2f, 3)
    };

    // Variables internas
    private int indexCarta = 0;
    private int layerCarta = 1;

    // Nombres de las cartas
    private string[] NombresCartas = new string[]
    {
        "Blue_0", "Blue_1", "Blue_2", "Blue_3", "Blue_4", "Blue_5", "Blue_6", "Blue_7", "Blue_8", "Blue_9",
        "Blue_Draw", "Blue_Reverse", "Blue_Skip",
        "Green_0", "Green_1", "Green_2", "Green_3", "Green_4", "Green_5", "Green_6", "Green_7", "Green_8", "Green_9",
        "Green_Draw", "Green_Reverse", "Green_Skip",
        "Red_0", "Red_1", "Red_2", "Red_3", "Red_4", "Red_5", "Red_6", "Red_7", "Red_8", "Red_9",
        "Red_Draw", "Red_Reverse", "Red_Skip",
        "Yellow_0", "Yellow_1", "Yellow_2", "Yellow_3", "Yellow_4", "Yellow_5", "Yellow_6", "Yellow_7", "Yellow_8", "Yellow_9",
        "Yellow_Draw", "Yellow_Reverse", "Yellow_Skip",
        "Plus4", "ChangeColor", "Back"
    };

    // Métodos principales
    private void LoadPrefabs()
    {
        ListaPrefabsCartas.Clear();

        foreach (string cardName in NombresCartas)
        {
            GameObject cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardName}");
            if (cardPrefab != null)
            {
                cardPrefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
                ListaPrefabsCartas.Add(cardPrefab);
            }
            else
            {
                Debug.LogError($"No se encontró el prefab '{cardName}'");
            }
        }

        Debug.Log("Prefabs cargados correctamente.");
    }

    public void SpawnCard()
    {
        if (ListaPrefabsCartas.Count > 0 && mano.Count < posicionesMano.Length)
        {
            GameObject cartaAleatoria = ListaPrefabsCartas[Random.Range(0, ListaPrefabsCartas.Count - 1)];
            GameObject cartaInstanciada = Instantiate(cartaAleatoria, posicionesMano[mano.Count], Quaternion.identity);
            mano.Add(cartaInstanciada);
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano está llena.");
        }
    }

    public void SpawnCardEnemiga()
    {
        if (ListaPrefabsCartas.Count > 0 && manoEnemiga.Count < posicionesManoEnemiga.Length)
        {

            GameObject cartaBack = ListaPrefabsCartas[ListaPrefabsCartas.Count - 1];
            GameObject cartaInstanciadaBack = Instantiate(cartaBack, posicionesManoEnemiga[manoEnemiga.Count], Quaternion.identity);


            //GameObject cartaAleatoria = ListaPrefabsCartas[Random.Range(0, ListaPrefabsCartas.Count - 1)];
            //GameObject cartaInstanciada = Instantiate(cartaAleatoria, posicionesManoEnemiga[manoEnemiga.Count], Quaternion.identity);
            manoEnemiga.Add(cartaInstanciadaBack);
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano enemiga está llena.");
        }
    }

    public void OnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mano.Count < posicionesMano.Length)
            {
                Debug.Log("Carta añadida.");
                SpawnCard();
            }
            else
            {
                Debug.Log("La mano está llena.");
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Carta eliminada.");
            TirarCarta();
        }
    }

    public void InitManos()
    {
        for (int i = 0; i < 7; i++)
        {
            SpawnCard();
            SpawnCardEnemiga();
        }
    }

    public void SelectCarta()
    {
        if (mano.Count == 0) return;

        mano[indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

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
        mano[indexCarta].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        indexCarta = (indexCarta + direccion + mano.Count) % mano.Count;
        mano[indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
    }

    public void TirarCarta()
    {
        Vector3 posicionJuego = new Vector3(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            1
        );

        GameObject cartaTirada = Instantiate(mano[indexCarta], posicionJuego, Quaternion.Euler(0f, 0f, Random.Range(-10f, 20f)));
        cartaTirada.GetComponent<SpriteRenderer>().sortingOrder = layerCarta++;
        Game.Add(cartaTirada);

        if (Game.Count > 15)
        {
            Destroy(Game[0]);
            Game.RemoveAt(0);
        }

        Destroy(mano[indexCarta]);
        mano.RemoveAt(indexCarta);
        indexCarta = (indexCarta - 1 + mano.Count) % mano.Count;
        ReorganizeCards();
    }

    private void ReorganizeCards()
    {
        for (int i = 0; i < mano.Count; i++)
        {
            mano[i].transform.position = posicionesMano[i];
        }
    }

    // Ciclos de Unity
    void Start()
    {
        mano.Clear();
        manoEnemiga.Clear();
        LoadPrefabs();
        InitManos();
    }

    void Update()
    {
        SelectCarta();
        OnKeyPress();
    }
}


//TODO IMPLEMENTAR LOGICA COINCIDENCIA COLOR/NUMERO CON CLASE CARD Y DECK
//MENSAJE DE ERROR SI NO HAY COINCIDENCIA