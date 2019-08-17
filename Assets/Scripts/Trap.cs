using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    private Animator anim;

	// Use this for initialization
	void Start () {
        anim = gameObject.GetComponent<Animator>();
    }

    public void Activate(GameObject victim) {
        print("TRAP ACTIVE");
        anim.Play("Landmine_Explode");
        StartCoroutine(WaitAFew(.25f, victim));
    }

    IEnumerator WaitAFew(float seconds, GameObject victim) {
        yield return new WaitForSeconds(seconds); 
        Explode(victim);
    }

    private void Explode(GameObject victim) {
        Animal animal = (Animal)victim.GetComponent(typeof(Animal));
        foreach(GameObject neighbor in animal.GetEnvironment().Where(n => n != null).ToList()){
            if(neighbor.tag == "Animal"){
                animal.RemoveFromEnvironment(gameObject);
            }
        }
        Destroy(victim);
        Destroy(this.gameObject);
    }

}
