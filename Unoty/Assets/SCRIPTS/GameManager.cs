using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void checkWinner()
    {

        if (Constants.Instance.mano.Count == 0)
        {
            Constants.Instance.Game.Clear();
            foreach (var card in Constants.Instance.Game)
            {
                Destroy(card);
            }
            UI_Manager.Instance.VictoriaJugador();
        }
        else if (Constants.Instance.manoEnemiga.Count == 0)
        {
            Constants.Instance.Game.Clear();
            foreach (var card in Constants.Instance.Game)
            {
                Destroy(card);
            }
            UI_Manager.Instance.VictoriaIA();
        }
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

            TirarCarta();
            return;
        }

        // Comprobar si la carta se puede jugar por coincidencia de color o número/tipo
        if (colorSeleccionado == colorUltima || tipoSeleccionado == tipoUltima)
        {

            TirarCarta();
        }
        else
        {
            //UI_Manager.Instance.warningTirarCarta();
            StartCoroutine(UI_Manager.Instance.warningTirarCarta(1.0f));  // You can adjust the duration here

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

            if (Constants.Instance.Game.Count > 15)
            {
                Destroy(Constants.Instance.Game[0]);
                Constants.Instance.Game.RemoveAt(0);
            }

            Destroy(Constants.Instance.mano[Constants.Instance.indexCarta]);
            Constants.Instance.mano.RemoveAt(Constants.Instance.indexCarta);
            Constants.Instance.indexCarta = (Constants.Instance.indexCarta - 1 + Constants.Instance.mano.Count) % Constants.Instance.mano.Count;
            ManoManager.Instance.ReorganizeCards();
            StartCoroutine(tirarCartaIA());

        }

    }
    public IEnumerator tirarCartaIA()
    {
        Constants.Instance.cartaTiradaTurno = true;
        yield return new WaitForSeconds(1f);


        bool cartaJugada = false;
        string colorUltimaCarta = "";

        if (Constants.Instance.Game.Count > 0)
        {
            GameObject ultimaCartaJugada = Constants.Instance.Game[Constants.Instance.Game.Count - 1];
            string nombreUltimaCarta = ultimaCartaJugada.name.Replace("(Clone)", "").Trim();
            string[] partesUltima = nombreUltimaCarta.Split('_');
            colorUltimaCarta = partesUltima[0];
        }

        foreach (GameObject carta in Constants.Instance.manoEnemiga)
        {
            string nombreCarta = carta.name.Replace("(Clone)", "").Trim();
            if (nombreCarta == "Plus4" || nombreCarta == "ChangeColor")
            {
                TirarCartaEnemiga(carta);
                ManoManager.Instance.CambiarTurno();
                cartaJugada = true;
                break;
            }
        }


        if (!cartaJugada)
        {
            foreach (GameObject carta in Constants.Instance.manoEnemiga)
            {
                string nombreCarta = carta.name.Replace("(Clone)", "").Trim();
                string[] partesCarta = nombreCarta.Split('_');
                string tipoCarta = partesCarta.Length > 1 ? partesCarta[1] : "";

                if (Constants.Instance.Game.Count > 0)
                {
                    GameObject ultimaCartaJugada = Constants.Instance.Game[Constants.Instance.Game.Count - 1];
                    string nombreUltimaCarta = ultimaCartaJugada.name.Replace("(Clone)", "").Trim();
                    string[] partesUltima = nombreUltimaCarta.Split('_');
                    string tipoUltima = partesUltima.Length > 1 ? partesUltima[1] : "";

                    if (tipoCarta == tipoUltima)
                    {
                        TirarCartaEnemiga(carta);
                        ManoManager.Instance.CambiarTurno();
                        cartaJugada = true;
                        break;
                    }
                }
            }
        }


        if (!cartaJugada)
        {
            foreach (GameObject carta in Constants.Instance.manoEnemiga)
            {
                string nombreCarta = carta.name.Replace("(Clone)", "").Trim();
                string[] partesCarta = nombreCarta.Split('_');
                string colorCarta = partesCarta[0];

                if (colorCarta == colorUltimaCarta)
                {

                    TirarCartaEnemiga(carta);
                    ManoManager.Instance.CambiarTurno();
                    cartaJugada = true;
                    break;
                }
            }
        }



        if (!cartaJugada)
        {

            Debug.Log("roba card nueva");
            CardSpawner.Instance.SpawnCardEnemiga();
            ManoManager.Instance.CambiarTurno();
        }
    }

    public void TirarCartaEnemiga(GameObject carta)
    {
        string nombreCarta = carta.name.Replace("(Clone)", "").Trim();


        GameObject cartaReal = Constants.Instance.ListaPrefabsCartas.Find(c => c.name == nombreCarta);

        if (cartaReal != null)
        {

            Sprite spriteCarta = cartaReal.GetComponent<SpriteRenderer>().sprite;

            Vector3 posicionJuego = new Vector3(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                1
            );

            GameObject cartaTirada = Instantiate(carta, posicionJuego, Quaternion.Euler(0f, 0f, Random.Range(-10f, 20f)));

            cartaTirada.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

            cartaTirada.GetComponent<SpriteRenderer>().sprite = spriteCarta;
            cartaTirada.GetComponent<SpriteRenderer>().sortingOrder = Constants.Instance.layerCarta++;
            Constants.Instance.Game.Add(cartaTirada);


            Constants.Instance.manoEnemiga.Remove(carta);
            Destroy(carta);

            ManoManager.Instance.ReorganizeCardsEnemiga();

        }
        else
        {
            Debug.LogError($"No se encontró la carta correspondiente a {nombreCarta}");
        }
    }
}
