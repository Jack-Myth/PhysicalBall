using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Camera>().aspect = 9 / 16f;
        GameObject.Find("UIRoot").transform.localScale = new Vector3(Screen.width / 405f, Screen.height / 720f, 1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
