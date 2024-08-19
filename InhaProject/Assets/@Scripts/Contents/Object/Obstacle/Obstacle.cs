using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//현재 사용 X
public enum ObstacleType
{
    None,
    GroundTrap,// 
}

public class Obstacle : BaseObject
{
    //필요하면 장애물별 이벤트 추가를 위한 변수
    [SerializeField]
    private ObstacleType obstacleType { get;  set; } = ObstacleType.None;

    //충돌시 ( hit시)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // 플레이어 hit 
            Player player = other.gameObject.GetComponent<Player>();

            player?.OnHit(new AttackParam(!player.LookLeft,1)); // 이런식인가?
          
        }
    }
    
    public override bool Init()
    {
        if (!base.Init())
            return false;


        return true;
    }

    //디스폰 필요하면 사용
    private void DestroyObstacle()
    {
        Debug.Log("Destroy");
        // 자신을 파괴
        Managers.Object.DespawnObject(gameObject);

    }

    //스폰 필요하면 사용
    private void SpawnObstacle(ObstacleType spawnObstacleType)
    {
        //Vector3 spawnPosition = transform.position; // 현재 아이템 위치에 생성
        //Quaternion spawnRotation = Quaternion.identity;
        //Managers.Object.SpawnObject(, , );

    }
}
