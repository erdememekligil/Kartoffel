using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour {
	public Transform TargetToFollow;
	// Use this for initialization
	void Start () {
		ServerManager.Instance.OnServerFrame += OnServerFrame;
	}

	void OnServerFrame (object sender, ServerFrame sf)
	{
		if (TargetToFollow != null) {
			transform.DOMove(TargetToFollow.position, sf.deltaTime).SetEase(Ease.InOutCubic);
		}
	}

}
