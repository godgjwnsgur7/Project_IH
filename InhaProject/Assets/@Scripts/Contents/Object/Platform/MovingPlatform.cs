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
        float moveDuration = journeyLength / moveSpeed;

        while (elapsedTime < moveDuration)
        {
            float fractionOfJourney = elapsedTime / moveDuration;
            lastPlatformPosition = transform.position;
            transform.position = Vector3.Lerp(from, to, fractionOfJourney);

            if (player != null)
            {
                Vector3 platformMovement = transform.position - lastPlatformPosition;

                player.AddPosition(platformMovement);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }


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