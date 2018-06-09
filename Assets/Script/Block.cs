using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    private int health = 1;

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
            GetComponentInChildren<TextMesh>().text = health.ToString();
        }
    }

    // Use this for initialization
    void Start () {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(1, 359));
        GetComponentInChildren<TextMesh>().transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.z);
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.7f, 1f), Random.Range(0.7f, 1f), Random.Range(0.7f, 1f));
        StartCoroutine(Bigger(gameObject.transform.localScale));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator Bigger(Vector3 TargetScale)
    {
        for(float i=0;i<=1;i+=0.07f)
        {
            gameObject.transform.localScale = Vector3.Lerp(Vector3.zero, TargetScale,i);
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var BallInstance = collision.gameObject.GetComponent<Ball>();
        if (BallInstance)
        {
            Health -= BallInstance.Damage;
            GameMode.Get().GameScore = GameMode.Get().GameScore + BallInstance.Damage;
            if (Health<=0)
                PostBlockDestroyed();
        }
    }

    void PostBlockDestroyed()
    {
        GameMode.Get().BlockToDestroy(gameObject);
    }

    public IEnumerator ShakeWarning()
    {
        Vector3 OriginalPosition = gameObject.transform.position;
        for(int i=0;i<30;i++)
        {
            gameObject.transform.position = OriginalPosition + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
            yield return new WaitForFixedUpdate();
        }
        gameObject.transform.position = OriginalPosition;
        yield break;
    }
}
