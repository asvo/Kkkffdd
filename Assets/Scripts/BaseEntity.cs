using UnityEngine;
using System.Collections;

public class BaseEntity : MonoBehaviour {

    public float InitMoveSpeed = 2f;
    public float MaxMoveSpeed = 10f;

    public virtual void Move(MoveDir moveDir) { }

    public virtual void EndMove() { }
}
