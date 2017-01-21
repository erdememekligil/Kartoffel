using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeteroidController : MonoBehaviour {
	public int id = 0;

	public void SetId(int i){
		id = i;
		Debug.Log ("Set id of meteor " + i);
		transform.GetChild(0).GetComponent<TextMesh> ().text = id.ToString ();
	}

	public void SetPosition(Vector3 newPos, float deltaTime){
		//transform.position = newPos;
		transform.DOKill();
		transform.DOMove (newPos, deltaTime).SetEase (Ease.Linear);
	}
}
