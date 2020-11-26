using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScene : BaseScene
{
    public Text _startText;

    public override void Clear()
    {
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Title;
    }
    void Start()
    {
        Init();
        StartCoroutine(StartTextCon());
    }
    public void GoMain()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }
    IEnumerator StartTextCon()
    {
        while (true)
        {
            _startText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _startText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }
    }
}
