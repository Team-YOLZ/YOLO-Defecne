using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner _towerSpawner;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;

    private void Awake()
    {
        //mainCamera 할당
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //ray  (origin : maincamera , direction : 클릭된 마우스 좌표)
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //raycast : ray에 검출된 object return 
            if(Physics.Raycast(ray,out hit, Mathf.Infinity)) // 일단 무한대로 쏨.
            {
                if(hit.transform.CompareTag("Tile"))
                {
                    _towerSpawner.SpawnTower(hit.transform);
                }
            }

        }
    }
}
