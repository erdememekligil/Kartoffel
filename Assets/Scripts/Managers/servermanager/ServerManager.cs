using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

public class ServerManager : SingletonComponent<ServerManager> {
	public int RoomId = 1;
	public string RegionType = "w";
	public float CalculatedForce = 0;
	public ServerFrame ServerFrame;

	public bool IsReady { get; private set;}

	public delegate void ServerFrameEvent(object sender, ServerFrame sf);
	public event ServerFrameEvent OnServerFrame;

	public delegate void PlanetDamaged(object sender, ServerFrame sf, long diffHealth);
	public event PlanetDamaged OnPlanetDamaged;

	[SerializeField]
	private string requestEndPoint = "http://project.kinix.org/ggj2017/index.php";
	[SerializeField]
	private string operationPath = "?room={0}&region={1}&force={2}";

	// Use this for initialization
	void Start () {
		Coroutine serverRefresh = StartCoroutine(ServerRefresh());
	}

	// Update is called once per frame
	IEnumerator ServerRefresh () {
		while (true) {
			if (ServerManager.Instance.IsReady) {
				UnityWebRequest requestObj = UnityWebRequest.Get(requestEndPoint + String.Format (operationPath, RoomId, RegionType, CalculatedForce));
				CalculatedForce = 0;
				yield return requestObj.Send();

				ServerFrame receivedFrame = JsonUtility.FromJson<ServerFrame> (requestObj.downloadHandler.text);
				if (ServerFrame == null) {
					ServerFrame = receivedFrame;
					Transform background = GameObject.Find ("BG").transform; //TODO: parametric
					background.position = new Vector3 (receivedFrame.x, receivedFrame.y, 0f);
					background.GetComponent<BackgroundController> ().enabled = true;
					if (OnServerFrame != null) {
						OnServerFrame (this, ServerFrame);
					}
				} else if (receivedFrame.time > ServerFrame.time) {
					long previousTimeEpoch = (ServerFrame != null) ? ServerFrame.time : 0;
					if (ServerFrame.health > receivedFrame.health) {
						if (OnPlanetDamaged != null) {
							OnPlanetDamaged (this, ServerFrame, ServerFrame.health - receivedFrame.health);
						}
					}
					ServerFrame = receivedFrame;
					ServerFrame.deltaTime = (ServerFrame.time - previousTimeEpoch) / 1000.0f;
					if (OnServerFrame != null) {
						OnServerFrame (this, ServerFrame);
					}
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	public void SetReady(){
		IsReady = true;
	}
}
