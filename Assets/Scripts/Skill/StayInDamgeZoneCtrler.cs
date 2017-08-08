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
            zone = CreateStayInZone(skillId);
            mStayInZones.Add(skillId, zone);
        }
        zone.SetSize(zoneSize);
        zone.Place(owner, position, persistime, damge);
    }

    private void CreateRoot()
    {
        mStayInZoneRoot = new GameObject("StayInZoneRoot");
    }

    private StayInDamageZone CreateStayInZone(int skillId)
    {
        GameObject stayZoneGobj = new GameObject(skillId.ToString());
        stayZoneGobj.layer = Util.DamageZoneLayer;
        stayZoneGobj.transform.SetParent(mStayInZoneRoot.transform);
        stayZoneGobj.transform.localScale = Vector2.one;
        //add collider
        BoxCollider2D collider = stayZoneGobj.AddComponent<BoxCollider2D>();
        collider.size = new Vector2(1, 1);
        StayInDamageZone zoneSc = stayZoneGobj.AddComponent<StayInDamageZone>();
        return zoneSc;
    }

    private StayInDamageZone GetStayInZone(int skillId)
    {
        StayInDamageZone zone = null;
        mStayInZones.TryGetValue(skillId, out zone);
        return zone;
    }
}
