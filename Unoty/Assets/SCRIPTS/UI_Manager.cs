using TMPro;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager Instance {get; private set;}

    void Awake() { Instance = this;}






    //Aqui  esta funcio que es la unica que has de cridar del scrit es un script que funcione per si sol
    public void warningTirarCarta(string mensaje, TMP_FontAsset fuente, int tamaño, float duracion)
    {
        GameObject texto = Instantiate(textoPrefab, canvas.transform);

        TMP_Text textComponent = texto.GetComponent<TMP_Text>();
        textComponent.text = mensaje;
        textComponent.font = fuente; 
        textComponent.fontSize = tamaño; 
        textComponent.alignment = TextAlignmentOptions.Center;
        textComponent.color = Color.red; 

        
        RectTransform rt = texto.GetComponent<RectTransform>();
        rt.anchoredPosition = Vector2.zero;

        Destroy(texto, duracion);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}






