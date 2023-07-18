using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomMakeManager : MonoBehaviour
{
    [SerializeField] TMP_InputField nameText;
    public string getName { get { return nameText.text; } }
    [SerializeField] TMP_InputField optionText;
    public string getOption { get { return optionText.text; } }
    [SerializeField] Toggle passwardToggle;
    public bool getPasswardIsOn { get { return passwardToggle.isOn; } }
    [SerializeField] TMP_InputField passwardText;
    public string getPassward { get { return passwardText.text; } }
    [SerializeField] TMP_InputField userMaxText;
    public int getUserMax { get { return int.Parse(userMaxText.text); } }
}
