using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorTransf : MonoBehaviour {

    public GameObject test1;
    public GameObject test2;
    Vector2 test1_pos, test1_neg, relative;
    private float x = 0f;
    private float y = 0f;

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
        Vector2 test1_pos = new Vector2(test1.transform.position.x + x, test1.transform.position.y + y);
        relative = test1.transform.InverseTransformVector(test1_pos);
        Ray2D ray = new Ray2D(test1.transform.position, relative);
        
        Debug.DrawLine(test1.transform.position, test1_pos, Color.cyan);
    }
}
