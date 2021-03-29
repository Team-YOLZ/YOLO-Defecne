using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textplayerHP;   // 플레이어 체력 텍스트.
    [SerializeField]
    private TextMeshProUGUI textplayerGold; // 플레이어 골드 텍스트.
    [SerializeField]
    private TextMeshProUGUI textWave;       //플레이어 웨이브 텍스트.
    [SerializeField]
    private Text CanBuildGold;               //타워 건설에 필요한 골드 텍스트.
    private PlayerHP PlayerHP;              //플레이어 체력 정보.
    private PlayerGold playerGold;          //플레이어 골드 정보.
    private WaveSystem WaveSystem;          //웨이브 정보.
    private TowerSpawner CanTowerGold;      //타워 건설 필요 골드 정보.

    private void Start()
    {
        PlayerHP = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerHP>();
        playerGold = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerGold>();
        WaveSystem = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<WaveSystem>();
        CanTowerGold = GameObject.FindGameObjectWithTag("TowerSpawner").GetComponent<TowerSpawner>();
    }

    private void Update()
    {
        textplayerHP.text = PlayerHP.CurrentHP + "/" + PlayerHP.MaxHP;
        textplayerGold.text = playerGold.CurrentGold.ToString();
        textWave.text = WaveSystem.CurrentWave.ToString();
        CanBuildGold.text = CanTowerGold.towerBuildGold.ToString();
    }
}
