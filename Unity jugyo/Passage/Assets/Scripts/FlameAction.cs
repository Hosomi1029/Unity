using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameAction : MonoBehaviour {

    Light MyLight;
	void Start () {
        MyLight = GetComponent<Light>();
        StartCoroutine("FireAction");
	}

    IEnumerator FireAction() {
        while (true) {
            float Rnd = Random.Range(0.01f, 0.02f);
            yield return new WaitForSeconds(Rnd);
            MyLight.intensity = Rnd * 100;
        }
    }
}
