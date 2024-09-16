using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
public class MovingPlatform : BasePlatform
{
    public Vector3 moveDirection = Vector3.right; // �̵� ����
    public float moveSpeed = 2f; // �̵� �ӵ� (�ʴ� �Ÿ�)
    public float moveDistance = 5f; // �̵� �Ÿ�
    public float waitTimeAtPoints = 1f; // ��� �ð� (��)

    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 lastPlatformPosition;

    public override bool Init()
    {
        if (!base.Init())
            return false;

        startPosition = transform.position;
        endPosition = startPosition + moveDirection.normalized * moveDistance;
        lastPlatformPosition = startPosition;
        StartCoroutine(MovePlatform());
        return true;
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            // �̵� startPosition -> endPosition
            yield return StartCoroutine(MoveToPosition(startPosition, endPosition));

            // endPosition�� ���� �� ���
            yield return new WaitForSeconds(waitTimeAtPoints);

            // �̵� endPosition -> startPosition
            yield return StartCoroutine(MoveToPosition(endPosition, startPosition));

            // startPosition�� ���� �� ���
            yield return new WaitForSeconds(waitTimeAtPoints);
        }
    }

    private IEnumerator MoveToPosition(Vector3 from, Vector3 to)
    {
        float elapsedTime = 0f;
        float journeyLength = Vector3.Distance(from, to);

        while (elapsedTime < journeyLength / moveSpeed)
        {
            float distanceCovered = (elapsedTime / (journeyLength / moveSpeed)) * journeyLength;
            float fractionOfJourney = distanceCovered / journeyLength;
            transform.position = Vector3.Lerp(from, to, fractionOfJourney);

            if (player != null)
            {
                // �÷��� �̵� �Ÿ� ���
                Vector3 platformMovement = transform.position - lastPlatformPosition;

                // �÷��̾� ��ġ ����
                player.transform.position += platformMovement;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ��Ȯ�� ���� ��ġ ����
        transform.position = to;
        lastPlatformPosition = to;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
       base.OnCollisionEnter(collision);
    }


    protected override void OnCollisionExit(Collision collision)
    {
        base.OnCollisionExit(collision);
        if (player != null)
        {
            player = null;
        }

    }
}