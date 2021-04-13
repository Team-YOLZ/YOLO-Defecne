//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//public class TowerDragHandler : MonoBehaviour
//{
//    private float startPosX;
//    private float startPosY;
//    private bool isBeginHeld = false;

//    private Vector2 StartPos;

//    private void Update()
//    {
//        if (Input.GetMouseButton(0))
//        {
//            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//            RaycastHit2D rayhit = Physics2D.Raycast(mousePos, Vector2.zero);

//            if (rayhit.collider.tag !=null )
//            {
//                Vector2 StartPos = rayhit.collider.gameObject.transform.GetChild(1).transform.position;
//                rayhit.collider.gameObject.transform.GetChild(1).transform.position = mousePos;
//                Debug.Log("Down" + rayhit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name);

//                if (Input.GetMouseButtonUp(0))
//                {
//                    if (rayhit.collider.tag == "Tower")
//                    {
//                        Debug.Log("UP" + rayhit.collider.gameObject.GetComponent<SpriteRenderer>().sprite.name);
//                    }
//                    else rayhit.collider.gameObject.transform.GetChild(1).transform.position = StartPos;
//                }
//            }
//        }
//    }
//}
