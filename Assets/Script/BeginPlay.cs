using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPlay : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate ()
        {
            GameMode.Get().StartGame();
            StartCoroutine(HideMenu());
        });

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    
    IEnumerator HideMenu()
    {
        for (float i = 1; i >= 0; i -= 0.02f)
        {
            GameObject.Find("MainMenuPanel").transform.localScale = new Vector3(i, i, 1);
            GameObject.Find("MainUI").transform.localScale = new Vector3(1f - i, 1f - i, 1);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
    
}
