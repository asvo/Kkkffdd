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
    private BoxCollider2D mBoxColider;

    public void SetSize(Vector2 size)
    {
        mBoxColider = GetComponent<BoxCollider2D>();
        if (null != mBoxColider)
        {
            mBoxColider.offset = new Vector2(0, 1f);    //这里采用角色的偏移。。
            mBoxColider.size = size;
            mBoxColider.isTrigger = true;
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

    public void OnTriggerEnter2D(Collider2D collider)
    {
        OnTrigColider(collider);
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        OnTrigColider(collider);
    }

    private void OnTrigColider(Collider2D collider)
    {
        if (mDamagedEntites == null || collider.gameObject == null)
        {
            return;
        }
        if (collider.gameObject.layer == Util.PlayerLayer)
            return;
    //    Debug.Log("coliider to " + collider.gameObject.name);
        if (!mIsActive)
            return;
        BaseEntity entity = collider.gameObject.GetComponent<BaseEntity>();
        if (null == entity)
            return;
        //todo, 如果是目标对象
        //这里简单认为是怪物就是目标
        if (entity is Monster)
        {
            if (mDamagedEntites.Contains(entity)) //（一块区域内伤害对象一次）
                return;
            if (mDamagedEntites.Count == 0) //if hit any enemy. once hit only once trigger OnHitAntEnemy.
                OnHitAnyEnemy();
            mDamagedEntites.Add(entity);
            DamagerHandler.Instance().CalculateDamage(mOwner, entity, mDamage);
        }
    }

    private void OnHitAnyEnemy()
    {
        SkillDataMgr.Instance().ReducePlayerAllSkillCd(1.0f);
    }

    private void Clear()
    {
        mDamagedEntites = new List<BaseEntity>();
        mIsActive = true;
        gameObject.SetActive(mIsActive);
        mBoxColider.isTrigger = true;
    }

    private void OnOver()
    {
        mIsActive = false;
        mBoxColider.isTrigger = false;
        gameObject.SetActive(mIsActive);
        mDamagedEntites.Clear();
    }
}
