using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class TowerData : MonoBehaviour
{
    //[SerializeField]
    //private Button buttonUpgrade;
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private Button buttonUpgrade;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
        //if (!PhotonNetwork.IsMasterClient) gameObject.tag = "MyInfo(Client)";
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerweapon)
    {
        //출력해야하는 타워 정보 받아와 저장.
        currentTower = towerweapon.GetComponent<TowerWeapon>();

        gameObject.transform.GetChild(0).gameObject.SetActive(true);
        gameObject.transform.GetChild(1).gameObject.SetActive(true);
        gameObject.transform.GetChild(2).gameObject.SetActive(true);

        //타워 정보 갱신
        UpdateTowerData();
    }
    public void OffPanel()
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        gameObject.transform.GetChild(2).gameObject.SetActive(false);
    }
    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.Towersprite;
        textLevel.text = "lv" + currentTower.Level;
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }
    public void OnClickEventTowerUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();

        if(isSuccess == true)
        {
            //타워정보 갱신.
            UpdateTowerData();
        }
        else
        {
            //타워 업그레이드에 필요한 비용이 부족한건데 어떻게 할건지 여기 입력.
        }
    }

}
