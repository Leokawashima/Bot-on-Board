using UnityEngine;
using UnityEngine.UI;

public abstract class MapObject_SO_Template : ScriptableObject
{
    public string m_ObjectName = "null";
    public string m_Info = "カードにカーソルを合わせたときに表示する説明文";

    public GameObject m_Prefab;
    private Animator m_Animator;
    protected Animator GetAnimator {
        get { 
            return m_Animator ? m_Animator = m_Prefab.GetComponent<Animator>() : m_Animator;
        }
    }

    public GameObject m_Card;

    public bool m_IsCollider;

    const int ObjOffset = 1;//マップに高低差を設ける場合この変数を使わないべき

    public virtual MapObject ObjectSpawn(Vector2Int posdata_, Vector3 pos_, Transform tf_)
    {
        var go = Instantiate(m_Prefab, tf_.position + pos_, m_Prefab.transform.rotation, tf_);
        var mo = go.AddComponent<MapObject>();
        mo.m_SO = this;

        mo.m_Pos = posdata_;

        return mo;
    }
    public virtual MapObjectCard CardCreate(Transform tf_)
    {
        var go = Instantiate(m_Card, tf_);
        var moc = go.GetComponent<MapObjectCard>();
        moc.m_SO = this;
        moc.m_Text.text = m_ObjectName;

        return moc;
    }
    public virtual void TrunUpDate() { }
    public virtual void Destry() { }
}

public abstract class MO_Destroy_SO_Template : MapObject_SO_Template
{
    public uint m_Destry_MaxTurn, m_Destroy_NowTurn;
}