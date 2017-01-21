using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerFrame {
	public int time = 0;
	public float x = 0;
	public float y = 0;
	public Meteroid[] meteroids;
	public Jumper jumpers;
}

public class Meteroid{
	public float x;
	public float y;
}
public class Jumper{
	public int w;
	public int e;
	public int s;
	public int n;
}