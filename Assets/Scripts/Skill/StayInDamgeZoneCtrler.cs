using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StayInDamgeZoneCtrler : Single<StayInDamgeZoneCtrler>
{
    private Dictionary<int, StayInDamageZone> mStayInZones = new Dictionary<int,StayInDamageZone>();
    private GameObject mStayInZoneRoot;
	
    public void PlaceZone(int skillId, BaseEntity owner, Vector2 position, Vector2 zoneSize, float persistime, int damge)
    {
        if (null == mStayInZoneRoot)
            CreateRoot();
        StayInDamageZone zone = GetStayInZone(skillId);
        if (null == zone)
        {
            zone = CreateStayInZone(skillId, owner.transform);
            mStayInZones.Add(skillId, zone);
        }
        zone.SetSize(zoneSize);
        zone.Place(owner, position, persistime, damge);
    }

    public void CancelPlaceZone(int skillId)
    {
        Util.LogAsvo("cancel zone : " + skillId);
        StayInDamageZone zone = GetStayInZone(skillId);
        if (null != zone)
        {
            zone.RemoveZoneFromOutter();
        }
    }

    private void CreateRoot()
    {
        mStayInZoneRoot = new GameObject("StayInZoneRoot");
    }

    private StayInDamageZone CreateStayInZone(int skillId, Transform ownerTrans)
    {
        GameObject stayZoneGobj = new GameObject(skillId.ToString());
        stayZoneGobj.layer = Util.DamageZoneLayer;
        StayInDamageZoneType zoneType = GetStayInTypeBySkill(skillId);
        if (zoneType == StayInDamageZoneType.StayStatic)
            stayZoneGobj.transform.SetParent(mStayInZoneRoot.transform);
        else if (zoneType == StayInDamageZoneType.StayFollowOwner)
            stayZoneGobj.transform.SetParent(ownerTrans);
        stayZoneGobj.transform.localScale = Vector2.one;
        //add collider
        BoxCollider2D collider = stayZoneGobj.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1, 1);
        StayInDamageZone zoneSc = stayZoneGobj.AddComponent<StayInDamageZone>();
        return zoneSc;
    }

    private StayInDamageZoneType GetStayInTypeBySkill(int skillId)
    {
        if (skillId == SkillConst.PlayerSkill01SlotId || skillId == SkillConst.PlayerSkill02SlotId)
            return StayInDamageZoneType.StayFollowOwner;
        return StayInDamageZoneType.StayStatic;
    }

    private StayInDamageZone GetStayInZone(int skillId)
    {
        StayInDamageZone zone = null;
        if (null != mStayInZones)
            mStayInZones.TryGetValue(skillId, out zone);
        return zone;
    }
}

public enum StayInDamageZoneType
{
    StayStatic,
    StayFollowOwner,
}
