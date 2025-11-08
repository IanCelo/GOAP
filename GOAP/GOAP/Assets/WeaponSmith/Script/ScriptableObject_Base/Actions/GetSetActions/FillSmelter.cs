using UnityEngine;


namespace GOAP_Planner
{

[CreateAssetMenu(menuName = "Scriptable/Actions/FillSmelter", fileName = "FillSmelterScriptable")]
public class FillSmelter : Action
{
	private Storage m_Storage;
	public override bool FinishActing(ref States _WorldStates)
	{
		if (!base.FinishActing(ref _WorldStates)) return false;
		
		PutInStorage(Storage);

		return true;
	}

	public override void SetStorage()
	{
		Storage = WorldManager.Instance.Smelter;

	}
}

}

