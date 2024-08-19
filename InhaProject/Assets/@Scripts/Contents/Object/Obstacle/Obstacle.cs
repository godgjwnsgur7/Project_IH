using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� ��� X
public enum ObstacleType
{
    None,
    GroundTrap,// 
}

public class Obstacle : BaseObject
{
    //�ʿ��ϸ� ��ֹ��� �̺�Ʈ �߰��� ���� ����
    [SerializeField]
    private ObstacleType obstacleType { get;  set; } = ObstacleType.None;

    //�浹�� ( hit��)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            // �÷��̾� hit 
            Player player = other.gameObject.GetComponent<Player>();

            player?.OnHit(new AttackParam(!player.LookLeft,1)); // �̷����ΰ�?
          
        }
    }
    
    public override bool Init()
    {
        if (!base.Init())
            return false;


        return true;
    }

    //���� �ʿ��ϸ� ���
    private void DestroyObstacle()
    {
        Debug.Log("Destroy");
        // �ڽ��� �ı�
        Managers.Object.DespawnObject(gameObject);

    }

    //���� �ʿ��ϸ� ���
    private void SpawnObstacle(ObstacleType spawnObstacleType)
    {
        //Vector3 spawnPosition = transform.position; // ���� ������ ��ġ�� ����
        //Quaternion spawnRotation = Quaternion.identity;
        //Managers.Object.SpawnObject(, , );

    }
}
