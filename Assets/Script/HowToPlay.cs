using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlay : MonoBehaviour {

    bool IsHTPShowed = false;
	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(delegate ()
        {
            if (IsHTPShowed)
                return;
            IsHTPShowed = true;
            StartCoroutine(ShowHowToPlay());
        });
        GameObject.Find("HowToPlayImage").transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ShowHowToPlay()
    {
        GameObject HTP = GameObject.Find("HowToPlayImage");
        for(float i=0;i<1;i+=0.02f)
        {
            HTP.transform.localScale = new Vector3(i, i, 1);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
}
