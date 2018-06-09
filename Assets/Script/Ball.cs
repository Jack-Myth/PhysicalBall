using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public int Damage = 1;
    public bool IsRecycled = true;
    Vector3 LastPositon;
    int JumpCount = 0;
	// Use this for initialization
	void Start () {
        Invoke("CheckPosition",0.02f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void CheckPosition()
    {
        if(!IsRecycled)
        {
            if (LastPositon == gameObject.transform.position)
            {
                if (++JumpCount >= 100)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector3(0, 5, 0);
                }
            }
            else
                JumpCount = 0;
        }
        LastPositon = gameObject.transform.position;
        Invoke("CheckPosition", 0.02f);
    }
}
