using UnityEngine;
using UnityEngine.UI;

public abstract class MapObject_SO_Template : ScriptableObject
{
    public string m_MOName = "null";
    public string m_Info = "カードにカーソルを合わせたときに表示する説明文";

    public GameObject m_Prefab;
    private Animator m_Animator;
    protected Animator GetAnimator {
        get { 
            return m_Animator ? m_Animator = m_Prefab.GetComponent<Animator>() : m_Animator;
        }
    }

    public GameObject m_Card;
    private Animator m_CardAnimator;
    protected Animator GetCardAnimator {
        get {
            return m_CardAnimator ? m_CardAnimator = m_Card.GetComponent<Animator>() : m_CardAnimator;
        }
    }

    public Sprite 
        m_CardFrameImage,
        m_CardImage;

    public bool m_IsCollider;

    public virtual MapObject ObjectSpawn(Vector2Int pos_)
    {
        var go = Instantiate(m_Prefab);
        var mo = go.AddComponent<MapObject>();
        mo.m_SO = this;

        mo.m_Pos = pos_;

        return mo;
    }
    public virtual MapObjectCard CardCreate(Transform tf_)
    {
        var go = Instantiate(m_Card);
        var moc = go.AddComponent<MapObjectCard>();
        moc.m_SO = this;

        var go_img_frame = Instantiate(new GameObject("IMG_Frame"), go.transform);
        go_img_frame.AddComponent<Image>().sprite = m_CardFrameImage;

        var go_img = Instantiate(new GameObject("IMG_Card"), go.transform);
        go_img.AddComponent<Image>().sprite = m_CardImage;

        return moc;
    }
    public virtual void TrunUpDate() { }
    public virtual void Destry() { }
}

public abstract class MO_Destroy_SO_Template : MapObject_SO_Template
{
    public uint m_Destry_MaxTurn, m_Destroy_NowTurn;
}