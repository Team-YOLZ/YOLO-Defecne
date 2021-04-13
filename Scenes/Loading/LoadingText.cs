using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class LoadingText : Loading
{
    [SerializeField]
    private Text Loading_text;

    protected override void Init()  //부모 클래스가 실행해줌
    {
        base.Init();
        StartCoroutine(Loader_Text()); //0.8초마다 팁 보여주기

    }

    IEnumerator Loader_Text()
    {
        while (true)
        {
            int rand = Random.Range(0, 4);
            switch (rand)
            {
                case 0:
                    Loading_text.text = "팁 : 0";
                    break;
                case 1:
                    Loading_text.text = "팁 : 1";
                    break;
                case 2:
                    Loading_text.text = "팁 : 2";
                    break;
                case 3:
                    Loading_text.text = "팁 : 3";
                    break;
            }
            yield return new WaitForSeconds(.8f);
        }
    }
}
