using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public struct PlayerData
{
    public string name;
    public PlayerType type;
    public int hp, mp, exp, defend;
    public int lv;
    public int skillMP, passiveCount;
}

[Serializable]
public struct ChestData
{
    public string name;
    public ChestType type;
}

[Serializable]
public struct MonsterData
{
    public string name;
    public MonsterType type;
    public Stage stage;
}

[Serializable]
public struct RelicData
{
    public string name, explanation;
    public Tier tier;
    public int index;
}

[Serializable]
public struct TrapData
{
    public string name;
    public TrapType type;
    public Direction[] targetDir;
}

[Serializable]
public struct WeaponData
{
    public string name;
    public WeaponType type;
    public Tier tier;
    public WeaponAttribute attribute;
    public WeaponPlus plus;
    public int index;
}
[Serializable]
public struct WeaponPlus
{
    public int dmg;
    public bool[] enforce; // 0: Drain
}

[Serializable]
public struct ExplainData
{
    public CardType type;
    public string[] explains;
}

[Serializable]
public struct TierColor
{
    public Tier tier;
    public Color color;
}

[Serializable]
public struct BuffIconData
{
    public BuffIconType type;
    public Sprite sprite;
}

[Serializable]
public class CSVList
{
    public RelicData[] relicDatas;
    public WeaponData[] weaponDatas;
    public MonsterData[] monsterDatas;
    public TrapData[] trapDatas;
    public ExplainData[] explainDatas;
    public TierColor[] tierColors;
    public BuffIconData[] buffIconDatas;

    #region Relic
    public RelicData FindRelic(int index)
    {
        return relicDatas[index];
    }

    public ref RelicData ExportRelic(RelicData data)
    {
        return ref relicDatas[data.index];
    }
    #endregion

    #region Weapon
    public WeaponData FindWeapon(WeaponType _type, Tier _tier)
    {
        // 조건 맞춤 무기 리스트
        List<WeaponData> validWeapons = new List<WeaponData>();

        // 무기 데이터 배열 순환
        foreach(WeaponData data in weaponDatas)
        {
            // 타입과 티어가 일치하면 조건 맞춤 무기 리스트에 추가한다.
            if (data.type == _type && data.tier == _tier)
                validWeapons.Add(data);
        }

        // 리스트 원소 개수가 0 이상이면
        if (validWeapons.Count > 0)
            return validWeapons[UnityEngine.Random.Range(0, validWeapons.Count)];
        // 오류 상황
        else
        {
            Debug.LogError("무기 찾기 오류");
            return validWeapons[0];
        }
    }

    public WeaponData FindWeapon(Tier _tier)
    {
        // 조건 맞춤 무기 리스트
        WeaponData[] validWeapons = Array.FindAll(weaponDatas, data => data.tier == _tier);

        return validWeapons[UnityEngine.Random.Range(0, validWeapons.Length)];
    }

    public WeaponData[] FindWeapon(WeaponAttribute attribute)
    {
        return Array.FindAll(weaponDatas, data => data.attribute == attribute);
    }

    public WeaponData FindWeapon(int ID)
    {
        return Array.Find(weaponDatas, data => data.index == ID);
    }

    public bool EnforceCheck(WeaponData _data, EnforceID id)
    {
        return _data.plus.enforce[(int)id];
    }
    #endregion

    #region Monster
    public MonsterData FindMonster(string name)
    {
        return Array.Find(monsterDatas, data => data.name == name);
    }
    #endregion

    #region Trap
    public TrapData FindTrap(TrapType type)
    {
        return Array.Find(trapDatas, data => data.type == type);
    }
    #endregion

    #region Explain
    public string ExportExplain_Ran(CardType type)
    {
        ExplainData explainData = Array.Find(explainDatas, data => data.type == type);

        return explainData.explains[UnityEngine.Random.Range(0, explainData.explains.Length)];
    }
    #endregion

    #region Tier
    public Color ExportColor(Tier tier)
    {
        return Array.Find(tierColors, color => color.tier == tier).color;
    }
    #endregion

    #region BuffIcon
    public BuffIconData FindBuffIconData(BuffIconType type)
    {
        return Array.Find(buffIconDatas, icon => icon.type == type);
    }
    #endregion
}

// 기타 데이터
public enum AnimID { Idle, Walk, Damaged, Atk, Die, Interaction}