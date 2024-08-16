using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Item2 : BaseItem
{
    protected override void Start()
    {
        base.Start();
        // Item2의 추가적인 초기화 작업
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 충돌한 객체가 Player 태그를 가진 경우
        {
            Debug.Log("충돌 체크");
            if (Input.GetKey(KeyCode.Z)) //임시 사용
            {
                Debug.Log("z체크");
                ItemState = EItemState.Use; // 상태를 Use로 변경
                /*
                 상호작용 함수
                 */
                DestroyItem();
            } 
        }
    }
    protected override bool UseStateCondition()
    {
        // Item2의 Use 상태 조건
        return base.UseStateCondition();
    }

    protected override void UseStateEnter()
    {
        // Item2의 Use 상태 진입 시 작업
        base.UseStateEnter();
    }

    protected override void UsedStateEnter()
    {
        // Item2의 Used 상태 진입 시 작업
        base.UsedStateEnter();
    }
}