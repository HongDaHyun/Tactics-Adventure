using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Card_Relics : Card
{
    private RelicManager relicManager;
    private Relic relic;

    public override void OnCreatedInPool()
    {
        base.OnCreatedInPool();
        relicManager = RelicManager.Instance;
    }

    public override void SetCard()
    {
        relic = spawnManager.SpawnRelic_Ran(objTrans); // ���� ��ȯ

        SetCardName(relic.data.name);
        SetUI(relic.data.explanation);
    }

    public override void DestroyCard()
    {
        spawnManager.DeSpawnRelic(relic);
        DODestroy();
    }

    public override IEnumerator DoCard()
    {
        yield return CollectRelic();
        yield return spawnManager.playerCard.Move(pos);
    }

    public override IEnumerator Damaged(int _amount)
    {
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator CollectRelic()
    {
        yield return relicManager.AddRelicList(relic);
        spawnManager.SpawnRelicIcon(relic.data.index);
    }
}