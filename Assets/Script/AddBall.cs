using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBall : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameMode.Get().SpawnBall();
        GameMode.Get().PostBallRecycled();
        GameMode.Get().BlockToDestroy(gameObject);
    }
}
