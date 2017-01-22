using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BackgroundController : MonoBehaviour {

	[SerializeField]
	private Transform planet;

	[SerializeField]
	private float width = 102f;

	[SerializeField]
	private Transform center;

	void Start(){
		//init center
		Debug.Log("Start of BG controller");
		foreach (Transform childBg in transform) {
			SpriteRenderer sr = childBg.GetComponent<SpriteRenderer> ();
			Rect rect = new Rect (childBg.position, sr.bounds.size/2);
			bool containsPlanet = rect.Contains (planet.position);// (planet.position, minn, maxx);

			if (containsPlanet) {
				center = childBg;
				Debug.Log("Start of BG controller - found center." + sr.bounds.size);
			}
		}
	}

	// Use this for initialization
	void Update () {
		//return;
		// planet
		foreach (Transform childBg in transform) {
			SpriteRenderer sr = childBg.GetComponent<SpriteRenderer> ();
			Rect rect = new Rect (childBg.position, sr.bounds.size/2);
			bool containsPlanet = rect.Contains (planet.position);// (planet.position, minn, maxx);

			float centerDist = Vector3.Distance (planet.position, center.position);
			float childDist = Vector3.Distance (planet.position, childBg.position);

			if (childDist < centerDist) {
				Debug.Log("BG controller - new found center.");

				Directions direction = FindDirection (center.position, childBg.position);
				if (direction == Directions.NA) {
					Debug.LogError ("Direction is NA");
					break;
				}

				//shift & assign new center.
				center = childBg;

				Shift (direction);
				break;
			}
		}
	}

	private Directions FindDirection(Vector3 oldCenter, Vector3 newCenter){
		if (oldCenter.x < newCenter.x)
			return Directions.RIGHT;
		else if (oldCenter.x > newCenter.x)
			return Directions.LEFT;
		else if (oldCenter.y > newCenter.y)
			return Directions.DOWN;
		else if (oldCenter.y < newCenter.y)
			return Directions.UP;
		else
			return Directions.NA;
	}

	private bool Contains (Vector3 pos, Vector3 minn, Vector3 maxx){
		return pos.x < maxx.x && pos.y < maxx.y && pos.x > minn.y && pos.y > minn.y;
	}

	private void Shift(Directions direction){
		List<Distances> children = new List<Distances> ();

		//find 3 bg with max distance to new center.
		foreach (Transform childBg in transform) {
			float d = Vector3.Distance (childBg.position, center.position);
			children.Add (new Distances{t= childBg, dist=d});
		}
		children.Sort ((x, y) => -x.dist.CompareTo(y.dist));
		for (int i = 0; i < 3; i++) {
			Vector3 newPos = children [i].t.position;
			if (direction == Directions.RIGHT)
				newPos.x += width * 3;
			else if (direction == Directions.LEFT)
				newPos.x -= width * 3;
			else if (direction == Directions.DOWN)
				newPos.y -= width * 3;
			else if (direction == Directions.UP)
				newPos.y += width * 3;
			children [i].t.position = newPos;
		}
	}

	private class Distances {
		public Transform t;
		public float dist;
	}

	private enum Directions{
		UP, RIGHT, DOWN, LEFT, NA
	}
}
