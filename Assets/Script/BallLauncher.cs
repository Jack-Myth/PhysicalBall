using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour {

    GameObject BallToPut;
    bool ReadyToLaunch = false;
    Vector3 MousePressPosition=new Vector3();
    bool IsMousePressed = false;
    bool IsBallHolding = false;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            IsMousePressed = true;
        if (Input.GetMouseButtonUp(0))
            IsMousePressed = false;
    }

    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.GetComponent<Ball>()&& collision.transform.GetComponent<Ball>().IsRecycled)
        {
            if (IsBallHolding)
                yield break;
            IsBallHolding = true;
            collision.transform.GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<BoxCollider2D>().isTrigger = false;
            BallToPut = collision.gameObject;
            float PutAlpha = 0;
            while (PutAlpha<=1)
            {
                BallToPut.transform.position = new Vector3(Mathf.Lerp(BallToPut.transform.position.x, 0f, PutAlpha), Mathf.Lerp(BallToPut.transform.position.y, 3.4f, PutAlpha));
                PutAlpha = PutAlpha + 0.05f;
                yield return new WaitForFixedUpdate();
            }
            ReadyToLaunch = true;
        }
    }

    public void ReadyLaunch()
    {
        //StopCoroutine("GetLaunchDirection");
        StartCoroutine(GetLaunchDirection());
    }

    IEnumerator GetLaunchDirection()
    {
        while(!ReadyToLaunch)
            yield return new WaitForFixedUpdate();
        ArrayList BallCollection = GameMode.Get().GetBallCollection();
        for (int i = 0; i < BallCollection.Count; i++)
        {
            if (!((GameObject)BallCollection[i]).GetComponent<Ball>().IsRecycled)
                yield break;
        }
        ReadyToLaunch = false;
        ArrayList BlockCollection = GameMode.Get().GetBlockCollection();
        for (int i = 0; i < BlockCollection.Count; i++)
        {
            if (((GameObject)BlockCollection[i]).transform.position.y >= 1.9)
            {
                if (((GameObject)BlockCollection[i]).GetComponent<Block>())
                {
                    GameMode.Get().PostGameOver();
                    yield break;
                }
                else
                {
                    GameMode.Get().BlockToDestroy(((GameObject)BlockCollection[i]));
                }
            }
        }
        float[] TargetY = new float[BlockCollection.Count];
        for(int i=0;i<BlockCollection.Count;i++)
        {
            TargetY[i]= ((GameObject)BlockCollection[i]).transform.position.y;
        }
        for (float b=0;b<1f;b+=0.05f)
        {
            for (int i = 0; i < BlockCollection.Count; i++)
            {
                ((GameObject)BlockCollection[i]).transform.position = new Vector3(((GameObject)BlockCollection[i]).transform.position.x, Mathf.Lerp(TargetY[i], TargetY[i]+1.5f,b));
            }
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < BlockCollection.Count; i++)
            if (((GameObject)BlockCollection[i]).transform.position.y >= 1.9)
            {
                var BlockInstance = ((GameObject)BlockCollection[i]).GetComponent<Block>();
                if (BlockInstance)
                    StartCoroutine(BlockInstance.ShakeWarning());
            }
        GameMode.Get().SpawnBlockLine(Random.Range(1, 5));
        GameObject AimLine=null;
        while (true)
        {
            if (IsMousePressed)
            {
                //MousePressPosition = GameObject.Find("Main Camera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(0,3.4f));
                MousePressPosition = Input.mousePosition;
                MousePressPosition.x = MousePressPosition.x / (Screen.width / 405f);
                MousePressPosition.y = MousePressPosition.y / (Screen.height / 720f);
                AimLine = Instantiate(Resources.Load<GameObject>("AimLine"));
                AimLine.transform.position = new Vector3(0, 3.4f);
                while(true)
                {
                    Vector3 tmpMousePosition = Input.mousePosition;
                    tmpMousePosition.x = tmpMousePosition.x /(Screen.width / 405f);
                    tmpMousePosition.y = tmpMousePosition.y /(Screen.height / 720f);
                    if (!IsMousePressed)
                    {
                        if (AimLine)
                            Destroy(AimLine);
                        StartCoroutine(LaunchBall(tmpMousePosition - MousePressPosition));
                        yield break;
                    }
                    if (IsMousePressed && AimLine)
                    {
                        AimLine.transform.localScale = new Vector3((tmpMousePosition - MousePressPosition).magnitude / 944f, 1, 1);
                        AimLine.transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(new Vector3(1, 0, 0), tmpMousePosition - MousePressPosition, new Vector3(0, 0, 1)));
                    }
                    yield return new WaitForFixedUpdate();
                }
            }
            yield return new WaitForFixedUpdate();
         }
    }

    IEnumerator LaunchBall(Vector3 LaunchDirection)
    {
        LaunchDirection.Scale(new Vector3(0.02f, 0.02f));
        Debug.Log((LaunchDirection).ToString());
        //GetComponent<BoxCollider2D>().isTrigger = true;
        var BallCollection = GameMode.Get().GetBallCollection();
        for(int i=0;i<BallCollection.Count;i++)
        {
            ((GameObject)BallCollection[i]).transform.position = new Vector3(0, 3.4f, 0);
            ((GameObject)BallCollection[i]).GetComponent<Ball>().IsRecycled = false;
            ((GameObject)BallCollection[i]).GetComponent<Collider2D>().sharedMaterial= Resources.Load<PhysicsMaterial2D>("PhyMat");
            ((GameObject)BallCollection[i]).GetComponent<Rigidbody2D>().simulated = true;
            ((GameObject)BallCollection[i]).GetComponent<Rigidbody2D>().velocity = LaunchDirection;
            yield return new WaitForSeconds(0.2f);
        }
        IsBallHolding = false;
    }


    public void Reset()
    {
        ReadyToLaunch = false;
        MousePressPosition = new Vector3();
        IsMousePressed = false;
        IsBallHolding = false;
    }
}
