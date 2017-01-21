using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeteroidManager : MonoBehaviour{
	public List<GameObject> meteroidPrefabs = new List<GameObject>();
	public List<MeteroidController> activeMeteroids = new List<MeteroidController>();
	private bool inProgress = false;

	// Use this for initialization
	void OnEnable () {
		ServerManager.Instance.OnServerFrame += OnServerFrame;
	}

	void OnDisable () {
		ServerManager.Instance.OnServerFrame -= OnServerFrame;
	}

	public void OnServerFrame (object sender, ServerFrame sf)
	{
		if (!inProgress) {
			inProgress = true;
			Debug.Log ("OnServerFrame call MeteorManager");
			if (sf.meteroids == null){
				Debug.Log ("Meteroid array is null");
			}else{
				MeteorMovement (sf);
			}
			inProgress = false;
		}
	}

	private void MeteorMovement(ServerFrame sf){
		Meteroid[] meteroids = sf.meteroids;
		//Init And Move
		foreach (Meteroid m in meteroids) {
			MeteroidController mc = activeMeteroids.Find(x => x.id == m.id);
			if (mc == null) {
				GameObject gmc = GameObject.Instantiate (meteroidPrefabs [UnityEngine.Random.Range (0, meteroidPrefabs.Count)], 
					new Vector3 (m.x, m.y), Quaternion.Euler (new Vector3 (0, 0, m.angle)));
				gmc.transform.SetParent(transform);
				mc = gmc.GetComponent<MeteroidController> ();
				mc.SetId (m.id);
				activeMeteroids.Add (mc);
			}
			mc.SetPosition (new Vector3 (m.x, m.y), sf.deltaTime);
		}
		//
		List<MeteroidController> activeMeteroidsTemp = new List<MeteroidController>();
		foreach (MeteroidController mc in activeMeteroids) {
			//check if active meteor is in sf.meteors. if not destroy it.
			Meteroid m = meteroids.ToList().Find(x => x.id == mc.id);
			if (m == null) {
				GameObject.Destroy (mc.gameObject);
			}
			else
				activeMeteroidsTemp.Add(mc);
		}
		activeMeteroids = activeMeteroidsTemp;
	}

}
