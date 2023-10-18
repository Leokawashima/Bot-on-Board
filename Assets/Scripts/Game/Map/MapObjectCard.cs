using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapObjectCard : MonoBehaviour
{
    public MapObject_SO_Template m_SO;
    public int m_Index;
    public TextMeshProUGUI m_Text;
    public Toggle m_Toggle;
    public CardManager m_CardManager;

    public void ObjectSpawn(MapChip chip_, MapManager map_)
    {
        m_SO.ObjectSpawn(chip_.m_position, chip_.transform.position + Vector3.up, map_.transform);
    }

    public void Trash()
    {
        m_CardManager.m_TrashCardList.Add(m_Index);
        m_CardManager.m_HandCardList.Remove(m_Index);
        Destroy(gameObject);
    }
}