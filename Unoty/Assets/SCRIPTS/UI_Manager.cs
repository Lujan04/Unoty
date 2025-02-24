using System.Collections;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    [SerializeField] private GameObject textoPrefabWarningCarta;
    [SerializeField] private GameObject textoPrefabWarningTurno;
    [SerializeField] private GameObject textoPrefabLimiteCartas;
    [SerializeField] private GameObject TextoVictoriaIA;
    [SerializeField] private GameObject TextoVictoriaJugador;
    [SerializeField] private Canvas canvas;

    private static bool TextoExistente = false;

    void Awake() { Instance = this; }

    public IEnumerator warningTirarCarta(float duracion = 1.0f)
    {
        if (TextoExistente == true) yield break;

        TextoExistente = true;
        GameObject texto = Instantiate(textoPrefabWarningCarta, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-110, -90);
        Destroy(texto, duracion);

        // Wait for 3 seconds before setting TextoExistente to false
        yield return new WaitForSeconds(2.0f);

        TextoExistente = false;
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

    public IEnumerator warningLimiteCartas(float duracion = 1.0f)
    {
        if (TextoExistente == true) yield break;

        TextoExistente = true;
        GameObject texto = Instantiate(textoPrefabLimiteCartas, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(300, -90);
        Destroy(texto, duracion);

        // Wait for 3 seconds before setting TextoExistente to false
        yield return new WaitForSeconds(2.0f);

        TextoExistente = false;
    }

    public IEnumerator VictoriaIA(float duracion = 1.0f)
    {
        if (TextoExistente == true) yield break;

        TextoExistente = true;
        GameObject texto = Instantiate(TextoVictoriaIA, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-150, -90);
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
        rt.anchoredPosition = new Vector2(-180, -90);
        Destroy(texto, duracion);

        // Wait for 3 seconds before setting TextoExistente to false
        yield return new WaitForSeconds(2.0f);

        TextoExistente = false;
    }
}
