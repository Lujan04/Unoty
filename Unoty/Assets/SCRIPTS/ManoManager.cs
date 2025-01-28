using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.GPUSort;

public class ManoManager : MonoBehaviour
{
    public List<GameObject> mano = new List<GameObject>();
    Vector3[] posicionesMano = new Vector3[]
    {
        new Vector3(-3.7f, -3.33f, 3),
        new Vector3(-2.45f, -3.33f, 3),
        new Vector3(-1.2f, -3.33f, 3),
        new Vector3(0.05f, -3.33f, 3),
        new Vector3(1.3f, -3.33f, 3),
        new Vector3(2.55f, -3.33f, 3),
        new Vector3(3.8f, -3.33f, 3)
    };
    int indexCarta = 0;
    int layerCarta = 1;

    public List<GameObject> ListaPrefabsCartas = new List<GameObject>();

    private string[] NombresCartas = new string[]
    {
        "Blue_0_0", "Blue_1_0", "Blue_2_0", "Blue_3_0", "Blue_4_0", "Blue_5_0", "Blue_6_0", "Blue_7_0", "Blue_8_0", "Blue_9_0",
        "Blue_Draw_0", "Blue_Reverse_0", "Blue_Skip_0",
        "Green_0_0", "Green_1_0", "Green_2_0", "Green_3_0", "Green_4_0", "Green_5_0", "Green_6_0", "Green_7_0", "Green_8_0", "Green_9_0",
        "Green_Draw_0", "Green_Reverse_0", "Green_Skip_0",
        "Red_0_0", "Red_1_0", "Red_2_0", "Red_3_0", "Red_4_0", "Red_5_0", "Red_6_0", "Red_7_0", "Red_8_0", "Red_9_0",
        "Red_Draw_0", "Red_Reverse_0", "Red_Skip_0",
        "Yellow_0_0", "Yellow_1_0", "Yellow_2_0", "Yellow_3_0", "Yellow_4_0", "Yellow_5_0", "Yellow_6_0", "Yellow_7_0", "Yellow_8_0", "Yellow_9_0",
        "Yellow_Draw_0", "Yellow_Reverse_0", "Yellow_Skip_0",
        "Plus4", "ChangeColor"
    };

    private void LoadPrefabs()
    {
        ListaPrefabsCartas.Clear();

        foreach (string cardName in NombresCartas)
        {
            GameObject cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardName}");
            if (cardPrefab != null)
            {
                SpriteRenderer spriteRenderer = cardPrefab.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 2;
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
            GameObject cartaAleatoria = ListaPrefabsCartas[Random.Range(0, ListaPrefabsCartas.Count)];
            GameObject cartaInstanciada = Instantiate(cartaAleatoria, posicionesMano[mano.Count], Quaternion.identity);
            mano.Add(cartaInstanciada);
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano está llena.");
        }
    }

    public void OnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space) && mano.Count < posicionesMano.Length)
        {
            Debug.Log("Carta añadida.");
            SpawnCard();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("La mano está llena.");
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Carta eliminada.");
            tirarCarta();
        }
    }

    public void initMano()
    {
        for (int i = 0; i < 7; i++)
        {
            SpawnCard();
        }
    }

    public void selectCarta()
    {
        if (mano.Count == 0) return;

        mano[indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mano[indexCarta].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            indexCarta = (indexCarta + 1) % mano.Count;
            mano[indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            mano[indexCarta].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            indexCarta = (indexCarta - 1 + mano.Count) % mano.Count;
            mano[indexCarta].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        }
    }

    public void tirarCarta()
    {
        int Zposicionjuego = layerCarta;

        Vector3 posicionJuego = new Vector3(
            Random.Range(0f, 1.5f),
            Random.Range(0f, 1.5f),
            1
        );
        Vector3 rotacionCarta = new Vector3(
                  0,
                  0,
                  Random.Range(0f, 100f)
              );



        //Debug.Log("Antes: " + mano[indexCarta].transform.position);

        GameObject cartaTirada = Instantiate(mano[indexCarta], posicionJuego, Quaternion.Euler(0f, 0f, Random.Range(-10f, 20f)));
        SpriteRenderer spriteRenderer = cartaTirada.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingOrder = Zposicionjuego;

        //mano[indexCarta].transform.position = posicionJuego;
        //mano[indexCarta].transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-10f, 20f));
        //Debug.Log("Despues: " + mano[indexCarta].transform.position);

        Destroy(mano[indexCarta]);
        mano.RemoveAt(indexCarta);
        indexCarta = (indexCarta - 1 + mano.Count) % mano.Count;
        ReorganizeCards();
        layerCarta++;
    }

    private void ReorganizeCards()
    {
        for (int i = 0; i < mano.Count; i++)
        {
            mano[i].transform.position = posicionesMano[i];
        }
    }

    void Start()
    {
        mano.Clear();
        LoadPrefabs();
        initMano();
    }

    void Update()
    {
        selectCarta();
        OnKeyPress();
    }
}