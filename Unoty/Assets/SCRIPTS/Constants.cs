

using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Constants Instance { get; private set; }

    public Vector3[] posicionesMano = new Vector3[]
{
    new Vector3(-6.34f, -3.33f, 3),
    new Vector3(-4.94f, -3.33f, 3),
    new Vector3(-3.54f, -3.33f, 3),
    new Vector3(-2.14f, -3.33f, 3),
    new Vector3(-0.74f, -3.33f, 3),
    new Vector3(0.66f, -3.33f, 3),
    new Vector3(2.06f, -3.33f, 3),
    new Vector3(3.46f, -3.33f, 3),
    new Vector3(4.86f, -3.33f, 3),
    new Vector3(6.26f, -3.33f, 3)
};

    public Vector3[] posicionesManoEnemiga = new Vector3[]
    {
    new Vector3(-6.34f, 3.2f, 3),
    new Vector3(-4.94f, 3.2f, 3),
    new Vector3(-3.54f, 3.2f, 3),
    new Vector3(-2.14f, 3.2f, 3),
    new Vector3(-0.74f, 3.2f, 3),
    new Vector3(0.66f, 3.2f, 3),
    new Vector3(2.06f, 3.2f, 3),
    new Vector3(3.46f, 3.2f, 3),
    new Vector3(4.86f, 3.2f, 3),
    new Vector3(6.26f, 3.2f, 3)
    };

        public Vector3[] posicionesDescarte = new Vector3[] {
            new Vector3(-7.52f, -0.84f, 0),
            new Vector3(-6.52f, -0.84f, 0),
            new Vector3(-5.52f, -0.84f, 0),
            new Vector3(-4.52f, -0.84f, 0),
            new Vector3(-3.52f, -0.84f, 0),
        };



    public string[] NombresCartas = new string[] {
    "blue_0", "blue_1", "blue_2", "blue_3", "blue_4", "blue_5", "blue_6", "blue_7", "blue_8", "blue_9",
    "blue_draw", "blue_reverse", "blue_skip",
    "green_0", "green_1", "green_2", "green_3", "green_4", "green_5", "green_6", "green_7", "green_8", "green_9",
    "green_draw", "green_reverse", "green_skip",
    "red_0", "red_1", "red_2", "red_3", "red_4", "red_5", "red_6", "red_7", "red_8", "red_9",
    "red_draw", "red_reverse", "red_skip",
    "yellow_0", "yellow_1", "yellow_2", "yellow_3", "yellow_4", "yellow_5", "yellow_6", "yellow_7", "yellow_8", "yellow_9",
            "yellow_draw", "yellow_reverse", "yellow_skip",
            "plus4", "changecolor", "back"
    };


    public List<GameObject> ListaPrefabsCartas = new List<GameObject>();

    public List<GameObject> mano = new List<GameObject>();
    public List<GameObject> manoEnemiga = new List<GameObject>();
    public List<GameObject> Game = new List<GameObject>();
    public List<GameObject> CartasTiradas = new List<GameObject>();


    public bool cartaTiradaTurno = false;

    public int indexCarta = 0;
    public int layerCarta = 1;

    void Awake() { Instance = this; }
}
