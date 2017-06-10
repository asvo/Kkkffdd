
using System.Collections.Generic;

public class SkillCaster : Single<SkillCaster> 
{
    
    public void CastSkill(SkillData skilldata, BaseEntity from)
    {
        List<BaseEntity> targetList = FindTargets(skilldata, from);

        for(int i = 0; i < targetList.Count; ++i)
        {

        }
    }

    private List<BaseEntity> FindTargets(SkillData skilldata, BaseEntity from)
    {
        //TODO;
        return new List<BaseEntity>();
    }
}
