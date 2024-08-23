using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType
{
    None,
    GroundTrap,
    Portal,       // 포탈
    BossStageDoor // 보스 스테이지로 가는 문
}
public class Obstacle : BaseObject
{
    [SerializeField]
    private ObstacleType obstacleType { get; set; } = ObstacleType.None;

    private bool isPlayerInRange = false;
    private Player player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player = other.gameObject.GetComponent<Player>();

            if (obstacleType == ObstacleType.GroundTrap)
            {
                // 플레이어 hit 
                player?.OnHit(new AttackParam(!player.LookLeft, 1)); // 이런식인가?
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            player = null;
        }
    }

    public override bool Init()
    {
        if (!base.Init())
            return false;

        // InputMgr에서 위 방향키 입력을 받아 이벤트를 추가
        Managers.Input.OnArrowKeyEntered += OnArrowKeyEntered;

        return true;
    }

    private void OnArrowKeyEntered(Vector2 inputVec)
    {
        if (isPlayerInRange && inputVec.y > 0) // 위 방향키가 눌렸을 때
        {
            switch (obstacleType)
            {
                case ObstacleType.Portal:
                    TeleportPlayer();
                    break;
                case ObstacleType.BossStageDoor:
                    EnterBossStage();
                    break;
            }
        }
    }

    private void TeleportPlayer()
    {
        // 플레이어를 포탈 위치로 이동시키는 로직
        //  player.transform.position = destinationPosition; 이런거?
    }

    private void EnterBossStage()
    {
        // 플레이어가 보스 스테이지로 이동하는 로직


    }


    // 디스폰 필요하면 사용
    private void DestroyObstacle()
    {
        Debug.Log("Destroy");
        // 자신을 파괴
        Managers.Object.DespawnObject(gameObject);
    }

    // 스폰 필요하면 사용
    private void SpawnObstacle(ObstacleType spawnObstacleType)
    {
        // Vector3 spawnPosition = transform.position; // 현재 아이템 위치에 생성
        // Quaternion spawnRotation = Quaternion.identity;
        // Managers.Object.SpawnObject(, , );
    }
}
