using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObstacleType
{
    None,
    GroundTrap,
    Portal,       // ��Ż
    BossStageDoor // ���� ���������� ���� ��
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
                // �÷��̾� hit 
                player?.OnHit(new AttackParam(!player.LookLeft, 1)); // �̷����ΰ�?
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

        // InputMgr���� �� ����Ű �Է��� �޾� �̺�Ʈ�� �߰�
        Managers.Input.OnArrowKeyEntered += OnArrowKeyEntered;

        return true;
    }

    private void OnArrowKeyEntered(Vector2 inputVec)
    {
        if (isPlayerInRange && inputVec.y > 0) // �� ����Ű�� ������ ��
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
        // �÷��̾ ��Ż ��ġ�� �̵���Ű�� ����
        //  player.transform.position = destinationPosition; �̷���?
    }

    private void EnterBossStage()
    {
        // �÷��̾ ���� ���������� �̵��ϴ� ����


    }


    // ���� �ʿ��ϸ� ���
    private void DestroyObstacle()
    {
        Debug.Log("Destroy");
        // �ڽ��� �ı�
        Managers.Object.DespawnObject(gameObject);
    }

    // ���� �ʿ��ϸ� ���
    private void SpawnObstacle(ObstacleType spawnObstacleType)
    {
        // Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        // Quaternion spawnRotation = Quaternion.identity;
        // Managers.Object.SpawnObject(, , );
    }
}
