using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PlanetController : MonoBehaviour {
	[SerializeField]
	private GameObject explosionEffect;
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
	[SerializeField]
	private float distanceThreshold = 40f;
	[SerializeField]
	private Animator[] jumperAnimators;

	void OnEnable () {
		ServerManager.Instance.OnServerFrame += OnServerFrame;
		ServerManager.Instance.OnPlanetDamaged += OnPlanetDamaged;
	}

	void OnDisable () {
		ServerManager.Instance.OnServerFrame -= OnServerFrame;
		ServerManager.Instance.OnPlanetDamaged -= OnPlanetDamaged;
	}
	
	// Update is called once per frame
	private void OnServerFrame (object sender, ServerFrame serverFrame) {
		if (useServerControls) {
			Vector3 positionToMove = new Vector3 (serverFrame.x, serverFrame.y);
			float d = Vector3.Distance (transform.position, positionToMove);
			if (d > distanceThreshold) {
				Debug.Log ("Teleporting d:" + d);
				transform.position = positionToMove;
			} else {
				transform.DOKill ();
				transform.DOMove (positionToMove, serverFrame.deltaTime).SetEase (Ease.Linear);
			}
		} else {
			Vector3 pos = GetPlanetPosition();
			transform.position = pos;
		}

		if (serverFrame.jumpers.n == 1) {
			jumperAnimators[0].SetTrigger ("Jump");
		} 
		if (serverFrame.jumpers.e == 1) {
			jumperAnimators[1].SetTrigger ("Jump");
		} 
		if (serverFrame.jumpers.s == 1) {
			jumperAnimators[2].SetTrigger ("Jump");
		} 
		if (serverFrame.jumpers.w == 1) {
			jumperAnimators[3].SetTrigger ("Jump");
		} 
	}

	private void OnPlanetDamaged (object sender, ServerFrame serverFrame, long diffHealth) {
		Debug.LogWarning ("health: " + serverFrame.health + " - diff health: " + diffHealth);
		GameObject exp = GameObject.Instantiate (explosionEffect, Vector3.zero, Quaternion.Euler (new Vector3 (0, 0, UnityEngine.Random.Range(0,360))));
		exp.transform.SetParent(transform);
		exp.transform.localPosition = Vector3.zero;
		Camera.main.DOKill ();
		Camera.main.transform.localPosition = Vector3.zero;
		Camera.main.DOShakePosition (1, 3, 10, 90);
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
