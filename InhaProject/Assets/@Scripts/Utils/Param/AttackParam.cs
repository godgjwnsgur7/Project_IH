using System;

public class AttackParam
{
    public bool isAttackerLeft;
    public float damage;
    public float pushPower;

    public AttackParam(bool isAttackerLeft, float damage = 0, float pushPower = 3.5f)
    {
        this.isAttackerLeft = isAttackerLeft;
        this.damage = damage;
        this.pushPower = pushPower;
    }
}
