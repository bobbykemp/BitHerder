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
        anim.Play("Landmine_Explode");
        StartCoroutine(WaitAFew(6f, victim));
    }

    IEnumerator WaitAFew(float seconds, GameObject victim) {
        yield return new WaitForSeconds(seconds); 
        Explode(victim);
    }

    private void Explode(GameObject victim) {
        Destroy(victim);
        Destroy(this.gameObject);
    }

}
