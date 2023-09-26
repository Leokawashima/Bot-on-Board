using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateQR : MonoBehaviour
{
    [SerializeField]string m_Text = "github.com";

    [ContextMenu("CreateQR")]
    void Create()
    {
        CreateQRCode(m_Text);
    }

    public void CreateQRCode(string input)
    {
        #region Encoding
        //モード指示子 英数字固定
        string modeIndicator = "0010";
        //文字数指示子
        string lenIndicator = Convert.ToString((input.Length), 2);
        while (lenIndicator.Length < 9) { lenIndicator = "0" + lenIndicator; }
        //データ
        string data = "";
        for (int i = 0; i <= (input.Length - 1) / 2; i++)
        {
            int H, L, deciInt;
            string deciStr = "";
            if (i * 2 + 1 < input.Length)
            {
                H = input[i * 2];
                L = input[i * 2 + 1];
                deciInt = AscTo45(H) * 45 + AscTo45(L);
                deciStr = Convert.ToString((deciInt), 2);
                while (deciStr.Length < 11) { deciStr = "0" + deciStr; }
            }
            else
            {
                L = input[i * 2];
                deciInt = AscTo45(L);
                deciStr = Convert.ToString((deciInt), 2);
                while (deciStr.Length < 6) { deciStr = "0" + deciStr; }
            }
            data += deciStr;
        }
        //終端パターン
        string terminator = "0000";

        string dataraw = modeIndicator + lenIndicator + data + terminator;
        List<int> dataList = new List<int>();
        while (dataraw.Length >= 8)
        {
            string codeLang = dataraw.Substring(0, 8);
            dataList.Add(Convert.ToInt32(codeLang, 2));
            dataraw = dataraw.Substring(8);
        }
        if (dataraw.Length > 0)
        {
            while (dataraw.Length < 8) { dataraw += "0"; }
            string codeLang = dataraw.Substring(0, 8);
            dataList.Add(Convert.ToInt32(codeLang, 2));
        }
        while (dataList.Count < 9)
        {
            bool addflg = true;
            if (addflg) { dataList.Add(236); }
            else { dataList.Add(17); }
            addflg = !addflg;
        }

        //リードソロモン誤り訂正方式で誤り訂正コード語作成
        List<int> exponent = new List<int>() { 1 };
        List<int> integer = new List<int>() { 0 };
        for (int i = 0; i < 255; i++)
        {
            exponent.Add(exponent[i] * 2);
            if (exponent[i + 1] >= 256)
            {
                exponent[i + 1] = exponent[i + 1] - 256;
                exponent[i + 1] = exponent[i + 1] ^ 29;
            }
            integer.Add(0);
        }
        for (int i = 0; i <= 255; i++)
        {
            for (int j = 0; j < 255; j++)
                if (exponent[j] == i) { integer[i] = j; }
        }

        List<int> fx = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 8; i >= 0; i--) { fx.Add(dataList[i]); }
        List<int> gx = new List<int>() { 79, 99, 125, 53, 85, 134, 143, 41, 249, 83, 197, 22, 119, 120, 83, 66, 119, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
        while (GetMaxDegree(fx) >= 17)
        {
            int DeltaDegree = GetMaxDegree(fx) - GetMaxDegree(gx);
            List<int> gxBuf = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i <= 17; i++) { gxBuf[i + DeltaDegree] = gx[i]; }

            int DeltaAlpha = integer[fx[GetMaxDegree(fx)]];
            for (int i = 0; i <= 25; i++)
            {
                int gxInt = gxBuf[i];
                if (gxInt == 0) { gxBuf[i] = 0; }
                else
                {
                    int gxAlpha = integer[gxInt];
                    gxAlpha += DeltaAlpha;
                    if (gxAlpha > 255) { gxAlpha -= 255; }
                    gxBuf[i] = exponent[gxAlpha];
                }
            }
            for (int i = 0; i <= 25; i++) { fx[i] = fx[i] ^ gxBuf[i]; }
        }

        for (int i = 16; i >= 0; i--) { dataList.Add(fx[i]); }
        string dataWithErrorStr = "";
        for (int i = 0; i <= 25; i++)
        {
            int num = dataList[i];
            string str = Convert.ToString(num, 2);
            while (str.Length < 8) { str = "0" + str; }
            dataWithErrorStr += str;
        }
        #endregion Encoding

        #region Create
        //QRコード生成
        int QRsize = 21;
        int[,] bitAry = new int[QRsize, QRsize];

        //初期化
        for (int i = 0; i < QRsize; ++i)
        {
            for (int j = 0; j < QRsize; ++j)
            {
                bitAry[i, j] = 0;
            }
        }

        //タイミングパターン
        for (int i = 0; i < QRsize; ++i)
        {
            if (i % 2 == 0)
            {
                bitAry[i, 6] = 1;
                bitAry[6, i] = 1;
            }
        }
        bitAry[13, 8] = 1;

        //位置検出パターン
        int startrow, startcol;
        startrow = 0;
        startcol = 0;
        for (int i = 0; i <= 6; ++i)
        {
            for (int j = 0; j <= 6; ++j)
            {
                if ((i == 0) || (j == 0) || (i == 6) || (j == 6)) { bitAry[startrow + i, startcol + j] = 1; }
                if ((i != 1) && (j != 1) && (i != 5) && (j != 5)) { bitAry[startrow + i, startcol + j] = 1; }
            }
        }
        startrow = 0;
        startcol = 14;
        for (int i = 0; i <= 6; ++i)
        {
            for (int j = 0; j <= 6; ++j)
            {
                if ((i == 0) || (j == 0) || (i == 6) || (j == 6)) { bitAry[startrow + i, startcol + j] = 1; }
                if ((i != 1) && (j != 1) && (i != 5) && (j != 5)) { bitAry[startrow + i, startcol + j] = 1; }
            }
        }
        startrow = 14;
        startcol = 0;
        for (int i = 0; i <= 6; ++i)
        {
            for (int j = 0; j <= 6; ++j)
            {
                if ((i == 0) || (j == 0) || (i == 6) || (j == 6)) { bitAry[startrow + i, startcol + j] = 1; }
                if ((i != 1) && (j != 1) && (i != 5) && (j != 5)) { bitAry[startrow + i, startcol + j] = 1; }
            }
        }

        //データを埋めていく
        int row = QRsize - 1;
        int col = QRsize - 1;
        bool UpFlag = true;
        for (int i = 0; i <= 103; ++i)
        {
            bitAry[row, col] = (int)Char.GetNumericValue(dataWithErrorStr[i * 2]);
            bitAry[row, col - 1] = (int)Char.GetNumericValue(dataWithErrorStr[i * 2 + 1]);

            if (UpFlag)
            {
                if (IsDataArea(row - 1, col)) { row += -1; }
                else
                {
                    if (IsTimingArea(row - 1, col)) { row += -2; }
                    else
                    {
                        col += -2;
                        UpFlag = !UpFlag;
                        if (IsTimingArea(row, col)) { col += -1; }
                    }
                }
            }
            else
            {
                if (IsDataArea(row + 1, col)) { row += 1; }
                else
                {
                    if (IsTimingArea(row + 1, col)) { row += 2; }
                    else
                    {
                        col += -2;
                        UpFlag = !UpFlag;
                        if (IsTimingArea(row, col)) { col += -1; }
                        else
                        {
                            if (IsFormatArea(row, col)) { row += -8; }
                        }

                    }
                }
            }
        }

        //マスク処理
        for (int i = 0; i <= 20; ++i)
        {
            for (int j = 0; j <= 20; ++j)
            {
                if ((IsDataArea(i, j)) && ((i + j) % 3 == 0))
                {
                    if (bitAry[i, j] == 1)
                    {
                        bitAry[i, j] = 0;
                    }
                    else
                    {
                        bitAry[i, j] = 1;
                    }
                }
            }
        }

        //string formatStr = "001100111010000";
        string formatStr = "000010111001100";
        int index = 0;
        int Celindex = 0;
        while (index < 15)
        {
            bitAry[Celindex, 8] = (int)Char.GetNumericValue(formatStr[index]);
            Celindex += 1;
            while (!IsFormatArea(Celindex, 8)) { Celindex += 1; }
            index += 1;
        }

        index = 0;
        Celindex = 20;
        while (index < 15)
        {
            bitAry[8, Celindex] = (int)Char.GetNumericValue(formatStr[index]);
            Celindex += -1;
            while (!IsFormatArea(8, Celindex))
            {
                Celindex += -1;
            }
            if (Celindex == 8) { Celindex += -1; }
            index += 1;
        }

        #endregion Create

        var tex = CreateTexture(QRsize, bitAry);
        tex.Apply();

        string path = Name.Setting.AppFilePath + "/QR.png";
        System.IO.File.WriteAllBytes(path, tex.EncodeToPNG());
        DestroyImmediate(tex);
        AssetDatabase.Refresh();
    }

    bool IsPositionArea(int i, int j)
    {
        if ((i - 0 <= 7) && (j - 0 <= 7)) { return true; }
        if ((i - 0 <= 7) && (j - 13 <= 7) && (j >= 13)) { return true; }
        if ((j - 0 <= 7) && (i - 13 <= 7) && (i >= 13)) { return true; }
        return false;
    }
    bool IsTimingArea(int i, int j)
    {
        if ((i == 6) || (j == 6)) { return true; }
        if ((i == 13) && (j == 8)) { return true; }
        return false;
    }
    bool IsFormatArea(int i, int j)
    {
        if ((i == 8) && ((j < 6) || (j == 7) || (j == 8) || (j > 12))) { return true; }
        if ((j == 8) && ((i < 6) || (i == 7) || (i == 8) || (i > 13))) { return true; }
        return false;
    }
    bool IsDataArea(int i, int j)
    {
        if (IsPositionArea(i, j)) { return false; }
        if (IsTimingArea(i, j)) { return false; }
        if (IsFormatArea(i, j)) { return false; }
        if ((i > 20) || (j > 20)) { return false; }
        if ((i < 0) || (j < 0)) { return false; }
        return true;
    }

    int GetMaxDegree(List<int> function)
    {
        for (int i = 25; i >= 0; i--)
        {
            if (function[i] > 0)
                return i;
        }
        return 0;
    }
    int AscTo45(int asc)
    {
        if ((asc >= 48) && (asc <= 57)) { return asc - 48; }
        if ((asc >= 65) && (asc <= 90)) { return asc - 65 + 10; }
        if ((asc >= 97) && (asc <= 122)) { return asc - 97 + 10; }
        if (asc == 32) { return 36; }
        if (asc == 36) { return 37; }
        if (asc == 37) { return 38; }
        if (asc == 42) { return 39; }
        if (asc == 43) { return 40; }
        if (asc == 45) { return 41; }
        if (asc == 46) { return 42; }
        if (asc == 47) { return 43; }
        if (asc == 58) { return 45; }
        return 0;
    }

    Texture2D CreateTexture(int size, int[,] patern)
    {
        var texture = new Texture2D(size, size, TextureFormat.RGB24, false);

        for (int y = 0; y < texture.height; ++y)
            for (int x = 0; x < texture.width; ++x)
            {
                if (patern[y, x] == 1)
                    texture.SetPixel(x, y, Color.white);
                else
                    texture.SetPixel(x, y, Color.black);
            }

        return texture;
    }
}
