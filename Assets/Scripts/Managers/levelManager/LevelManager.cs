using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonComponent<LevelManager> {
	public int Year;
	public int EraLevel = 1;
	public GameObject Planet;
	public MeteroidManager MetroidManager;
	public Transform BackgroundWrapper;

	// Use this for initialization
	void Start () {
		ServerManager.Instance.OnServerFrame += OnServerFrame;
	}

	void OnServerFrame (object sender, ServerFrame sf)
	{
		Year = sf.health;
		if (Year >= 1980){
			EraLevel = 3;
			ChangeTheme ();
		}else if (Year >= 1000){
			EraLevel = 2;
			ChangeTheme (); 
		}else{
			EraLevel = 1;
			ChangeTheme (); 
		}
	}

	public void ChangeTheme (){
		string path = "Textures/" + GetEraName () + "/";
		Planet.GetComponent<SpriteRenderer> ().sprite = GetSprite (path + "Planet");

		foreach (Transform bg in BackgroundWrapper) {
			bg.GetComponent<SpriteRenderer> ().sprite = GetSprite (path + "BG");;
		}
	}

	public string GetEraName(){
		switch (EraLevel) {
		case 1:
			return "StoneAge";
		case 2:
			return "Renaisance";
		case 3:
			return "80s";
		case 4:
			return "ModernAge";
		}
		return "";
	}

	public Sprite GetSprite(string path){
		/*Texture2D tex = Resources.Load<Texture2D>(path);
		Rect rect = new Rect (0, 0, tex.width, tex.height);
		Vector2 pivot = new Vector2(0.5f ,0f);
		Sprite sp = Sprite.Create(tex, rect, pivot, 1);
		sp.pixelsPerUnit*/
		return Resources.Load<Sprite>(path);
	}

}
