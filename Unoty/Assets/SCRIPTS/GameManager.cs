using System.Collections;
using System.Linq;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Menu;
    public GameObject winIA;
    public GameObject winJugador;
    public bool MenuEnabled = false;
    public bool isWiner = false;
    private bool isCheckingWinner = false;
    public bool skipTurn = false;
    public bool showDescarte = false;
    public bool isReverse = false;  // Variable para controlar la dirección de los turnos

    private void Awake()
    {
        Instance = this;
    }

    public IEnumerator checkWinner()
    {
        if (isCheckingWinner || isWiner) yield break;
        isCheckingWinner = true;

        if (Constants.Instance.mano.Count == 0)
        {

         
            Debug.Log("win 1");
            foreach (var card in Constants.Instance.Game)
            {
                Destroy(card);
            }
            Constants.Instance.Game.Clear();

            winJugador.SetActive(true);
            yield return new WaitForSeconds(1f);
            winJugador.SetActive(false);

            isWiner = true;
            yield return new WaitForSeconds(0.5f);

            Menu.SetActive(true);
            Time.timeScale = 0;
        }
        else if (Constants.Instance.manoEnemiga.Count == 0)
        {
            
            Debug.Log("win 2");
            foreach (var card in Constants.Instance.Game)
            {
                Destroy(card);
            }

            Constants.Instance.Game.Clear();

            winIA.SetActive(true);
            yield return new WaitForSeconds(1f);
            winIA.SetActive(false);
            isWiner = true;
            yield return new WaitForSeconds(0.5f);

            Menu.SetActive(true);
            Time.timeScale = 0;
        }
        isCheckingWinner = false;
    }

    public void ComprobarCarta()
    {
        if (Constants.Instance.mano.Count == 0) return;

        GameObject cartaSeleccionada = Constants.Instance.mano[Constants.Instance.indexCarta];
        string nombreCartaSeleccionada = cartaSeleccionada.name.Replace("(Clone)", "").Trim();

        if (Constants.Instance.Game.Count == 0)
        {
            TirarCarta();
            return;
        }

        GameObject ultimaCartaJugada = Constants.Instance.Game[Constants.Instance.Game.Count - 1];
        string nombreUltimaCarta = ultimaCartaJugada.name.Replace("(Clone)", "").Trim();

        // Extraer color y número/tipo de ambas cartas
        string[] partesSeleccionada = nombreCartaSeleccionada.Split('_');
        string[] partesUltima = nombreUltimaCarta.Split('_');

        string colorSeleccionado = partesSeleccionada[0];
        string tipoSeleccionado = partesSeleccionada.Length > 1 ? partesSeleccionada[1] : "";

        string colorUltima = partesUltima[0];
        string tipoUltima = partesUltima.Length > 1 ? partesUltima[1] : "";

        if (nombreCartaSeleccionada == "Plus4" || nombreCartaSeleccionada == "ChangeColor" || nombreUltimaCarta == "Plus4" || nombreUltimaCarta == "ChangeColor")
        {
            Debug.Log("Carta especial tirada");
            TirarCarta();
            return;
        }

        if (nombreCartaSeleccionada == "ChangeColor")
        {
            TirarCarta();
        }

        // Comprobar si la
        if (colorSeleccionado == colorUltima || tipoSeleccionado == tipoUltima)
        {
            TirarCarta();
        }
        else
        {
            //UI_Manager.Instance.warningTirarCarta();
            StartCoroutine(UI_Manager.Instance.warningTirarCarta(1.0f));
        }
    }

    public void TirarCarta()
    {
        if (Constants.Instance.cartaTiradaTurno == false)
        {
            Vector3 posicionJuego = new Vector3(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                1
            );

            GameObject cartaTirada = Instantiate(Constants.Instance.mano[Constants.Instance.indexCarta], posicionJuego, Quaternion.Euler(0f, 0f, Random.Range(-10f, 20f)));
            cartaTirada.GetComponent<SpriteRenderer>().sortingOrder = Constants.Instance.layerCarta++;
            Constants.Instance.Game.Add(cartaTirada);

            if (Constants.Instance.Game.Count > 10)
            {
                Destroy(Constants.Instance.Game[0]);
                Constants.Instance.Game.RemoveAt(0);
            }

            string nombreCarta = Constants.Instance.mano[Constants.Instance.indexCarta].name.Replace("(Clone)", "").Trim();
            bool turnoExtra = false;

            if (nombreCarta == "Plus4")
            {
                for (int i = 0; i < 4; i++)
                {
                    CardSpawner.Instance.SpawnCardEnemiga();
                }
            }
            else if (nombreCarta.EndsWith("Reverse"))
            {
                isReverse = !isReverse;  // Cambiar la dirección de los turnos
                turnoExtra = true; // Permite que la IA o el jugador vuelva a jugar
            }
            else if (nombreCarta.EndsWith("Skip"))
            {
                skipTurn = true; // El turno se salta
                turnoExtra = true; // Permite que la IA o el jugador vuelva a jugar
            }
            else if (nombreCarta.EndsWith("_Draw"))
            {
                for (int i = 0; i < 2; i++)
                {
                    CardSpawner.Instance.SpawnCardEnemiga();
                }
            }

            Destroy(Constants.Instance.mano[Constants.Instance.indexCarta]);
            Constants.Instance.mano.RemoveAt(Constants.Instance.indexCarta);

            if (Constants.Instance.mano.Count != 0)
            {
                Constants.Instance.indexCarta = (Constants.Instance.indexCarta - 1 + Constants.Instance.mano.Count) % Constants.Instance.mano.Count;
            }

            // Si no es un turno extra, se cambia de turno
            if (!turnoExtra)
            {
                UI_Manager.Instance.cambiarTurnoUI();
                ManoManager.Instance.ReorganizeCards();
                StartCoroutine(tirarCartaIA());
            }
            else
            {
                Constants.Instance.cartaTiradaTurno = false; // Permite volver a jugar si es una carta especial
            }
        }
    }


    public IEnumerator tirarCartaIA()
    {
        Constants.Instance.cartaTiradaTurno = true;
        yield return new WaitForSeconds(0.5f);

        bool cartaJugada = false;
        bool turnoExtra = false;
        string colorUltimaCarta = "";
        string tipoUltimaCarta = "";

        // Obtener la última carta jugada en la mesa
        if (Constants.Instance.Game.Count > 0)
        {
            GameObject ultimaCartaJugada = Constants.Instance.Game[Constants.Instance.Game.Count - 1];
            string nombreUltimaCarta = ultimaCartaJugada.name.Replace("(Clone)", "").Trim();
            string[] partesUltima = nombreUltimaCarta.Split('_');
            colorUltimaCarta = partesUltima[0];
            tipoUltimaCarta = partesUltima.Length > 1 ? partesUltima[1] : "";
        }

        // Primero revisamos las cartas especiales
        foreach (GameObject carta in Constants.Instance.manoEnemiga.ToList())
        {
            string nombreCarta = carta.name.Replace("(Clone)", "").Trim();
            string[] partesCarta = nombreCarta.Split('_');
            string colorCarta = partesCarta[0];
            string tipoCarta = partesCarta.Length > 1 ? partesCarta[1] : "";

            // Jugar cartas especiales (Plus4, ChangeColor, Reverse, Skip, Draw)
            if (nombreCarta == "Plus4" || nombreCarta == "ChangeColor")
            {
                string nuevoColor = ElegirColorIA(); // Seleccionar color automáticamente
                TirarCartaEnemiga(carta, nuevoColor);
                cartaJugada = true;
                if (nombreCarta == "Plus4")
                {
                    for (int i = 0; i < 4; i++) CardSpawner.Instance.SpawnCard();
                }
                break;
            }
            else if (tipoCarta == "Reverse")
            {
                isReverse = !isReverse; // Cambiar la dirección del turno
                TirarCartaEnemiga(carta);
                cartaJugada = true;
                turnoExtra = true; // Permitir otro turno
                break;
            }
            else if (tipoCarta == "Skip")
            {
                skipTurn = true; // Saltar el turno del jugador
                TirarCartaEnemiga(carta);
                cartaJugada = true;
                turnoExtra = true; // Permitir otro turno
                break;
            }
            else if (tipoCarta == "Draw")
            {
                for (int i = 0; i < 2; i++) CardSpawner.Instance.SpawnCard();
                TirarCartaEnemiga(carta);
                cartaJugada = true;
                break;
            }
        }

        // Si no se jugó ninguna carta especial, buscar cartas normales compatibles
        if (!cartaJugada)
        {
            foreach (GameObject carta in Constants.Instance.manoEnemiga.ToList())
            {
                string nombreCarta = carta.name.Replace("(Clone)", "").Trim();
                string[] partesCarta = nombreCarta.Split('_');
                string colorCarta = partesCarta[0];
                string tipoCarta = partesCarta.Length > 1 ? partesCarta[1] : "";

                // Jugar cartas normales que coincidan en color o tipo
                if (colorCarta == colorUltimaCarta || tipoCarta == tipoUltimaCarta)
                {
                    TirarCartaEnemiga(carta);
                    cartaJugada = true;

                    // Verificar si es una carta especial (Reverse o Skip)
                    if (tipoCarta == "Reverse")
                    {
                        isReverse = !isReverse;
                        turnoExtra = true; // Permitir otro turno
                    }
                    else if (tipoCarta == "Skip")
                    {
                        skipTurn = true;
                        turnoExtra = true; // Permitir otro turno
                    }
                    break;
                }
            }
        }


        if (!cartaJugada)
        {
            Debug.Log("La IA no puede jugar. Robando y pasando turno...");
            CardSpawner.Instance.SpawnCardEnemiga();
            yield return new WaitForSeconds(0.5f);

            // Forzar cambio de turno
            Constants.Instance.cartaTiradaTurno = true;
            ManoManager.Instance.CambiarTurno();
            UI_Manager.Instance.cambiarTurnoUI();
        }
        else if (turnoExtra)
        {
            yield return new WaitForSeconds(1f); // Esperar antes del turno extra
            Constants.Instance.cartaTiradaTurno = false;
            StartCoroutine(tirarCartaIA());
        }
        else
        {
            // Asegurar reset de estados incluso si hay errores
            GameManager.Instance.skipTurn = false;
            ManoManager.Instance.CambiarTurno();
            UI_Manager.Instance.cambiarTurnoUI();
        }
    }

    private string ElegirColorIA()
    {
        var colores = Constants.Instance.manoEnemiga
            .Select(c => c.name.Split('_')[0])
            .Where(c => c != "ChangeColor" && c != "Plus4")
            .GroupBy(c => c)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .FirstOrDefault();

        return colores ?? new[] { "Red", "Blue", "Green", "Yellow" }[Random.Range(0, 4)];
    }


    public void TirarCartaEnemiga(GameObject carta, string nuevoColor = null)
    {
        // Obtener el nombre de la carta sin el sufijo "(Clone)"
        string nombreCarta = carta.name.Replace("(Clone)", "").Trim();

        // Si se proporciona un nuevo color (para cartas especiales como ChangeColor o Plus4), actualizar el nombre de la carta
        if (!string.IsNullOrEmpty(nuevoColor))
        {
            if (nombreCarta == "ChangeColor" || nombreCarta == "Plus4")
            {
                nombreCarta = $"{nuevoColor}_{nombreCarta}";
            }
        }

        // Buscar la carta correspondiente en la lista de prefabs
        GameObject cartaReal = Constants.Instance.ListaPrefabsCartas.Find(c => c.name == nombreCarta);

        if (cartaReal != null)
        {
            // Obtener el sprite de la carta
            Sprite spriteCarta = cartaReal.GetComponent<SpriteRenderer>().sprite;

            // Definir una posición aleatoria para la carta en la mesa
            Vector3 posicionJuego = new Vector3(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                1
            );

            // Instanciar la carta en la mesa
            GameObject cartaTirada = Instantiate(carta, posicionJuego, Quaternion.Euler(0f, 0f, Random.Range(-10f, 20f)));

            // Ajustar la escala de la carta
            cartaTirada.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            // Asignar el sprite correcto a la carta
            cartaTirada.GetComponent<SpriteRenderer>().sprite = spriteCarta;

            // Ajustar el orden de renderizado para que la carta se vea por encima de las demás
            cartaTirada.GetComponent<SpriteRenderer>().sortingOrder = Constants.Instance.layerCarta++;

            // Añadir la carta al montón de juego
            Constants.Instance.Game.Add(cartaTirada);

            // Eliminar la carta de la mano de la IA
            Constants.Instance.manoEnemiga.Remove(carta);

            // Destruir la carta original
            Destroy(carta);

            // Reorganizar las cartas en la mano de la IA
            ManoManager.Instance.ReorganizeCardsEnemiga();

            // Verificar si la carta tirada es una carta especial
            if (nombreCarta.EndsWith("Reverse"))
            {
                isReverse = !isReverse; // Cambiar la dirección del turno
            }
            else if (nombreCarta.EndsWith("Skip"))
            {
                skipTurn = true; // Saltar el turno del jugador
            }
            else if (nombreCarta.EndsWith("Draw"))
            {
                // Hacer que el jugador robe 2 cartas
                //for (int i = 0; i < 2; i++)
                //{
                //    CardSpawner.Instance.SpawnCard();
                //}
            }
            else if (nombreCarta == "Plus4")
            {
                // Hacer que el jugador robe 4 cartas
                for (int i = 0; i < 4; i++)
                {
                    CardSpawner.Instance.SpawnCard();
                }
            }
        }
        else
        {
            Debug.LogError($"No se encontró la carta correspondiente a {nombreCarta}");
        }
    }

    public void showMenuEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuEnabled = !MenuEnabled;
            Menu.SetActive(MenuEnabled);

            if (MenuEnabled)
            {
                Time.timeScale = 0;
                UI_Manager.Instance.mostrarDescarte(); // Añadir esta línea
            }
            else
            {
                Time.timeScale = 1;
                UI_Manager.Instance.ocultatDescarte(); // Añadir esta línea
            }
        }
    }

    public void showMenuButton()
    {
        Debug.Log("Show menu");
        MenuEnabled = !MenuEnabled;
        Menu.SetActive(MenuEnabled);

        if (MenuEnabled)
        {
            UI_Manager.Instance.mostrarDescarte();
        }
        else
        {
            UI_Manager.Instance.ocultatDescarte();
        }
    }   

    public void ResetGame()
    {

        //foreach (GameObject carta in Constants.Instance.mano)
        //{
        //    Destroy(carta);
        //}
        //Constants.Instance.mano.Clear();


        //foreach (GameObject carta in Constants.Instance.manoEnemiga)
        //{
        //    Destroy(carta);
        //}
        //Constants.Instance.manoEnemiga.Clear();


        //foreach (GameObject carta in Constants.Instance.Game)
        //{
        //    Destroy(carta);
        //}
        //Constants.Instance.Game.Clear();

        //Constants.Instance.cartaTiradaTurno = false;
        //Constants.Instance.indexCarta = 0;
        //Constants.Instance.layerCarta = 1;


        //   winIA.SetActive(false);
        //   winJugador.SetActive(false);


        //yield return new WaitForSeconds(1f);

        //CardSpawner.Instance.InitManos();

        SceneManager.LoadScene("Game");

    }


    void Update()
    {
        
        showMenuEscape();
        if (isWiner == false && !skipTurn) 
        {
            StartCoroutine(checkWinner());
        }
    }   
}
