using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Trap : Card
{
    private Trap trap;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
    }

    public override void OnGettingFromPool()
    {
        base.OnGettingFromPool();
    }

    public override void SetCard()
    {
        trap = spawnManager.SpawnRanTrap(childTrans[0]);
    }

    public override void DestroyCard()
    {
        spawnManager.DeSpawnTrap(trap);
    }
}
