
public class SkillData
{
    public int SkillId;
    public int SlotId;

    public float DamagePoint;
    public float Damage;
    public float AttackRange;
    public string AnimationName;

    public ActPriority Priority;
}

public enum ActPriority
{
    None = 0,
    NormalAttack,
    Skill,
    Move,

    Max
}
