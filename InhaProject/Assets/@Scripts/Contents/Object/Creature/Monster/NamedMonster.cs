using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NamedMonsterData
{

}

public enum ENamedMonsterState
{
    None,

    Dead,
}

public enum ENamedMonsterType
{
    NamedSkeletonWizard = 0,
    Max,
}

public class NamedMonster : MonoBehaviour
{
    [field: SerializeField, ReadOnly] public ENamedMonsterType NamedMonsterType { get; protected set; }
    [field: SerializeField, ReadOnly] public NamedMonsterData MonsterInfo { get; protected set; }

    [SerializeField, ReadOnly]
    private ENamedMonsterState _monsterState;
    public virtual ENamedMonsterState MonsterState
    {
        get { return _monsterState; }
        protected set
        {
            if (_monsterState == ENamedMonsterState.Dead)
                return;

            if (_monsterState == value)
                return;

            bool isChangeState = true;

            // TODO ม๘วเ ม฿
        }
    }
}
