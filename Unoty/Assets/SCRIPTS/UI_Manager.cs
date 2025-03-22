using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    [SerializeField] public GameObject textoPrefabWarningCarta;
    [SerializeField] private GameObject textoPrefabWarningTurno;
    [SerializeField] private GameObject textoPrefabLimiteCartas;
    [SerializeField] private GameObject TextoVictoriaIA;
    [SerializeField] private GameObject TextoVictoriaJugador;
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<GameObject> DescarteMenu = new List<GameObject>();
    





    [SerializeField] private GameObject TurnoIA;
    [SerializeField] private GameObject TurnoJugador;
    public bool isTurnoIA = false;
    public bool isTurnoJugador = true;

    private static bool TextoExistente = false;

    void Awake() { Instance = this; }

public IEnumerator warningTirarCarta(float duracion = 1.0f)
{
    Debug.Log("warningTirarCarta - Inicio");
    if (TextoExistente)
    {
        Debug.Log("Warning ya existe");
        yield break;
    }

    TextoExistente = true;
    
    // Instanciar el texto primero
    GameObject texto = Instantiate(textoPrefabWarningCarta, canvas.transform);
    RectTransform rt = texto.GetComponent<RectTransform>();
    rt.anchoredPosition = new Vector2(-110, -90);
    
    // Activar el texto
    texto.SetActive(true);
    Debug.Log("Warning activado");
    
    // Esperar el tiempo especificado
    yield return new WaitForSeconds(2.0f);
    
    Debug.Log("Desactivando warning");
    // Desactivar en lugar de destruir
    if (texto != null)
    {
        texto.SetActive(false);
        Destroy(texto);
    }
    
    yield return new WaitForSeconds(0.5f);
    TextoExistente = false;
    Debug.Log("warningTirarCarta - Fin");
}

    public IEnumerator warningTurnoIA(float duracion = 1.0f)
    {
        if (TextoExistente == true) yield break;

        TextoExistente = true;
        GameObject texto = Instantiate(textoPrefabWarningTurno, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-110, -90);
        Destroy(texto, duracion);

        // Wait for 3 seconds before setting TextoExistente to false
        yield return new WaitForSeconds(2.0f);

        TextoExistente = false;
    }

    // public IEnumerator warningLimiteCartas(float duracion = 1.0f)
    // {
    //     if (TextoExistente == true) yield break;

    //     TextoExistente = true;
    //     GameObject texto = Instantiate(textoPrefabLimiteCartas, canvas.transform);
    //     RectTransform rt = texto.GetComponent<RectTransform>();
    //     rt.anchoredPosition = new Vector2(300, -90);
    //     Destroy(texto, duracion);

    //     // Wait for 3 seconds before setting TextoExistente to false
    //     yield return new WaitForSeconds(2.0f);

    //     TextoExistente = false;
    // }

    public IEnumerator VictoriaIA(float duracion = 1.0f)
    {
        if (TextoExistente == true) yield break;

        TextoExistente = true;
        GameObject texto = Instantiate(TextoVictoriaIA, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-150, -10);
        Destroy(texto, duracion);

        // Wait for 3 seconds before setting TextoExistente to false
        yield return new WaitForSeconds(2.0f);

        TextoExistente = false;
    }

    public IEnumerator VictoriaJugador(float duracion = 1.0f)
    {
        if (TextoExistente == true) yield break;

        TextoExistente = true;
        GameObject texto = Instantiate(TextoVictoriaJugador, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-180, -10);
        Destroy(texto, duracion);

        yield return new WaitForSeconds(2.0f);

        TextoExistente = false;
    }


    public void mostrarDescarte()
    {
        ocultatDescarte();

        Debug.Log("Mostrar Descarte");

        int startIndex = Mathf.Max(0, Constants.Instance.Game.Count - 5);
        int count = Mathf.Min(5, Constants.Instance.Game.Count);

        for (int i = startIndex; i < startIndex + count; i++)
        {
            GameObject cartaOriginal = Constants.Instance.Game[i];

            GameObject CartaDescarte = Instantiate(
                cartaOriginal,
                Constants.Instance.posicionesDescarte[i - startIndex],
                Quaternion.identity
            );

            CartaDescarte.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
            CartaDescarte.transform.rotation = Quaternion.Euler(0, 0, 0);

            SpriteRenderer renderer = CartaDescarte.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.sortingOrder = 100; 
            }

            DescarteMenu.Add(CartaDescarte);
        }
    }

    public void ocultatDescarte()
    {
        Debug.Log("Borrar Descarte");
        foreach (GameObject carta in DescarteMenu)
        {
            Destroy(carta);
        }
        DescarteMenu.Clear();
    }



    public void cambiarTurnoUI() {

        isTurnoIA = !isTurnoIA;
        isTurnoJugador = !isTurnoJugador;
        TurnoIA.SetActive(isTurnoIA);
        TurnoJugador.SetActive(isTurnoJugador);

    }
}
