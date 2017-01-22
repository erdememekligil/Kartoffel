using UnityEngine;
using System.Collections;

public class ParticleSystemDisposer : MonoBehaviour {

	void Update () {
        if (!gameObject.GetComponent<ParticleSystem>().IsAlive()) Destroy(gameObject);
	}
}
