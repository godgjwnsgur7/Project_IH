using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class A : MonoBehaviour
{
    // 캐릭터 스테이더스 창을 관리하는 클래스
    // Hp, Mp, 실시간으로 변동이 되는 IngameUI들을 관리하는 컨트롤러 느낌?
    // 얘는 플레이어가 인게임에 스폰될 때 참조하기 시작할 것.

    // 플레이어에게 계속 갱신될 것.
    // 1) 플레이어에게 특정 데이터 클래스로 묶인 필요한 데이터들 일괄적으로 읽는 방법
    //    -> 지금 해당 클래스가 플레이어를 참조하는 방식

    // 2) 플레이어가 쏴주는 정보를 계속 받아서 갱신하는 방법
    //    -> 플레이어가 얘를 참조하는 방식

    // 3) 둘다 참조를 안하고 델리게이트를 활용해서 등록한 후 서로의 행동을 몰라도 되게 하는 방법.
    // -> 결국 이 클래스에서 하는 일은. 뷰를 보여주는 것.
    // ex) 쿨타임, 등등

    // 등등 많으니까 고민해서 만드셈ㅋ
}

public enum ESkillSlotType
{
    Skill1,
    Skill2,
    Skill3,
}

public class SkillSlot : MonoBehaviour
{
    

}
