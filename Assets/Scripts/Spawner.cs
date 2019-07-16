using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	[Range(0,100)]
	public int SpawnCount = 0;
	public GameObject InstantiationAnimal;

	//publicly-accessible method that will be used to start the spawn process
	public void SpawnHerd(){
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn() {
        for(int i = 0; i < SpawnCount; i++) {
            GameObject.Instantiate(InstantiationAnimal, gameObject.transform.position, gameObject.transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }

    }
}
