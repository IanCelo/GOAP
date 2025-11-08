using GOAP_Planner;
using UnityEngine;


public class WorldManager : Singleton<WorldManager>
{
    public States WorldStates;

    [SerializeField] private  Goal FillWoodStock;
    [SerializeField] private  Goal FillIronStock;
    [SerializeField] private  Goal FillCutter;
    [SerializeField] private  Goal FillSmelter;
    [SerializeField] private  Goal SellWeapons;
    
    public Storage WoodStock;
    public Storage IronStock;
    public Storage Cutter;
    public Storage Smelter;
    public Storage Output;

    [SerializeField] private float TimeToCraftUnitWeapon;

    private bool  m_ShouldCraftWeapon;
    private float m_UpdateTimer;

    // Update is called once per frame
    private void Update()
    {
        m_UpdateTimer += Time.deltaTime;
        
        if (m_UpdateTimer - Mathf.FloorToInt(m_UpdateTimer) <= Time.deltaTime)
            return;
        
        SetPriorityForStorageGoal(WoodStock, FillWoodStock);
        SetPriorityForStorageGoal(IronStock, FillIronStock);
        SetPriorityForStorageGoal(Cutter, FillCutter);
        SetPriorityForStorageGoal(Smelter, FillSmelter);
        SetPriorityForStorageGoal(Output, SellWeapons);
        
        if (m_UpdateTimer < TimeToCraftUnitWeapon)
            return;
        
        m_UpdateTimer = 0f;

        CraftWeapon();
    }

    private void CraftWeapon()
    {
        if (Smelter.Empty || Cutter.Empty || Output.Full)
            return;
        
        Smelter.Take(1);
        Cutter.Take(1);
        Output.PutIn(m_ShouldCraftWeapon ? 1 : 0);
        
        m_ShouldCraftWeapon = !m_ShouldCraftWeapon;
    }

    private void SetPriorityForStorageGoal(Storage _Storage, Goal _LinkedGoal)
    {
        _LinkedGoal.Priority = _Storage.GetFillPriority(WorldStates);
    }

    private void OnValidate()
    {
        WorldStates.Update();
    }
}
