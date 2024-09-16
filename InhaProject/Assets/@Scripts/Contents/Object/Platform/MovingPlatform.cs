using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;
public class MovingPlatform : BasePlatform
{
    public Vector3 moveDirection = Vector3.right; // 이동 방향
    public float moveSpeed = 2f; // 이동 속도 (초당 거리)
    public float moveDistance = 5f; // 이동 거리
    public float waitTimeAtPoints = 1f; // 대기 시간 (초)

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
            // 이동 startPosition -> endPosition
            yield return StartCoroutine(MoveToPosition(startPosition, endPosition));

            // endPosition에 도착 후 대기
            yield return new WaitForSeconds(waitTimeAtPoints);

            // 이동 endPosition -> startPosition
            yield return StartCoroutine(MoveToPosition(endPosition, startPosition));

            // startPosition에 도착 후 대기
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
                // 플랫폼 이동 거리 계산
                Vector3 platformMovement = transform.position - lastPlatformPosition;

                // 플레이어 위치 조정
                player.transform.position += platformMovement;
            }

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 정확히 도착 위치 설정
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