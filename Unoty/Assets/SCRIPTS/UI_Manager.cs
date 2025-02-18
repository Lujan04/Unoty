using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance { get; private set; }

    [SerializeField] private GameObject textoPrefabWarningCarta;
    [SerializeField] private GameObject textoPrefabWarningTurno;

    [SerializeField] private Canvas canvas;

    void Awake() { Instance = this; }
    public void warningTirarCarta(float duracion = 1.0f)
    {

        GameObject texto = Instantiate(textoPrefabWarningCarta, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-110, -90);
        Destroy(texto, duracion);
    }
    public void warningTurnoIA(float duracion = 1.0f)
    {

        GameObject texto = Instantiate(textoPrefabWarningTurno, canvas.transform);
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(-110, -90);
        Destroy(texto, duracion);
    }
}
