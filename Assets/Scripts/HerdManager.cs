using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HerdManager : MonoBehaviour {

    public GameObject instantiation_animal;
    [Range(1, 100)]
    public int animal_instantiation_count;

    private float total_x = 0;
    private float total_y = 0;
    private GameObject[] animals;
    private int animal_starting_amount = 0;
    private GameObject overlay;
    private Canvas canvas;
    private Text text;
    private GameObject startcrate;

    // Use this for initialization
    void Start () {

        SpawnHerd();

        animals = GameObject.FindGameObjectsWithTag("Animal");
        overlay = GameObject.FindGameObjectWithTag("Overlay");
        canvas = overlay.GetComponent<Canvas>();
        text = canvas.GetComponent<Text>();

        animal_starting_amount = animals.Length;

        text.text = "Herd Remaining : %" + Mathf.Round(((float)animals.Length / (float)animal_starting_amount) * 100f);

        InvokeRepeating("GetAverage", 0f, 3f);
    }
	
	// Update is called once per frame
	void Update () {

    }

    //private void SetupCanvasOverlay() {
    //    GameObject go = new GameObject("Overlay", typeof(Canvas));

    //    Canvas canvas = go.GetComponent<Canvas>();
    //    canvas.renderMode = RenderMode.ScreenSpaceOverlay;
    //}

    private void SpawnHerd() {
        startcrate = GameObject.FindGameObjectWithTag("StartCrate");

        for(int i = 0; i < animal_instantiation_count; i++) {
            GameObject.Instantiate(instantiation_animal, startcrate.transform.position, startcrate.transform.rotation);
        }

    }

    public void RecalculateAnimalNumber() {
        overlay = GameObject.FindGameObjectWithTag("Overlay");
        canvas = overlay.GetComponent<Canvas>();
        text = canvas.GetComponent<Text>();
        CancelInvoke();
        animals = GameObject.FindGameObjectsWithTag("Animal");

        text.text = "Herd Remaining : %" + Mathf.Round(((float)animals.Length / (float)animal_starting_amount) * 100f);

        InvokeRepeating("GetAverage", 0f, 3f);
    }

    public Vector2 GetAverage() {
        total_x = 0;
        total_y = 0;

        foreach (GameObject go in animals) {
            total_x += go.transform.position.x;
            total_y += go.transform.position.y;
        }

        float average_x = total_x / animals.Length;
        float average_y = total_y / animals.Length;
        //print(average_x);
        //print(average_y);

        Vector2 avg = new Vector2(average_x, average_y);

        foreach (GameObject go in animals) {
            Debug.DrawLine(go.transform.position, avg, Color.cyan, 3f);
        }

        return avg;

    }
}
