using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerFrame {
	public long time = 0;
	public int health = 0;
	public float deltaTime = 0;
	public float x = 0;
	public float y = 0;
	public Meteroid[] meteroids;
	public Jumper jumpers;
}
	
[System.Serializable]
public class Meteroid {
	public int id;
	public float x;
	public float y;
	public float angle;
}

[System.Serializable]
public class Jumper{
	public int w;
	public int e;
	public int s;
	public int n;
}