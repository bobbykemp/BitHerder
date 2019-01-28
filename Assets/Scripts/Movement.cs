using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    private float speed;

	// Use this for initialization
	void Start () {
        speed = 2f;
	}
	
	// Update is called once per frame
	void Update () {
        speed = 1f;

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = 2f;
        }

        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
            //pointlight.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-transform.right * Time.deltaTime * speed, Space.World);
            // pointlight.transform.eulerAngles = new Vector3(0, 270

        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(-transform.forward * Time.deltaTime * speed, Space.World);
            // pointlight.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(transform.right * Time.deltaTime * speed, Space.World);
            // pointlight.transform.eulerAngles = new Vector3(0, 90, 0);
        }

        transform.Rotate(0, Input.GetAxis("MouseAxis") * 150 * Time.deltaTime, 0);
    }
}
