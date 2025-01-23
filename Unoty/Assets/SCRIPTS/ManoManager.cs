using UnityEngine;
using System.Collections.Generic;
using static UnityEngine.Rendering.GPUSort;

public class ManoManager : MonoBehaviour
{
    public List<GameObject> mano = new List<GameObject>();
    Vector3 PosMano = new Vector3(-3.7f, -3.33f, 3);
    Vector3 Separacion = new Vector3(1.25f, 0, 0);


    private Vector3[] posicionesMano = new Vector3[]
  {
      
  };


    public List<GameObject> ListaPrefabsCartas = new List<GameObject>();



    // Nombres de los prefabs de las cartas
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
            SpriteRenderer spriteRenderer = cardPrefab.GetComponent<SpriteRenderer>();
            spriteRenderer.sortingOrder = 2;


            if (cardPrefab != null)
            {
                ListaPrefabsCartas.Add(cardPrefab);
            }
            else
            {
                Debug.LogError($"No se encontró el prefab '{cardName}'");
            }
        }

        Debug.Log("Prefabs cargados correctamente.");
    }

    private void SpawnCard()
    {
        if (ListaPrefabsCartas.Count > 0)
        {
            GameObject cartaAleatoria = ListaPrefabsCartas[Random.Range(0, ListaPrefabsCartas.Count)];
            Instantiate(cartaAleatoria, PosMano, Quaternion.identity);
            mano.Add(cartaAleatoria);
            PosMano = PosMano + Separacion;
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista.");
        }
    }

    // Manejo de teclas
    public void OnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.Space) && mano.Count <= 6)
        {
            Debug.Log("Carta añadida.");
            SpawnCard();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("La mano está llena.");
        } else if (Input.GetKeyDown(KeyCode.R)) { 
            mano.
        }
    }

    void Start()
    {
        mano.Clear();
        LoadPrefabs();  
    }

    void Update()
    {
        OnKeyPress();
    }
}
