using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Redcode.Pools;
using Sirenix.OdinInspector;
using TMPro;
using DG.Tweening;

public abstract class Card : MonoBehaviour, IPoolObject
{
    [Title("변수")]
    public int pos;
    public float animTime;
    public CardType type;
    public bool isTurn; // 턴제 확인

    [Title("자식 컴포넌트")]
    public SpriteRenderer backGround;
    public SpriteRenderer activeImg;
    public Transform objTrans;
    public TextMeshPro cardName;
    public TextMeshPro uiText;
    protected SpriteRenderer spriteRenderer;
    protected List<Transform> effectList = new List<Transform>();

    // 외부 컴포넌트
    protected SpriteData spriteData;

    // 매니저
    protected GameManager gameManager;
    protected SpawnManager spawnManager;
    protected TouchManager touchManager;
    protected CSVManager csvManager;
    protected RelicManager relicManager;

    public virtual void OnCreatedInPool()
    {
        name = name.Replace("(Clone)", "");

        // 내부 컴포넌트 불러오기
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 외부 컴포넌트 불러오기
        spriteData = SpriteData.Instance;

        gameManager = GameManager.Instance;
        spawnManager = SpawnManager.Instance;
        touchManager = TouchManager.Instance;
        csvManager = CSVManager.Instance;
        relicManager = RelicManager.Instance;
    }

    public virtual void OnGettingFromPool()
    {
        backGround.sprite = spriteData.ExportRanStage();
        spriteRenderer.color = Color.white;
        SetCard();
        SetActive(false);

        DOSpawn();
    }

    public abstract void SetCard();

    public abstract void DestroyCard();

    public abstract IEnumerator DoCard();

    public virtual void DoTurnCard()
    {

    }

    public virtual IEnumerator Move(int _pos)
    {
        bool isMoving = false;
        Transform target = spawnManager.cardPos[_pos];
        transform.SetParent(target);
        pos = _pos;

        transform.DOMove(target.position, 0.5f).SetEase(Ease.OutBounce).SetUpdate(true).OnComplete(() =>
            {
                transform.localPosition = Vector3.zero;
                isMoving = true;
            });

        yield return new WaitWhile(() => !isMoving);
    }

    public abstract IEnumerator Damaged(int _amount);

    protected Transform FindEffect(EffectType type)
    {
        return effectList.Find(effectTransform => effectTransform.name == type.ToString());
    }

    #region 애니메이션
    public void DOSpawn()
    {
        transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        transform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce).SetUpdate(true);
    }

    public void DODestroy()
    {
        transform.DOScale(0.05f, 0.2f).SetEase(Ease.InBounce).SetUpdate(true);
    }

    public void DODamaged()
    {
        spriteRenderer.color = Color.white;

        Sequence seq = DOTween.Sequence();

        seq.Append(spriteRenderer.DOColor(Color.red, 0.2f))
            .Append(spriteRenderer.DOColor(Color.white, 0.1f)).SetUpdate(true);
    }

    public IEnumerator SetAnim(Animator _anim, AnimID id)
    {
        _anim.SetTrigger(id.ToString());

        yield return new WaitForEndOfFrame();

        animTime = _anim.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animTime);
    }
    #endregion

    #region UI
    protected void SetCardName(string s)
    {
        cardName.text = s;
    }

    protected void SetUI(string s)
    {
        uiText.text = s;
    }

    public void SetActive(bool isActive)
    {
        if(isActive)
            activeImg.gameObject.SetActive(false);
        else
            activeImg.gameObject.SetActive(true);
    }
    #endregion

    #region 이웃찾기
    public Vector2 DirToPos(Direction dir)
    {
        Vector2 targetPos = transform.parent.position;

        switch (dir)
        {
            case Direction.T:
                targetPos += new Vector2(0, 2.4f);
                break;
            case Direction.B:
                targetPos += new Vector2(0, -2.4f);
                break;
            case Direction.L:
                targetPos += new Vector2(-1.8f, 0);
                break;
            case Direction.R:
                targetPos += new Vector2(1.8f, 0);
                break;
        }

        return targetPos;
    }

    public Direction PosToDir(int pos)
    {
        Vector2 curPos = transform.parent.position;
        Vector2 tarPos = spawnManager.cardPos[pos].position;

        if(curPos.x != tarPos.x)
        {
            if (curPos.x > tarPos.x)
                return Direction.L;
            else
                return Direction.R;
        }
        else if(curPos.y != tarPos.y)
        {
            if (curPos.y > tarPos.y)
                return Direction.B;
            else
                return Direction.T;
        }

        // 다 아니라면 (오류)
        Debug.LogError("방향 찾기 오류");
        return Direction.L;
    }

    public Card FindNeighbor(Direction dir)
    {
        return spawnManager.FindCard(DirToPos(dir));
    }

    public Card[] FindNeighbors(Direction[] dir)
    {
        int length = dir.Length;
        Vector3[] poses = new Vector3[length];

        for(int i = 0; i < length; i++)
            poses[i] = DirToPos(dir[i]);

        return spawnManager.FindCards(poses);
    }
    #endregion
}

public enum CardType { Player, Chest, Coin, Consumable, Monster, Relics, Trap, Weapon, Empty }
public enum Direction { T, R, B, L }