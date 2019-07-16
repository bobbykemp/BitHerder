using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HerdManager : MonoBehaviour {

    private GameObject[] spawners, animals;
    private int animal_starting_amount = 0;
    private GameObject overlay, startcrate;
    private Canvas canvas;
    private Text text;
    private int SpawnTotal = 0;

    void Start () {
        overlay = GameObject.FindGameObjectWithTag("Overlay");
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        canvas = overlay.GetComponent<Canvas>();
        text = canvas.GetComponent<Text>();

        //Iterate over level spawners and call their Spawn methods
        foreach (GameObject spawner in spawners){
            Spawner script = (Spawner)spawner.GetComponent(typeof(Spawner));
            script.SpawnHerd();
            SpawnTotal += script.SpawnCount;
        }

    }

    //Call this after any event that would change the number of animals in the level
    public void RecalculateAnimalNumber() {
        animals = GameObject.FindGameObjectsWithTag("Animal");
        text.text = "Herd Remaining : %" + Mathf.Round(((float)animals.Length / (float)SpawnTotal) * 100f);

    }

}
