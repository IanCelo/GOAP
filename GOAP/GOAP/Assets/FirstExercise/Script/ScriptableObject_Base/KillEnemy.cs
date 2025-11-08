using UnityEngine;


namespace GOAP_PlannerExo
{

[CreateAssetMenu(menuName = "Scriptable/ActionsExo/KillEnemy", fileName = "KillEnemyScriptable")]
public class KillEnemy : Action
{
    private         int m_RealCost;

    public override int GetCost(States _WorldState = null)
    {
        if (_WorldState is null)
            return Cost;

        return Cost * (_WorldState.GetAsDictionary()[StateMemberKey.HAS_WEAPON] ? 1 : 5);
    }

    public override bool Act(ref States _WorldState)
    {
        if (!IsValid(_WorldState))
            return false;

        ApplyWorldChanges(ref _WorldState);

        return true;
    }
}

}