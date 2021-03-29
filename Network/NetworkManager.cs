using UnityEngine;
using BackEnd;
using LitJson;
using System;
using UnityEngine.UI;
using static Define;

public class NetworkManager : MonoBehaviour
{

    //비동기로 가입, 로그인을 할때에는 Update()에서 처리를 해야합니다. 이 값은 Update에서 구현하기 위한 플래그 값 입니다.
    BackendReturnObject bro = new BackendReturnObject();
    bool isSuccess = false;
    bool isSuccessNicknamechecked = false; //닉네임 중복확인 여부


    string updatedPW = "c8xtwbeu";
    string email = "초기화된 비밀번호 받을 이메일 주소";
    string nicknameDuplitionText = ""; //닉네임 중복 확인하고 성공시 여기에 저

    public InputField idInput;
    public InputField PWInput;
    public InputField NicknameInput;

    public InputField SignUp_idInput; //회원가입 아이디 입력필드
    public InputField SignUp_PWInput; //회원가입 비밀번호 입력필드
    public InputField SignUp_NicknameInput; //회원가입 닉네임 입력필드

    public GameObject _loginIamge; //로그인 화면
    public GameObject _screenCoverIamge; //로그인화면 가리개 이미지
    public GameObject _signupIamge; //회원가입 화면
    public GameObject _signupNicknameIamge; //회원가입 닉네임 화면
    public GameObject _messageBoxImage; //메세지박스 화면

    public Text LoginError_Text; //로그인 에러 텍스트
    public Text SignUpError_Text; //회원가입 에러 텍스트
    public Text NicknameError_Text; //닉네임 중복 확인 텍스트
    public Text MessageBox_Text; //메세지박스 텍스트


    protected TitleSceneState _nowSceneState = TitleSceneState.Login;  //초기값 로그인화면



    void Start()
    {

        Backend.Initialize(HandleBackendCallback);
    }

    void MessageBox()  //메세지 박스 출렧
    {
        _messageBoxImage.SetActive(true);
        switch (_nowSceneState)
        {
            case TitleSceneState.Login:
                break;
            case TitleSceneState.SignUp:
                break;
            case TitleSceneState.SignUpNickname:
                MessageBox_Text.text = "회원가입을 축하 합니다!";
                break;
            case TitleSceneState.SignUpNicknameCancle:
                MessageBox_Text.text = "로그인시 다시 닉네임을 설정할 수 있습니다.";
                break;

        }


    }

    public void MessageBoxCheckBtn()  //메세지박스 버튼 클릭 
    {
        switch (_nowSceneState)
        {
            case TitleSceneState.Login:
                break;
            case TitleSceneState.SignUp:
                break;
            case TitleSceneState.SignUpNickname:
                Managers.Scene.LoadScene(Define.Scene.Main);
                Debug.Log("로비로 고고고!");
                break;
            case TitleSceneState.SignUpNicknameCancle:
                ShowLogin();
                break;

        }
        CleanMessageBox();
    }

    public void GoLoginFromSignUp()  //회원가입화면 ->로그인화면
    {
        CleanLogin();
        CleanSignUp();
        ShowLogin();
    }

    public void GoLoginFromNickname() //닉네임생성화면 -> 로그인화면
    {
        _nowSceneState = TitleSceneState.SignUpNicknameCancle;
        CleanLogin();
        CleanSignUp();
        CleanSignUpNickname();
        MessageBox();
    }

    void CleanLogin()
    {
        idInput.text = "";
        PWInput.text = "";
        LoginError_Text.text = "";
    }
    void ShowLogin()
    {
        _loginIamge.SetActive(true);
        _screenCoverIamge.SetActive(false);
        _signupIamge.SetActive(false);
        _signupNicknameIamge.SetActive(false);
        _messageBoxImage.SetActive(false);
    }
    void CleanSignUp()
    {
        SignUp_idInput.text = "";
        SignUp_PWInput.text = "";
        SignUpError_Text.text = "";

    }
    void ShowSignUp()
    {
        _loginIamge.SetActive(true);
        _screenCoverIamge.SetActive(true);
        _signupIamge.SetActive(true);
        _signupNicknameIamge.SetActive(false);
        _messageBoxImage.SetActive(false);
    }
    void CleanSignUpNickname()
    {
        SignUp_NicknameInput.text = "";
        NicknameError_Text.text = "";
    }
    void ShowSignUpNickname()
    {
        _loginIamge.SetActive(true);
        _screenCoverIamge.SetActive(true);
        _signupIamge.SetActive(false);
        _signupNicknameIamge.SetActive(true);
        _messageBoxImage.SetActive(false);
    }
    void CleanMessageBox()
    {
        MessageBox_Text.text = "";
    }

    void HandleBackendCallback()
    {
        if (Backend.IsInitialized)
        {
            // 구글 해시키 획득 
            if (!Backend.Utils.GetGoogleHash().Equals(""))
                Debug.Log(Backend.Utils.GetGoogleHash());

            // 서버시간 획득
            Debug.Log(Backend.Utils.GetServerTime());
        }
        // 실패
        else
        {
            Debug.LogError("Failed to initialize the backend");
        }
    }

    bool InputFieldEmptyCheck(InputField inputField)
    {
        return inputField != null && !string.IsNullOrEmpty(inputField.text);
    }

    // 커스텀 가입
    public void CustomSignUp()
    {
        Debug.Log("-------------CustomSignUp-------------");
        if (InputFieldEmptyCheck(SignUp_idInput) && InputFieldEmptyCheck(SignUp_PWInput)) //입력필드에 전부 입력이 되어있다면
        {
            BackendReturnObject BRO = Backend.BMember.CustomSignUp(SignUp_idInput.text, SignUp_PWInput.text, "tester");
            if (SignUp_idInput.text.Length < 4)  //아이디 글자 수 체크
            {
                SignUpError_Text.text = "4글자 이상의 아이디를 입력해주세요.";
                Debug.Log(" ID  4글자 제한"); 
            }
            else if (SignUp_idInput.text.Length > 15)
            {
                SignUpError_Text.text = "15글자 이하의 아이디를 입력해주세요.";
                Debug.Log(SignUp_idInput.text.Length);
            }
            else if (SignUp_PWInput.text.Length < 6)
            {
                SignUpError_Text.text = "6자리 이상의 비밀번호를 설정해주세요.";
                Debug.Log(" PW  6글자 제한");
            }
            else if (BRO.IsSuccess())  //커스텀 가입 성공
            {
                ShowSignUpNickname();

            }
            else //에러 코드
            {
                string error = BRO.GetStatusCode();
               
                switch (error)
                {
                    case "409":
                        SignUpError_Text.text = "아이디 중복입니다. 다른 아이디를 사용해주세요.";
                        break;
                    case "403":
                        SignUpError_Text.text = "AU 10명을 넘을 수 없습니다. 라이브 설정을 위해 저희 회사는 돈이 필요합니다..";
                        break;
                    default:
                        SignUpError_Text.text = $"서버 공통 에러 발생 +{BRO.GetMessage()}";
                        break;
                }

            }
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
            SignUpError_Text.text = "아이디 또는 비밀번호를 입력해주십시오.";
        }
    }

    public void ACustomSignUp()
    {
        Debug.Log("-------------ACustomSignUp-------------");
        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
            Backend.BMember.CustomSignUp(idInput.text, PWInput.text, NicknameInput.text, isComplete =>
            {
                Debug.Log(isComplete.ToString());

            });
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
        }
    }
    //중복로그인은 뒤끝패스 구매 후 구현 가능 (실시간 알림 기능에서 특정 유저 접속 핸들러 사용)
    // 커스텀 로그인
    public void CustomLogin()
    {
        Debug.Log("-------------CustomLogin-------------");

        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
           
            BackendReturnObject BRO = Backend.BMember.CustomLogin(idInput.text, PWInput.text);
            Debug.Log("로그인 시도 : " + BRO.GetStatusCode() + BRO.GetErrorCode());

            if (BRO.GetErrorCode() == "ForbiddenException")
            {
                LoginError_Text.text = "AU 10명을 넘을 수 없습니다. 라이브 설정을 위해 저희 회사는 돈이 필요합니다..";
            }
            else if (BRO.GetStatusCode() == "403")
            {
                LoginError_Text.text = "차단 당한 유저입니다. 게시판에 문의해주세요.";
            }
            else if (BRO.GetStatusCode() == "401")
            {
                LoginError_Text.text = "아이디 또는 비밀번호를 확인해주세요";
            }
            else if (BRO.IsSuccess())
            {
            JsonData nicknameJson = Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"]["nickname"];//닉네임 받아오기
                if (nicknameJson == null) //회원가입은 됐지만 닉네임을 설정 안했을 시
                {
                    ShowSignUpNickname();
                }
                else
                {
                    Debug.Log("로그인 성공");
                    Managers.Scene.LoadScene(Define.Scene.Main);
                }
       
            }
            else
            {
                LoginError_Text.text = $"서버 공통 에러 발생 +{BRO.GetMessage()}";
            }

        }
        else
        {
            Debug.Log("check IDInput or PWInput");
            LoginError_Text.text = "아이디 또는 비밀번호를 확인해주세요";
        }

    }

    public void ACustomLogin()
    {
        Debug.Log("-------------ACustomLogin-------------");
        if (InputFieldEmptyCheck(idInput) && InputFieldEmptyCheck(PWInput))
        {
            SendQueue.Enqueue(Backend.BMember.CustomLogin, idInput.text, PWInput.text, NicknameInput.text, isComplete =>
            {
                Debug.Log(isComplete.ToString());

            });
        }
        else
        {
            Debug.Log("check IDInput or PWInput");
        }
    }

    // 기기에 저장된 뒤끝 AccessToken으로 로그인 (페더레이션, 커스텀 회원가입 또는 로그인 이후에 시도 가능)
    public void LoginWithTheBackendToken()
    {
        Debug.Log("-------------LoginWithTheBackendToken-------------");
        Debug.Log(Backend.BMember.LoginWithTheBackendToken().ToString());
    }

    public void ALoginWithTheBackendToken()
    {
        Debug.Log("-------------ALoginWithTheBackendToken-------------");
        SendQueue.Enqueue(Backend.BMember.LoginWithTheBackendToken, isComplete =>
        {
            Debug.Log(isComplete.ToString());
        });
    }


    //뒤끝 RefreshToken 을 통해 뒤끝 AccessToken 을 재발급 받습니다
    public void RefreshTheBackendToken()
    {
        Debug.Log("-------------RefreshTheBackendToken-------------");
        Debug.Log(Backend.BMember.RefreshTheBackendToken().ToString());
    }

    public void ARefreshTheBackendToken()
    {
        Debug.Log("-------------ARefreshTheBackendToken-------------");
        // RefreshTheBackendToken 대신 RefreshTheBackendTokenAsync 사용
        SendQueue.Enqueue(Backend.BMember.RefreshTheBackendToken, isComplete =>
        {
            // 성공시 - Update() 문에서 토큰 저장
            Debug.Log(isComplete.ToString());
            isSuccess = isComplete.IsSuccess();
            bro = isComplete;
        });
    }

    // 서버에서 뒤끝 access_token과 refresh_token을 삭제
    public void Logout()
    {
        Debug.Log("-------------Logout-------------");
        Debug.Log(Backend.BMember.Logout().ToString());
    }

    public void ALogout()
    {
        Debug.Log("-------------ALogout-------------");
        SendQueue.Enqueue(Backend.BMember.Logout, isComplete =>
        {
            Debug.Log(isComplete.ToString());
        });
    }

    // 회원 탈퇴 
    public void SignOut()
    {
        Debug.Log("-------------SignOut-------------");
        Debug.Log(Backend.BMember.SignOut("탈퇴 사유").ToString());
    }

    public void ASignOut()
    {
        Debug.Log("-------------ASignOut-------------");
        SendQueue.Enqueue(Backend.BMember.SignOut, "탈퇴 사유", isComplete =>
        {
            Debug.Log(isComplete.ToString());
        });
    }

    public void CheckNicknameDuplication()
    {
        Debug.Log("-------------CheckNicknameDuplication-------------");
        if (InputFieldEmptyCheck(SignUp_NicknameInput))
        {
            BackendReturnObject BRO = Backend.BMember.CheckNicknameDuplication(SignUp_NicknameInput.text);
            if (BRO.IsSuccess())
            {
                Debug.Log(BRO + ", 해당 닉네임으로 설정 가능");
                NicknameError_Text.text = "닉네임 사용 가능";
                isSuccessNicknamechecked = true; //닉네임 사용 가능
                nicknameDuplitionText = SignUp_NicknameInput.text;
            }
            else if(BRO.GetStatusCode()=="409")
            {
                NicknameError_Text.text = "중복된 닉네임 입니다.";
                isSuccessNicknamechecked = false; //닉네임 사용 불가
            }
            else if(BRO.GetStatusCode()=="400")
            {
                NicknameError_Text.text = "앞/뒤 공백을 없애거나 20자 이하 닉네임을 설정해 주십시오.";
                isSuccessNicknamechecked = false;
            }

        }
        else
        {
            NicknameError_Text.text = "닉네임을 입력해 주세요.";
            isSuccessNicknamechecked = false;
        }
        //Debug.Log("-------------CheckNicknameDuplication!!!!-------------");
        //if(InputFieldEmptyCheck(SignUp_NicknameInput))
        //{
        //    BackendReturnObject bro = Backend.BMember.CheckNicknameDuplication(SignUp_NicknameInput.text);
        //    if (bro.IsSuccess())
        //    {
        //        Debug.Log(bro+ ", 해당 닉네임으로 설정 가능");
        //    }
        //    else
        //    {
        //        Debug.Log("닉네임 좀 보셈");
        //    }

        //}
    }

    public void ACheckNicknameDuplication()
    {
        Debug.Log("-------------A CheckNicknameDuplication-------------");

        if (InputFieldEmptyCheck(NicknameInput))
        {
            SendQueue.Enqueue(Backend.BMember.CheckNicknameDuplication, NicknameInput.text, bro =>
            {
                Debug.Log(bro);
            });
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }


    // 닉네임 생성 
    public void CreateNickname()
    {
        Debug.Log("-------------CreateNickname-------------");
        //if (InputFieldEmptyCheck(SignUp_NicknameInput))
        //{
        //    Debug.Log(Backend.BMember.CreateNickname(SignUp_NicknameInput.text).ToString());
        //}
        //else
        //{
        //    Debug.Log("check NicknameInput");
        //}

        //닉네임 중복 성공 여부 및 현재 닉네임 입력 필드와 중복확인 버튼 누른 후의 입력필드가 같은가?
        if(isSuccessNicknamechecked && (nicknameDuplitionText == SignUp_NicknameInput.text))
        {
            Debug.Log(Backend.BMember.CreateNickname(SignUp_NicknameInput.text).ToString());
            _nowSceneState = TitleSceneState.SignUpNickname;
            MessageBox(); //메세지 박스 출력
        }
        else
        {
            NicknameError_Text.text = "닉네임을 확인하여 주십시오.";
        }
    }

    public void ACreateNickname()
    {
        Debug.Log("-------------ACreateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            SendQueue.Enqueue(Backend.BMember.CreateNickname, NicknameInput.text, isComplete =>
            {
                Debug.Log(isComplete.ToString());
            });
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    // 닉네임 수정
    public void UpdateNickname()
    {
        Debug.Log("-------------UpdateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            Debug.Log(Backend.BMember.UpdateNickname(NicknameInput.text).ToString());
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    public void AUpdateNickname()
    {
        Debug.Log("-------------AUpdateNickname-------------");
        if (InputFieldEmptyCheck(NicknameInput))
        {
            SendQueue.Enqueue(Backend.BMember.UpdateNickname, NicknameInput.text, isComplete =>
            {
                Debug.Log(isComplete.ToString());
            });
        }
        else
        {
            Debug.Log("check NicknameInput");
        }
    }

    // 유저 정보 받아오기 - nickname
    public void GetUserInfo()
    {
        Debug.Log("-------------GetUserInfo-------------");
        BackendReturnObject userinfo = Backend.BMember.GetUserInfo();
        Debug.Log(userinfo);

        //text.text = userinfo.ToString();


        if (userinfo.IsSuccess())
        {
            JsonData Userdata = userinfo.GetReturnValuetoJSON()["row"];
            JsonData nicknameJson = Userdata["nickname"];
            
            // 닉네임 여부를 확인 하는 로직
            if (nicknameJson != null)
            {
                string nick = nicknameJson.ToString();
                Debug.Log("NickName is NOT null which is " + nick);
            }
            else
            {
                Debug.Log("NickName is null");
            }
        }

    }
    public string GetUserNickname()
    {
        JsonData nicknameJson = Backend.BMember.GetUserInfo().GetReturnValuetoJSON()["row"]["nickname"];//닉네임 받아오기

        return (string)nicknameJson;
    }

    public void AGetUserInfo()
    {
        Debug.Log("-------------AGetUserInfo-------------");
        SendQueue.Enqueue(Backend.BMember.GetUserInfo, userinfo =>
        {
            Debug.Log(userinfo.ToString());
            JsonData Userdata = userinfo.GetReturnValuetoJSON()["row"];
            JsonData nicknameJson = Userdata["nickname"];

            // 닉네임 여부를 확인 하는 로직
            if (nicknameJson != null)
            {
                string nick = nicknameJson.ToString();
                Debug.Log("NickName is NOT null which is " + nick);
            }
            else
            {
                Debug.Log("NickName is null");
            }
        });
    }

    // 푸시 토큰 입력
    public void PutDeviceToken()
    {
        Debug.Log("-------------PutDeviceToken-------------");
#if UNITY_ANDROID
        try{
        bro = Backend.Android.PutDeviceToken();
            Debug.Log(bro);
        //text.text = bro.ToString();
        }catch(Exception e){
            Debug.Log(e);
        }
#else
        Debug.Log(Backend.iOS.PutDeviceToken(isDevelopment.iosDev));
#endif
    }

    public void APutDeviceToken()
    {
        Debug.Log("-------------APutDeviceToken-------------");
#if UNITY_ANDROID
        SendQueue.Enqueue(Backend.Android.PutDeviceToken, Backend.Android.GetDeviceToken(), bro =>
        {
            Debug.Log(bro);
        });
#else
        SendQueue.Enqueue(Backend.iOS.PutDeviceToken, isDevelopment.iosDev, bro =>
        {
            Debug.Log(bro);
        });
#endif
    }

    // 푸시 토큰 삭제
    public void DeleteDeviceToken()
    {
        Debug.Log("-------------DeleteDeviceToken-------------");
#if UNITY_ANDROID
        Debug.Log(Backend.Android.DeleteDeviceToken());
#else
        Debug.Log(Backend.iOS.DeleteDeviceToken());
#endif
    }

    public void ADeleteDeviceToken()
    {
        Debug.Log("-------------ADeleteDeviceToken-------------");
#if UNITY_ANDROID
        SendQueue.Enqueue(Backend.Android.DeleteDeviceToken, bro =>
        {
            Debug.Log(bro);
        });
#else
        SendQueue.Enqueue(Backend.iOS.DeleteDeviceToken, bro =>
        {
            Debug.Log(bro);
        });
#endif
    }


    public void IsAccessTokenAlive()
    {
        Debug.Log("-------------IsAccessTokenAlive-------------");
        Debug.Log(Backend.BMember.IsAccessTokenAlive().ToString());
    }


    public void AIsAccessTokenAlive()
    {
        Debug.Log("-------------A IsAccessTokenAlive-------------");
        SendQueue.Enqueue(Backend.BMember.IsAccessTokenAlive, callback =>
        {
            Debug.Log(callback);
        });
    }



    public void UpdatePasswordResetEmail()
    {
        Debug.Log("-------------UpdatePasswordResetEmail-------------");
        bro = Backend.BMember.UpdateCustomEmail(email);
        Debug.Log(bro);
    }

    public void AUpdatePasswordResetEmail()
    {
        Debug.Log("-------------A UpdatePasswordResetEmail-------------");
        SendQueue.Enqueue(Backend.BMember.UpdateCustomEmail, email, callback =>
        {
            Debug.Log(callback);
        });
    }


    public void ResetPassword()
    {
        Debug.Log("-------------ResetPassword-------------");
        if (InputFieldEmptyCheck(idInput))
        {
            bro = Backend.BMember.ResetPassword(idInput.text, email);
            Debug.Log(bro);
        }
        else
        {
            Debug.Log("check IDInput");
        }
    }

    public void AResetPassword()
    {
        Debug.Log("-------------A ResetPassword-------------");
        if (InputFieldEmptyCheck(idInput))
        {
            SendQueue.Enqueue(Backend.BMember.ResetPassword, idInput.text, email, callback =>
            {
                Debug.Log(callback);
            });
        }
        else
        {
            Debug.Log("check IDInput");
        }
    }

    public void UpdatePassword()
    {
        Debug.Log("-------------UpdatePassword-------------");
        if (InputFieldEmptyCheck(PWInput))
        {
            bro = Backend.BMember.UpdatePassword(updatedPW, PWInput.text);
            Debug.Log(bro);
        }
        else
        {
            Debug.Log("check PWInput");
        }

    }

    public void AUpdatePassword()
    {
        Debug.Log("-------------A UpdatePassword-------------");
        if (InputFieldEmptyCheck(PWInput))
        {
            SendQueue.Enqueue(Backend.BMember.UpdatePassword, updatedPW, PWInput.text, callback =>
            {
                Debug.Log(callback);
            });
        }
        else
        {
            Debug.Log("check PWInput");
        }

    }

    public void GuestLogin()
    {
        Debug.Log("-------------GuestLogin-------------");

        bro = Backend.BMember.GuestLogin();
        Debug.Log(bro);
    }
    public void AGuestLogin()
    {
        Debug.Log("-------------A GuestLogin-------------");

        SendQueue.Enqueue(Backend.BMember.GuestLogin, callback =>
        {
            Debug.Log(callback);
        });

    }
    public void GetGuestID()
    {
        Debug.Log("-------------GetGuestID-------------");
        Debug.Log("게스트 아이디 : " + Backend.BMember.GetGuestID());
    }

    public void DeleteGuestInfo()
    {
        Debug.Log("-------------DeleteGuestInfo-------------");
        Backend.BMember.DeleteGuestInfo();
    }
}