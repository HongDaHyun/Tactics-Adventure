using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Monster : Card
{
    // 변수
    public int hp;

    // 자식 컴포넌트
    private Monster monster;

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
        // 몬스터 소환
        monster = spawnManager.SpawnRanMonster(objTrans);

        // 변수 설정
        hp = monster.data.hp;

        SetCardName(monster.data.name);
        SetUI($"<sprite=1>{hp}");
    }

    public override void DestroyCard()
    {
        spawnManager.DeSpawnMonster(monster);
    }

    public override void DoCard()
    {
        spawnManager.playerCard.Atk(this);
    }

    public override void Damaged(int _amount)
    {
        hp -= _amount;

        if (hp <= 0)
            Die();

        SetUI($"<sprite=1>{hp}");
    }

    private void Die()
    {
        // 변수 설정
        hp = 0;

        spawnManager.playerCard.Move(pos); // 플레이어 이동
        spawnManager.DeSpawnCard(this); // 카드 삭제
    }
}
