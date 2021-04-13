//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class UpgradeBtnCtrl : MonoBehaviour
//{
//    Image image;
//    private string Btnname;
//    private int count = 0;
//    private GameObject[] Dack;
//    private string[] Dackname;
//    [SerializeField]
//    private PlayerGold playerGold;
//    // Start is called before the first frame update
//    void Start()
//    {
//        image = GetComponent<Image>();
//    }

//    // Update is called once per frame
//    void FixedUpdate()
//    {
//        Btnname = image.sprite.name;
//        Dack = GameObject.FindGameObjectsWithTag("Tower");
//        for(int i=0; i<Dack.Length; i++)
//        {
//            if(Dack[i].GetComponent<SpriteRenderer>().sprite.name== Btnname)
//            {
//                count++;
//            }
//            if (count >= 2)
//            {
//                StopCoroutine(HitAlphaAnimation());
//                StartCoroutine(HitAlphaAnimation());
//            }
//            if(i == Dack.Length-1)
//            {
//                count = 0;
//            }
//        }
//    }
//    private IEnumerator HitAlphaAnimation()
//    {
//        //현재 적의 색상.
//        Color color = image.color;

//        //적의 투명도 40퍼센트.
//        color.a = 0.5f;
//        image.color = color;

//        //0.05초 대기
//        yield return new WaitForSeconds(0.1f);

//        //적의 투명도 100프로 설정.
//        color.a = 1.0f;
//        image.color = color;
//    }
//}
