using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// 制作者　日本電子専門学校　ゲーム制作科　22CI0212　川島
public class RoomConnectManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI connectNameText;
    public TextMeshProUGUI getNameText { get { return connectNameText; } }
    [SerializeField] TextMeshProUGUI connectOptionText;
    public TextMeshProUGUI getOptionText { get { return connectOptionText; } }
    [SerializeField] GameObject connectPasswardArea;
    public GameObject getPasswardArea { get { return connectPasswardArea; } }
    [SerializeField] TMP_InputField connectPaswardText;
    public TMP_InputField getPaswardText { get { return connectPaswardText; } }
}