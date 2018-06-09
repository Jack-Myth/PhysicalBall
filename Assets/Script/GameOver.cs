using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    bool IsScoreEnough = false;

    ArrayList RankItemCollection=new ArrayList();
        
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowGameOver()
    {
        StartCoroutine(showGameOver());
    }

    IEnumerator showGameOver()
    {
        IsScoreEnough = false;
        GameObject.Find("InputName").transform.localScale=Vector3.zero;
        GameObject RankContent = GameObject.Find("RankContent");
        for(int i=0;i< RankItemCollection.Count;i++)
        {
            Destroy((GameObject)RankItemCollection[i]);
        }
        for (float i = 0; i <= 1; i += 0.02f)
        {
            gameObject.transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForFixedUpdate();
        }
        WWW tmpWww = new WWW("http://jackmyth.cn/GameInterface/PhysicalBall/GetScore.php");
        yield return tmpWww;
        if(tmpWww.error!=null)
        {
            yield break;
        }
        var ItemCollection= tmpWww.text.Split("\n".ToCharArray(),System.StringSplitOptions.RemoveEmptyEntries);
        if(ItemCollection.Length<49)
        {
            IsScoreEnough = true;
            GameObject.Find("InputName").transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            ItemCollection[ItemCollection.Length - 1] = ItemCollection[ItemCollection.Length - 1].Replace("\\|/", "|");
            string[] atmp = ItemCollection[ItemCollection.Length - 1].Split("|".ToCharArray());
            if (int.Parse(atmp[1])<GameMode.Get().GameScore)
            {
                IsScoreEnough = true;
                GameObject.Find("InputName").transform.localScale = new Vector3(1, 1, 1);
            }
        }
        for(int i=0;i<ItemCollection.Length;i++)
        {
            ItemCollection[i] = ItemCollection[i].Replace("\\|/", "|");
            GameObject tmpRankItem = new GameObject();
            tmpRankItem.AddComponent<Text>();
            tmpRankItem.transform.SetParent(RankContent.transform);
            tmpRankItem.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            tmpRankItem.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
            tmpRankItem.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 20);
            tmpRankItem.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
            tmpRankItem.GetComponent<Text>().alignment=TextAnchor.MiddleLeft;
            tmpRankItem.transform.localPosition = new Vector3(0, i * -20, 0);
            tmpRankItem.transform.localScale = new Vector3(1, 1, 1);
            tmpRankItem.GetComponent<Text>().text = " "+(i+1).ToString()+"."+ ItemCollection[i].Replace("|", ": ");
            tmpRankItem.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            tmpRankItem.GetComponent<Text>().fontSize = 15;
            RankItemCollection.Add(tmpRankItem);
            RankContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (i+1) * 20);
        }
    }

    public void HideGameOver()
    {
        StartCoroutine(hideGameOver());
    }

    IEnumerator hideGameOver()
    {
        for (float i = 1; i >= 0; i -= 0.02f)
        {
            gameObject.transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForFixedUpdate();
        }
        for (float i = 0; i <= 1; i += 0.02f)
        {
            GameObject.Find("MainMenuPanel").transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForFixedUpdate();
        }
        string PlayerName = GameObject.Find("InputName").GetComponent<InputField>().text;
        PlayerName.Replace("\\|/", "");
        PlayerName.Replace("|", "");
        if (IsScoreEnough && PlayerName != "")
        {
            WWW tmpWWW = new WWW("http://jackmyth.cn/GameInterface/PhysicalBall/SendScore.php?name=" + PlayerName + "&score=" + GameMode.Get().GameScore);
            yield return tmpWWW;
            //var texta=tmpWWW.text;
        }
        yield break;
    }
}
