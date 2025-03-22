using UnityEngine;

public class CardSpawner : MonoBehaviour
{

    public static CardSpawner Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }
    public void LoadPrefabs()
    {
        Constants.Instance.ListaPrefabsCartas.Clear();

        foreach (string cardName in Constants.Instance.NombresCartas)
        {
            
            GameObject cardPrefab = Resources.Load<GameObject>($"Prefabs/Cards/{cardName}");
            if (cardPrefab != null)
            {
                cardPrefab.GetComponent<SpriteRenderer>().sortingOrder = 2;
                Constants.Instance.ListaPrefabsCartas.Add(cardPrefab);
            }
            else
            {
                Debug.LogError($"No se encontr� el prefab '{cardName}'");
            }
        }

        Debug.Log("Prefabs cargados correctamente.");
    }

    public void SpawnCard()
    {
        if (Constants.Instance.ListaPrefabsCartas.Count > 0 && Constants.Instance.mano.Count < Constants.Instance.posicionesMano.Length)
        {
            GameObject cartaAleatoria = Constants.Instance.ListaPrefabsCartas[Random.Range(0, Constants.Instance.ListaPrefabsCartas.Count - 1)];
            GameObject cartaInstanciada = Instantiate(cartaAleatoria, Constants.Instance.posicionesMano[Constants.Instance.mano.Count], Quaternion.identity);
            Constants.Instance.mano.Add(cartaInstanciada);
            ManoManager.Instance.perderTurnoManoLlena();

        //     Cambiar turno después de robar
        //     UI_Manager.Instance.cambiarTurnoUI();
        //     Constants.Instance.cartaTiradaTurno = true;
        //     StartCoroutine(GameManager.Instance.tirarCartaIA());
         }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano está llena.");
        }
    }


        public void DrawCard()
    {
        if (Constants.Instance.ListaPrefabsCartas.Count > 0 && Constants.Instance.mano.Count < Constants.Instance.posicionesMano.Length)
        {
            GameObject cartaAleatoria = Constants.Instance.ListaPrefabsCartas[Random.Range(0, Constants.Instance.ListaPrefabsCartas.Count - 1)];
            GameObject cartaInstanciada = Instantiate(cartaAleatoria, Constants.Instance.posicionesMano[Constants.Instance.mano.Count], Quaternion.identity);
            Constants.Instance.mano.Add(cartaInstanciada);
            ManoManager.Instance.perderTurnoManoLlena();

            // Cambiar turno después de robar
            UI_Manager.Instance.cambiarTurnoUI();
            Constants.Instance.cartaTiradaTurno = true;
            StartCoroutine(GameManager.Instance.tirarCartaIA());
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano está llena.");
        }
    }

    public void SpawnCardEnemiga()
    {  
        if (Constants.Instance.ListaPrefabsCartas.Count > 0 && Constants.Instance.manoEnemiga.Count < Constants.Instance.posicionesManoEnemiga.Length)
        {
            GameObject cartaAleatoria = Constants.Instance.ListaPrefabsCartas[Random.Range(0, Constants.Instance.ListaPrefabsCartas.Count - 1)];

            GameObject cartaInstanciada = Instantiate(cartaAleatoria, Constants.Instance.posicionesManoEnemiga[Constants.Instance.manoEnemiga.Count], Quaternion.identity);

            GameObject cartaReverso = Constants.Instance.ListaPrefabsCartas[Constants.Instance.ListaPrefabsCartas.Count - 1];
            Sprite reversoSprite = cartaReverso.GetComponent<SpriteRenderer>().sprite;

            SpriteRenderer spriteRenderer = cartaInstanciada.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = reversoSprite;

            Constants.Instance.manoEnemiga.Add(cartaInstanciada);
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano enemiga est� llena.");
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




}
