using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServerManager : SingletonComponent<ServerManager> {
	public int RoomId = 1;
	public string RegionType = "w";
	public float CalculatedForce = 1;
	public ServerFrame ServerFrame;

	public delegate void ServerFrameEvent(object sender, ServerFrame sf);
	public event ServerFrameEvent OnServerFrame;

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
			WWW requestObj = new WWW (requestEndPoint + String.Format(operationPath, RoomId, RegionType, CalculatedForce));
			yield return requestObj;
			ServerFrame receivedFrame = JsonUtility.FromJson<ServerFrame>(requestObj.text);
			if (ServerFrame == null) {
				ServerFrame = receivedFrame;
				if (OnServerFrame != null) {
					OnServerFrame (this, ServerFrame);
				}
			}else if (receivedFrame.time > ServerFrame.time) {
				long previousTimeEpoch = (ServerFrame != null) ? ServerFrame.time : 0;
				ServerFrame = receivedFrame;
				ServerFrame.deltaTime = (ServerFrame.time - previousTimeEpoch) / 1000.0f;
				if (OnServerFrame != null) {
					OnServerFrame (this, ServerFrame);
				}
			}
			yield return new WaitForSeconds (0.5f);
		}
	}
}
