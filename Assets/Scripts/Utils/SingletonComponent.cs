using System;
using UnityEngine;

public abstract class SingletonComponent<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static T Instance {
		get {
			return CreateOrGetInstance();
		}
	}

	public static T CreateOrGetInstance() {
		if (_instance == null) {
			T instance = FindInstance();
			if (instance == null) {
				instance = CreateInstance();
			}
			SetInstanceAttributes(instance);
		}
		return _instance;
	}

	protected virtual void Awake() {
		if (_instance == null) {
			// this is the only instance
			SetInstanceAttributes(this as T);
		}

		if (_instance != this) {
			// there is already another instance, kill this one
			Destroy(gameObject);
		}
	}

	protected virtual void OnDestroy() {
		if(_instance == this)
		{
			_instance = null;
		}
	}

	public static bool IsManagerActive() {
		return _instance != null;
	}

	private static T FindInstance() {
		return FindObjectOfType<T>();
	}

	private static T CreateInstance() {
		string goName = "_" + typeof(T).Name;
		GameObject go = GameObject.Find(goName);
		if (go == null) {
			go = new GameObject();
		}
		return go.AddComponent<T>();
	}

	private static void SetInstanceAttributes(T instance) {
		_instance = instance;
		_instance.gameObject.name = "_" + typeof(T).Name;
		_instance.gameObject.tag = "Manager";
		DontDestroyOnLoad(_instance.gameObject);
	}
}
