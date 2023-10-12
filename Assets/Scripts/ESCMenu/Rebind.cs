using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using MyFileSystem;

/// <summary>
/// まだ調整中
/// 参考　https://nekojara.city/unity-input-system-rebinding
/// </summary>
public class Rebind : MonoBehaviour
{
    [SerializeField] InputActionReference m_ActionReference;
    [SerializeField] TMP_Text _pathText;
    [SerializeField] Image _mask;
    [SerializeField] InputActionAsset _actionAsset;

    const string _scheme = "Keyboard";
    InputAction _action;
    InputActionRebindingExtensions.RebindingOperation _rebindOperation;

    readonly string m_FilePath = Name.Setting.FilePath_Setting + "/KeyBind.json";

    void Awake()
    {
        if(m_ActionReference == null) return;

        _action = m_ActionReference.action;

        RefreshDisplay();
    }
    void OnDestroy()
    {
        CleanUpOperation();
    }

    public void StartRebinding()
    {
        if(_action == null) return;

        // もしリバインド中なら、強制的にキャンセル
        // Cancelメソッドを実行すると、OnCancelイベントが発火する
        _rebindOperation?.Cancel();

        _action.Disable();

        // リバインド対象のBindingIndexを取得
        var bindingIndex = _action.GetBindingIndex(InputBinding.MaskByGroup(_scheme));

        if(_mask != null) _mask.enabled = true;

        // リバインドが終了した時の処理を行うローカル関数
        void OnFinished()
        {
            CleanUpOperation();// オペレーションの後処理

            _action.Enable();// 一時的に無効化したActionを有効化する

            if(_mask != null) _mask.enabled = false;
        }

        _rebindOperation = _action
            .PerformInteractiveRebinding(bindingIndex)
            .OnComplete(_ =>
            {
                // リバインドが完了した時の処理
                RefreshDisplay();
                OnFinished();
            })
            .OnCancel(_ =>
            {
                // リバインドがキャンセルされた時の処理
                OnFinished();
            })
            .WithCancelingThrough("<Keyboard>/escape").Start();
    }

    public void ResetOverrides()
    {
        _action?.RemoveAllBindingOverrides();
        RefreshDisplay();
    }

    public void RefreshDisplay()
    {
        if(_action == null || _pathText == null) return;

        _pathText.text = _action.GetBindingDisplayString();
    }

    void CleanUpOperation()
    {
        _rebindOperation?.Dispose();
        _rebindOperation = null;
    }

    public void Save()
    {
        if(_actionAsset == null) return;

        // InputActionAssetの上書き情報の保存
        var json = _actionAsset.SaveBindingOverridesAsJson();

        JsonFileManager.Save(m_FilePath, json);
    }

    public void Load()
    {
        if(_actionAsset == null) return;

        if (false == JsonFileManager.Load(m_FilePath, out var _str))
        {
            return;
        }

        // InputActionAssetの上書き情報を設定
        _actionAsset.LoadBindingOverridesFromJson(_str);
        RefreshDisplay();
    }
}
