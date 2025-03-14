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
                Debug.LogError($"No se encontró el prefab '{cardName}'");
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
            ManoManager.Instance.pasarTurnoManoLlena();
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
            // Seleccionar una carta aleatoria de la lista, excluyendo la carta de reverso (la última posición de la lista)
            GameObject cartaAleatoria = Constants.Instance.ListaPrefabsCartas[Random.Range(0, Constants.Instance.ListaPrefabsCartas.Count - 1)];

            // Instanciamos la carta aleatoria en la mano enemiga con el sprite del reverso
            GameObject cartaInstanciada = Instantiate(cartaAleatoria, Constants.Instance.posicionesManoEnemiga[Constants.Instance.manoEnemiga.Count], Quaternion.identity);

            // Obtener el sprite del reverso, que está en la última posición de la lista
            GameObject cartaReverso = Constants.Instance.ListaPrefabsCartas[Constants.Instance.ListaPrefabsCartas.Count - 1];
            Sprite reversoSprite = cartaReverso.GetComponent<SpriteRenderer>().sprite;

            // Cambiar el sprite de la carta instanciada a la carta de reverso
            SpriteRenderer spriteRenderer = cartaInstanciada.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = reversoSprite;

            // Añadir la carta con el reverso a la mano enemiga
            Constants.Instance.manoEnemiga.Add(cartaInstanciada);
        }
        else
        {
            Debug.LogWarning("No hay cartas cargadas en la lista o la mano enemiga está llena.");
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
