using System;
using System.Numerics;

public class AttackParam
{
    public BaseObject attacker;
    public bool isAttackerLeft;
    public float damage;
    public float pushPower;

    public AttackParam(BaseObject attacker, bool isAttackerLeft, float damage = 0, float pushPower = 3.5f)
    {
        this.attacker = attacker;
        this.isAttackerLeft = isAttackerLeft;
        this.damage = damage;
        this.pushPower = pushPower;
    }
}
