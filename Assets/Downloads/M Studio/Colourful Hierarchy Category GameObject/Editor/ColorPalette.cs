using System;
using System.Collections.Generic;
using UnityEngine;

// こちらのサイトを参考にインポートした
// https://umistudioblog.com/%E3%80%90unity%E3%80%91hierarchy-%E3%83%93%E3%83%A5%E3%83%BC%E3%82%92%E8%89%B2%E5%88%86%E3%81%91%E3%81%97%E3%81%A6%E8%A6%8B%E3%82%84%E3%81%99%E3%81%8F%E3%81%97%E3%81%9F%E3%81%84%E3%80%90%E7%84%A1/
// StyleHierarchy.csにたくさんコメントしている
namespace MStudio
{
    /// <summary>
    /// カスタムデザイン単位のクラス
    /// </summary>
    [Serializable]
    public class ColorDesign
    {
        [Tooltip("処理方法を選択するヤツ")]
        public KeyState State;
        [Tooltip("キーとなる名前")]
        public string KeyName;
        [Tooltip("アルファ値255にすんの忘れんなよ(意訳)")]
        public Color TextColor;
        [Tooltip("アルファ値255にすんの忘れんなよ(意訳)")]
        public Color BackGroundColor;
        [Tooltip("テキストの並び")]
        public TextAnchor TextAlignment;
        [Tooltip("太さとかの文字スタイル")]
        public FontStyle FontStyle;
        [Tooltip("テキストケース")]
        public CaseState Case;

        public enum KeyState
        {
            Equal,
            Begin,
            BeginDelete,
            End,
            EndDelete,
        }

        public enum CaseState
        {
            Default,
            ToUpper,
            ToLower,
        }
    }

    /// <summary>
    /// カスタムデザインをリストに保存するクラス
    /// </summary>
    [CreateAssetMenu(fileName = "ColorPalette", menuName = "MStudio/ColorPalette")]
    public class ColorPalette : ScriptableObject
    {
        public List<ColorDesign> ColorDesigns = new();
    }
}