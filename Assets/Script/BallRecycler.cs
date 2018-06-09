using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRecycler : MonoBehaviour {

    public bool IsL = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ball Ballinstance = collision.transform.GetComponent<Ball>();
        if (Ballinstance)
        {
            collision.sharedMaterial = null;
            StartCoroutine(StartRecycle(collision.gameObject));
        }
    }

    IEnumerator StartRecycle(GameObject BalltoRecycle)
    {
        BalltoRecycle.GetComponent<Rigidbody2D>().simulated = false;
        Vector3 MidPos = new Vector3(2.8f * (IsL ? -1 : 1), -4.7f, 0);
        Vector3 StartPos = BalltoRecycle.transform.position;
        for (float i=0;i<=1;i+=0.05f)
        {
            BalltoRecycle.transform.position = Vector3.Lerp(StartPos, MidPos, i);
            yield return new WaitForFixedUpdate();
        }
        MidPos = new Vector3(2.8f * (IsL ? -1 : 1), 4.7f, 0);
        StartPos = BalltoRecycle.transform.position;
        for (float i = 0; i <= 1; i += 0.02f)
        {
            BalltoRecycle.transform.position = Vector3.Lerp(StartPos, MidPos, i);
            yield return new WaitForFixedUpdate();
        }
        BalltoRecycle.GetComponent<Rigidbody2D>().simulated = true;
        BalltoRecycle.GetComponent<Rigidbody2D>().velocity = new Vector3(IsL ? 3f : -3f, 2f, 0);
        BalltoRecycle.GetComponent<Ball>().IsRecycled = true;
        GameMode.Get().PostBallRecycled();
        yield break;
    }

}
