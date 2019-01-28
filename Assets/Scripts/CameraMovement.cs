using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public float speed = 2f;
    private Vector2 mousepos;
    private Vector2 origin, destination;

	// Use this for initialization
	void Start () {
        Camera.main.orthographicSize = 6f;
	}
	
	// Update is called once per frame
	void Update () {

        mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //print(mousepos);

        //Camera pan
        if (Input.GetMouseButton(1)) {
            transform.Translate(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && Camera.main.orthographicSize < 30f) {
            //scroll in
            Camera.main.orthographicSize += 1f;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f && Camera.main.orthographicSize > 1f) {
            //scroll out
            Camera.main.orthographicSize -= 1f;
        }

    }
}
