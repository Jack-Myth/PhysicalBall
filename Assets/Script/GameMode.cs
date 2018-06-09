using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMode  {
    
    ArrayList BallCollection, BlockCollection;
    static GameMode MyInstance=null;
    int GameRound = 1;
    private int gameScore = 0;

    public int GameScore
    {
        get
        {
            return gameScore;
        }

        set
        {
            gameScore = value;
            GameObject.Find("Score").GetComponent<Text>().text = "Score: " + gameScore.ToString();
        }
    }

    private GameMode()
    {
        BallCollection = new ArrayList();
        BlockCollection = new ArrayList();
    }

    public static GameMode Get()
    {
        if (MyInstance == null)
            MyInstance = new GameMode();
        return MyInstance;
    }

    public void SpawnBall()
    {
        var FirstBall = Object.Instantiate(Resources.Load<GameObject>("Ball"));
        FirstBall.transform.position = new Vector3(0, 5.5f, 3);
        FirstBall.GetComponent<Ball>().IsRecycled = true;
        BallCollection.Add(FirstBall);
    }
    public void StartGame()
    {
        if(IsGamePlaying())
        {
            Debug.LogWarning("Game Is Already Started!");
            return;
        }
        GameScore = 0;
        GameRound = 1;
        SpawnBall();
        PostBallRecycled();
    }

    public void EndGame()
    {
        for (int i = 0; i < BallCollection.Count; i++)
        {
            Object.Destroy((GameObject)BallCollection[i]);
        }
        BallCollection.Clear();
        for (int i = 0; i < BlockCollection.Count; i++)
        {
            Object.Destroy((GameObject)BlockCollection[i]);
        }
        BlockCollection.Clear();
        GameObject.Find("BallLauncher").GetComponent<BallLauncher>().Reset();
    }

    public bool IsGamePlaying()
    {
        return BallCollection.Count > 0;
    }

    public ArrayList GetBallCollection()
    {
        return BallCollection;
    }

    public ArrayList GetBlockCollection()
    {
        return BlockCollection;
    }

    public void PostBallRecycled()
    {
        for(int i=0;i<BallCollection.Count;i++)
        {
            if (!((GameObject)BallCollection[i]).GetComponent<Ball>().IsRecycled)
                return;
        }
        GameObject.Find("BallLauncher").GetComponent<BallLauncher>().ReadyLaunch();
    }
    public void BlockToDestroy(GameObject BlocktoDestroy)
    {
        BlockCollection.Remove(BlocktoDestroy);
        Object.Destroy(BlocktoDestroy);
    }

    public void SpawnBlockLine(int Count)
    {
        Count = Mathf.Clamp(Count, 0, 5);
        char[] BlockPosition=new char[5];
        for (int i = 0; i < 5; i++)
            BlockPosition[i] = (char)0;
        while (Count>0)
        {
            int RandomPosition = Random.Range(0, 4);
            if (BlockPosition[RandomPosition] != 0)
                continue;
            BlockPosition[RandomPosition] = (char)1;
            Count--;
        }
        for(int i=0;i<5;i++)
        {
            if (BlockPosition[i]==0&&Random.Range(0,10)<=1)
            {
                BlockPosition[i] = (char)2;
            }
        }
        for(int i=0;i<5;i++)
        {
            switch ((int)BlockPosition[i])
            {
                case 1:
                    {
                        var BlockTri = Object.Instantiate(Resources.Load<GameObject>("BlockTri"));
                        BlockTri.transform.position = new Vector3((i - 2)*0.9f, -3.8f);
                        BlockTri.GetComponent<Block>().Health = Random.Range(GameRound, GameRound*3);
                        BlockCollection.Add(BlockTri);
                    }
                    break;
                case 2:
                    {
                        var AddBall = Object.Instantiate(Resources.Load<GameObject>("AddBall"));
                        AddBall.transform.position = new Vector3((i - 2)*0.9f, -3.8f);
                        BlockCollection.Add(AddBall);
                    }
                    break;
            }
        }
        GameRound++;
    }
    public void PostGameOver()
    {
        for(int i=0;i<BallCollection.Count;i++)
        {
            Object.Destroy((GameObject)BallCollection[i]);
        }
        BallCollection.Clear();
        for (int i = 0; i < BlockCollection.Count; i++)
        {
            Object.Destroy((GameObject)BlockCollection[i]);
        }
        BlockCollection.Clear();
        GameObject.Find("BallLauncher").GetComponent<BallLauncher>().Reset();
        GameObject.Find("GameOver").GetComponent<GameOver>().ShowGameOver();
    }
}
