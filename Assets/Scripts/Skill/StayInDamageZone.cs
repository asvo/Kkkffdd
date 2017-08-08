using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 伤害区域放置物。 （一块区域内伤害对象一次）
/// </summary>
public class StayInDamageZone : MonoBehaviour {

    private bool mIsActive;
    private List<BaseEntity> mDamagedEntites;
    private BaseEntity mOwner;

    private float mPersistTime;
    private int mDamage;

    public void SetSize(Vector2 size)
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (null != boxCollider)
        {
            boxCollider.offset = new Vector2(0, 1f);    //这里采用角色的偏移。。
            boxCollider.size = size;
        }
    }

    public void Place(BaseEntity owner, Vector2 position, float persistTime, int damge)
    {
        mOwner = owner;
        mPersistTime = persistTime;
        mDamage = damge;
        Clear();
        transform.position = position;
        Invoke("OnDisplace", mPersistTime);
    }

    private void OnDisplace()
    {
        OnOver();
    }

    public void RemoveZoneFromOutter()
    {
        if (mIsActive)
        {
            CancelInvoke("OnDisplace");
        }
        OnOver();
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if (!mIsActive)
            return;
        if (mDamagedEntites == null || collider.gameObject == null)
        {
            return;
        }
        BaseEntity entity = collider.gameObject.GetComponent<BaseEntity>();
        if (null == entity)
            return;
        //todo, 如果是目标对象
        //这里简单认为是怪物就是目标
        if (entity is Monster)
        {
            if (mDamagedEntites.Contains(entity)) //（一块区域内伤害对象一次）
                return;
            mDamagedEntites.Add(entity);
            DamagerHandler.Instance().CalculateDamage(mOwner, entity, mDamage);
        }
    }

    private void Clear()
    {
        mDamagedEntites = new List<BaseEntity>();
        mIsActive = true;
        gameObject.SetActive(mIsActive);
    }

    private void OnOver()
    {
        mIsActive = false;
        gameObject.SetActive(mIsActive);
        //
    }
}
