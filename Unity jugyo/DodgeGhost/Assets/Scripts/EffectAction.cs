using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectAction : MonoBehaviour {

    ParticleSystem[] Particles;

    void Start() {
        GetComponent<AudioSource>().Play();
    }

    void Update() {
        bool isAlive = false; 
        Particles = FindObjectsOfType<ParticleSystem>();
        foreach (ParticleSystem stored in Particles) {
            if (stored.isPlaying) {
                isAlive = true;
            }
        }
        if (!isAlive) {
            Destroy( gameObject );
        }
    }
}
