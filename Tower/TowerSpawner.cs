using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _towerPrefab; //리소스매니저 사용 안할 시 사용.
    [SerializeField]
    private EnemySpawner _enemySpawner; //현재 맵에 존재하는 적 리스트 정보 얻기위해.

    Transform Root;
    public void SpawnTower(Transform tileTransform)
    {
        Tile tile = tileTransform.GetComponent<Tile>();

        //우선 타워건설 가능한 타일인지 판단.
        if(tile._isBuildTower== true)
        {
            return; //이미 타워가 있는곳이면 생성되지 않고 return
        }

        //선택한 타일에 타워 생성.
        tile._isBuildTower = true;

        //선택한 타일의 위치에 타워 오브젝트 생성.
        GameObject go = Instantiate(_towerPrefab, tileTransform.transform.position,Quaternion.identity); // 리소스매니저 사용 x
        go.name = "@Tower01";
        if (Root == null)
        {
            Root = new GameObject().transform;
            Root.name = $"{go.name}_Group";
        }
        go.transform.parent = Root.transform;
        //타워 무기에 enemyspawner List 전달.
        go.GetComponent<TowerWeapon>().SetUp(_enemySpawner); 
    }
}
