using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static Define;

public class TitleScene : BaseScene
{
    public GameObject _titleIamge; //타이틀 화면
    public GameObject _loginIamge; //로그인 화면
    public GameObject _signupIamge; //회원가입 화면
    public GameObject _screenCoverIamge; //로그인화면 가리개 이미지
    public Text _startText;

    private TitleSceneState _nowSceneState = TitleSceneState.Title; //화면상태 초기값 타이틀화면

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
        _titleIamge.SetActive(true); 
        _loginIamge.SetActive(false);
        _signupIamge.SetActive(false);
        _screenCoverIamge.SetActive(false);
    }
    public void GoLogin()
    {
        _titleIamge.SetActive(false);
        _loginIamge.SetActive(true);
        _signupIamge.SetActive(false);
        _screenCoverIamge.SetActive(false);

        _nowSceneState = TitleSceneState.Login; //현재 화면 상태 로그인화면
    }

    public void GoSignUp()
    {
        _titleIamge.SetActive(false);
        _loginIamge.SetActive(true);
        _signupIamge.SetActive(true);
        _screenCoverIamge.SetActive(true);

        _nowSceneState = TitleSceneState.SignUp;  //현재 화면 상태 회원가입화면
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
