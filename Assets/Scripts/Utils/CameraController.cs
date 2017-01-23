using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {
	public Transform TargetToFollow;
	// Use this for initialization
	void Start () {
		//ServerManager.Instance.OnServerFrame += OnServerFrame; //This is not necessary since camera moves with planet.
	}

//	void OnServerFrame (object sender, ServerFrame sf)
//	{
//		if (TargetToFollow != null) {
//			//transform.DOMove(TargetToFollow.position, sf.deltaTime).SetEase(Ease.Linear);
////			transform.position.Set (TargetToFollow.position.x, TargetToFollow.position.y, TargetToFollow.position.z);
//			//transform.position = TargetToFollow.position;
//		}
//	}

	void Update(){
		if (TargetToFollow != null) {
			//transform.DOMove(TargetToFollow.position, sf.deltaTime).SetEase(Ease.Linear);
			//			transform.position.Set (TargetToFollow.position.x, TargetToFollow.position.y, TargetToFollow.position.z);
			transform.parent.position = TargetToFollow.position;
		}
	}

}
