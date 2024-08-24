using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;


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
    public ObstacleType obstacleType { get; set; } = ObstacleType.None;

    protected bool isPlayerInRange = false;
    protected Player player;

    // 트랩 빼서 나눌까?
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player ??= collision.collider.gameObject.GetComponent<Player>();
            player?.OnHit(new AttackParam(!player.LookLeft, 1));
            switch (obstacleType)
            {
                case ObstacleType.GroundTrap:


                    break;

            }

        }
    }
    // 그럴까?
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInRange = false;

        }
    }
    //근데 Door 에서 필요할꺼 같은데
    

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

    public virtual void TeleportPlayer(Portal otherPortal = default)
    {

        if (player != null)
        {
            player.transform.position = otherPortal != null ? otherPortal.transform.position : GetComponentInChildren<Portal>().connectedPortal.transform.position;
        }



    }

    public virtual void EnterBossStage()
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
