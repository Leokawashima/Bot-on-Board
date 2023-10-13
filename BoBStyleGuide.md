# Bot on Board Style Guide
* * *
## 命名規則
#### ケース名
* camelCase(キャメルケース)  
    このプロジェクトではローワーキャメルケースを指すものとします。  
    具体的には先頭の単語の最初を小文字、続く単語の最初を大文字にします。  
  ``` Charp
  int playerIndex;
  ```
* PascalCase(パスカルケース)  
    このプロジェクトではアッパーキャメルケースを指すものとします。  
    具体的には単語の最初を全て大文字にします。  
  ``` Charp
  int PlayerIndex;
  ```
* snake_case(スネークケース)  
    このプロジェクトではローワースネークケースを指すものとします。  
    具体的にはアンダースコアを使用して単語の間を接続します。  
  ``` Charp
  int player_index;
  ```
* CONSTANT_CASE(コンスタントケース)  
    このプロジェクトではアッパースネークケースを指すものとします。  
    具体的には単語は全て大文字にし、アンダースコアを使用して単語の間を接続します。
  ``` Charp
  int PLAYER_INDEX;
  ```
#### 規則一覧
| 種類 | スタイル | 例 | 備考 |
| ---- | ---- | ---- | ---- |
| 名前空間 | PascalCase | namespace PlayerData |
| インターフェース | PascalCase | interface IPlayerIndex | プレフィックス `I` |
| クラス | PascalCase | class PlayerIndex |
| 抽象クラス | PascalCase | abstract class PlayerIndex_Template | サフィックス `_Template` |
| ScriptableObjectクラス | PascalCase | class PlayerIndex_SO | サフィックス `_SO` |
| ScriptableObject抽象クラス | PascalCase | abstract class PlayerIndex_SO_Template | サフィックス `_SO_Template` |
| メソッド | PascalCase | void PlayerIndex() |
| ローカルメソッド | PascalCase | void _PlayerIndex() | プレフィックス `_` |
| コルーチン | PascalCase | IENumrator CoPlayerIndex() | プレフィックス `Co` |
| ローカルコルーチン | PascalCase | IENumrator Co_PlayerIndex() | プレフィックス `Co_` |
| private フィールド | camelCase | private int m_playerIndex; | プレフィックス `m_` |
| protected フィールド | camelCase | protected m_playerIndex; | プレフィックス `m_` |
| public フィールド | PascalCase | public int PlayerIndex; |
| internal フィールド | PascalCase | internal int PlayerIndex; |
| protected internal フィールド | PascalCase | protected internal int PlayerIndex; |
| private protected フィールド | camelCase | private protected int m_playerIndex; | プレフィックス `m_` |
| const フィールド | CONSTANT_CASE | const int PLAYER_INDEX; |
| private / protected static フィールド | PascalCase | private static int m_PlayerIndex; | プレフィックス `m_` |
| public / internal static フィールド | PascalCase | public static int PlayerIndex; |
| ローカル変数 | camelCase | int _playerIndex; | プレフィックス `_` |
| 引数 / パラメーター | camelCase | (int playerIndex_) | サフィックス `_` |
| プロパティ | PascalCase | public int PlayerIndex { get; private set; }|
| 列挙型 | PascalCase | enum PlayerState { <br />&nbsp;&nbsp;&nbsp;&nbsp;Non = -1, <br />&nbsp;&nbsp;&nbsp;&nbsp;Alive, <br />&nbsp;&nbsp;&nbsp;&nbsp;Dead, <br />} |
#### 変数名省略について
このプロジェクトではフィールドの変数名を省略したものにすることを禁じます。  
具体的にはカウントの変数を`Cnt`と省略すること禁じます。  
ローカル変数はその限りではありません。  
具体的にはゲームオブジェクトを`go`と省略することを許容します。  
しかし、使用されているスコープ内で似通った意味合いを持つ変数等がある場合に省略を多用することは好ましくありません。  
使用範囲が限定されている場合のみ許容されているということを念頭に命名をしてください。  

## フォーマットルール
#### コメントアウトについて
``` CSharp
// コメントアウトは必ず半角スペースを空けて記述します。
// 基本的に最大でも2~3行ほどで簡潔に説明することを遵守してください。
/*
 * 範囲コメントアウトでは必ず開始と終了を新規の行に配置して記述します。
 * アスタリスクを用いた範囲コメントは基本的に細かい仕様を伝えるための使用以外を禁じます。
 * また、主に呼び出しを行う上層関数等での使用も禁じます。
 * 理由としてはそのようなコメントをしないと理解の難しい呼び出し方をする関数は、
 * 可読性を著しく損なう可能性が高く、設計の時点で間違えている可能性が高いためです。
 * 末端の呼び出される側である関数内でのロジックの解説を行う場合などにのみ使用してください。
 */
/// XMLコメントは機能が完成している関数には原則記述します。
/// 例外としてローカルメソッドなどはコメントはするべきですがXMLでの記述は必須でないものとします。

/// <summary>
/// 一人当たりのプレイヤーデータを管理するクラス
/// </summary>
public class PlayerData : MonoBehaviour
{
    /// <summary>
    /// プレイヤーのインデックスを初期化する処理
    /// </summary>
    void InitializeIndex()
    {
        // 記述...
    }
}

/// このようにクラスやメソッドにはコメントでマークダウンを行ってください。
/// <summary>では最大でも2~3行ほどで簡潔に説明することを遵守してください。
```
#### 改行について
  このプロジェクトでは基本的にK&Rスタイルは使用せず、オールマンスタイルで改行するものとします。  
  具体的には波かっこ `{` を使用してから改行することを禁じ、開き波かっこと閉じ波かっこを縦に整列して記述します。  
``` CSharp
// K&R
namespace PlayerData {
	public class PlayerData	{
      // 記述...
	}
}

// AllMan
namespace PlayerData
{
  public class PlayerData
  {
      // 記述...
	}
}
```
