using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;


public enum EObstacleType
{
    None,
    Trap,
    Portal,       // ��Ż
    BossStageDoor // ���� ���������� ���� ��
}
public class Obstacle : BaseObject
{
    [SerializeField]
    public EObstacleType obstacleType { get; set; } = EObstacleType.None;

    protected bool isPlayerInRange = false;
    protected Player player;

    // Ʈ�� ���� ������?
    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            isPlayerInRange = true;
            player ??= collision.collider.gameObject.GetComponent<Player>();
           
            switch (obstacleType)
            {
                case EObstacleType.Trap:

                    player?.OnHit(new AttackParam(this, !player.LookLeft, 1));
                    break;

            }

        }
    }
    // �׷���?
    protected void OnCollisionExit(Collision collision)
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
                case EObstacleType.Portal:
                    TeleportPlayer();
                    break;
                case EObstacleType.BossStageDoor:
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
    private void SpawnObstacle(EObstacleType spawnObstacleType)
    {
        // Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        // Quaternion spawnRotation = Quaternion.identity;
        // Managers.Object.SpawnObject(, , );
    }
}
