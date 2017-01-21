using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public class PlanetController : MonoBehaviour {
	public int RoomId = 1;
	public string RegionType = "w";
	public float CalculatedForce = 1;
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
	private string requestEndPoint = "http://project.kinix.org/ggj2017/temp/index.php";
	[SerializeField]
	private string operationPath = "?room=1&region=w&force=1";

	// Use this for initialization
	void Start () {
		Coroutine gameLoop = StartCoroutine(GameLoop());
	}
	
	// Update is called once per frame
	IEnumerator GameLoop () {
		while (true) {
			if (useServerControls) {
				WWW requestObj = new WWW (requestEndPoint + operationPath); //String.Format (operationPath, RoomId, RegionType, CalculatedForce));
				yield return requestObj;
				ServerFrame resultObj = JsonUtility.FromJson<ServerFrame> (requestObj.text);
				transform.DOMove(new Vector3 (resultObj.x, resultObj.y),0.2f).SetEase(Ease.InOutElastic);
			} else {
				transform.position = GetPlanetPosition ();
			}
			yield return new WaitForSeconds (0.5f);
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
