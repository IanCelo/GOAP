using UnityEngine;


namespace GOAP_Planner
{

public class CharacterStateManager : MonoBehaviour
{
	[SerializeField] private StateKey Wood;
    [SerializeField] private StateKey Iron;
    [SerializeField] private StateKey Weapon;
    [SerializeField] private StateKey Axe;
    [SerializeField] private StateKey Pickaxe;

	internal void UpdateWorldStates(Character _Player, ResourceType _Type, int _Tool)
	{
		_Player.ChangeState(Wood, _Type == ResourceType.WOOD);
		_Player.ChangeState(Iron, _Type == ResourceType.IRON);
		_Player.ChangeState(Weapon, _Type == ResourceType.WEAPON);
		_Player.ChangeState(Axe, _Tool == 1);
		_Player.ChangeState(Pickaxe, _Tool == 2);
	}
}

}
