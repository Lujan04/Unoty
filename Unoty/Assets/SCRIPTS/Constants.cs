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

    public string[] NombresCartas = new string[]
{
        "Blue_0", "Blue_1", "Blue_2", "Blue_3", "Blue_4", "Blue_5", "Blue_6", "Blue_7", "Blue_8", "Blue_9",
        "Blue_Draw", "Blue_Reverse", "Blue_Skip",
        "Green_0", "Green_1", "Green_2", "Green_3", "Green_4", "Green_5", "Green_6", "Green_7", "Green_8", "Green_9",
        "Green_Draw", "Green_Reverse", "Green_Skip",
        "Red_0", "Red_1", "Red_2", "Red_3", "Red_4", "Red_5", "Red_6", "Red_7", "Red_8", "Red_9",
        "Red_Draw", "Red_Reverse", "Red_Skip",
        "Yellow_0", "Yellow_1", "Yellow_2", "Yellow_3", "Yellow_4", "Yellow_5", "Yellow_6", "Yellow_7", "Yellow_8", "Yellow_9",
        "Yellow_Draw", "Yellow_Reverse", "Yellow_Skip",
        "Plus4", "ChangeColor", "Back"
};

    public List<GameObject> ListaPrefabsCartas = new List<GameObject>();

    public List<GameObject> mano = new List<GameObject>();
    public List<GameObject> manoEnemiga = new List<GameObject>();
    public List<GameObject> Game = new List<GameObject>();

    public bool cartaTiradaTurno = false;

    public int indexCarta = 0;
    public int layerCarta = 1;

    void Awake() { Instance = this; }
}
