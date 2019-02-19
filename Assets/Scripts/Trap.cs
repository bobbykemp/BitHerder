using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
    }

    public void Activate(GameObject victim) {
        print("Animal collision from landmine");
        anim.Play("Landmine_Explode");
        //StartCoroutine(WaitAFew(5f));
        
    }

    //IEnumerator WaitAFew(float seconds) {
    //    //yield return new Waut
    //}

    private void Explode(GameObject victim) {
        Destroy(victim);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
