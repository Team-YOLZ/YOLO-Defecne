using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TowerDragHandler : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private bool isBeginHeld = false;

    private void Update()
    {
        if(isBeginHeld == true )
        {
            Vector3 mousepos;
            mousepos = Input.mousePosition;
            mousepos = Camera.main.ScreenToWorldPoint(mousepos);

            this.gameObject.transform.localPosition = new Vector3(mousepos.x - startPosX, mousepos.y - startPosY, 0);
        }
    }

    public void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(gameObject.name);
            Vector3 mousepos;
            mousepos = Input.mousePosition;
            mousepos = Camera.main.ScreenToWorldPoint(mousepos);

            startPosX = mousepos.x - this.transform.localPosition.x;
            startPosY = mousepos.y - this.transform.localPosition.y;

            isBeginHeld = true;
        }

    }
    public void OnMouseUp()
    {
        isBeginHeld = false;
    }
}
