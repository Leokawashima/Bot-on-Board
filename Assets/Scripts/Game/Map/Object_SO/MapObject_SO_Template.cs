using UnityEngine;
using UnityEngine.UI;

public abstract class MapObject_SO_Template : ScriptableObject
{
    public string m_ObjectName = string.Empty;
    public string m_Info = "カードにカーソルを合わせたときに表示する説明文";

    public uint
        m_Destroy_MaxTurn = 10,
        m_Destroy_SpawnTurn = 10;

    public int m_cost = 0;

    public bool m_IsCollider = false;

    public GameObject m_Prefab;
    public MapObjectCard m_Card;

    const float CardSelectOffset = 50.0f;

    Animator m_Animator;
    protected Animator GetAnimator
    { get
        { 
            return m_Animator == null ? m_Animator = m_Prefab.GetComponent<Animator>() : m_Animator;
        }
    }

    public virtual MapObject ObjectSpawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var go = Instantiate(m_Prefab, tf_.position + pos_, m_Prefab.transform.rotation, tf_);
        var mo = go.AddComponent<MapObject>();
        mo.m_SO = this;

        mo.m_Pos = posdata_;
        mo.NowTurn = m_Destroy_SpawnTurn;

        return mo;
    }
    public virtual MapObjectCard CardCreate(int index_, Transform tf_, ToggleGroup group_, CardManager cardManager_)
    {
        var moc = Instantiate(m_Card, tf_);
        moc.m_SO = this;
        moc.m_Index = index_;
        moc.m_Text.text = m_ObjectName;
        moc.m_Toggle.group = group_;
        moc.m_CardManager = cardManager_;
        moc.m_Toggle.onValueChanged.AddListener((bool isOn_) =>
        {
            var rect = moc.transform as RectTransform;
            rect.anchoredPosition = new Vector2(
                rect.anchoredPosition.x,
                isOn_ ?
                rect.anchoredPosition.y + CardSelectOffset :
                rect.anchoredPosition.y - CardSelectOffset
                );
        });

        return moc;
    }
    public virtual void Destry() { }
}

//以下継承クラス　インターフェース化する方がデータ設計が楽だが、
//プロパティは通常のインスペクターから見えない為後々編集エディターを作るまでクラス実装

public abstract class MO_SO_Weapon : MapObject_SO_Template
{
    public int m_AttackPow = 1;
    public abstract bool CheckAttackCollider();
    public abstract void Attack();
}

public abstract class MO_SO_Heal : MapObject_SO_Template
{
    public int m_HealPow = 1;
}