using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HerdManager : MonoBehaviour {

    private float total_x = 0;
    private float total_y = 0;
    private GameObject[] animals;

    // Use this for initialization
    void Start () {
        animals = GameObject.FindGameObjectsWithTag("Animal");
        print("Number of animals to herd: " + animals.Length);

        InvokeRepeating("GetAverage", 0f, 3f);
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    Vector2 GetAverage() {
        total_x = 0;
        total_y = 0;

        foreach (GameObject go in animals) {
            total_x += go.transform.position.x;
            total_y += go.transform.position.y;
        }

        float average_x = total_x / animals.Length;
        float average_y = total_y / animals.Length;
        print(average_x);
        print(average_y);

        Vector2 avg = new Vector2(average_x, average_y);

        foreach (GameObject go in animals) {
            Debug.DrawLine(go.transform.position, avg, Color.cyan, 3f);
        }

        return avg;

    }
}
