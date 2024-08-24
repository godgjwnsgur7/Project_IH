using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;


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
    public ObstacleType obstacleType { get; set; } = ObstacleType.None;

    protected bool isPlayerInRange = false;
    protected Player player;

    // Ʈ�� ���� ������?
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
    // �׷���?
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInRange = false;

        }
    }
    //�ٵ� Door ���� �ʿ��Ҳ� ������
    

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

    public virtual void TeleportPlayer(Portal otherPortal = default)
    {

        if (player != null)
        {
            player.transform.position = otherPortal != null ? otherPortal.transform.position : GetComponentInChildren<Portal>().connectedPortal.transform.position;
        }



    }

    public virtual void EnterBossStage()
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
