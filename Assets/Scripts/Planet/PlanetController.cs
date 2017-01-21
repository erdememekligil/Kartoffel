using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PlanetController : MonoBehaviour {
	[SerializeField]
	private bool useServerControls = false;
	[SerializeField]
	private float speedMultiplier = 0.5f;
	[SerializeField]
	private float frictionMultipler = 0.2f;
	[SerializeField]
	private Vector3 speedVector = Vector3.zero;
	[SerializeField]
	private Vector3 minSpeed = Vector3.one * -5;
	[SerializeField]
	private Vector3 maxSpeed = Vector3.one * 5;

	void OnEnable () {
		ServerManager.Instance.OnServerFrame += OnServerFrame;
	}

	void OnDisable () {
		ServerManager.Instance.OnServerFrame -= OnServerFrame;
	}
	
	// Update is called once per frame
	private void OnServerFrame (object sender, ServerFrame serverFrame) {
		if (useServerControls) {
			transform.DOKill();
			transform.DOMove(new Vector3 (serverFrame.x, serverFrame.y),serverFrame.deltaTime).SetEase(Ease.Linear);
		}
	}


	public Vector3 GetPlanetPosition(){
		speedVector = LimitSpeed(speedVector - speedVector * frictionMultipler, minSpeed, maxSpeed);
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			speedVector += Vector3.down * speedMultiplier;
		} else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			speedVector += Vector3.left * speedMultiplier;
		} else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			speedVector += Vector3.right * speedMultiplier;
		} else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			speedVector += Vector3.up * speedMultiplier;
		}

		//move the planet
		return new Vector3 (transform.position.x + speedVector.x,
			transform.position.y + speedVector.y,
			transform.position.z + speedVector.z);
	}
	 
	public Vector3 LimitSpeed(Vector3 speedVector, Vector3 minValue, Vector3 maxValue){
		float x = Mathf.Clamp (speedVector.x, minValue.x, maxValue.x);
		float y = Mathf.Clamp (speedVector.y, minValue.y, maxValue.y);
		float z = Mathf.Clamp (speedVector.z, minValue.z, maxValue.z);
		return new Vector3(x,y,z);
	}

}
