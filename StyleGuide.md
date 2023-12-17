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
| イベントハンドラ | PascalCase | void OnPlayerIndex() | プレフィックス`On` |
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
| readonly フィールド | CONSTANT_CASE | readonly int m_PLAYER_INDEX; | プレフィックス `m_` |
| private / protected static フィールド | camelCase | private static int s_playerIndex; | プレフィックス `s_` |
| public / internal static フィールド | PascalCase | public static int PlayerIndex; |
| ローカル変数 | camelCase | int _playerIndex; | プレフィックス `_` |
| 引数 / パラメーター | camelCase | (int playerIndex_) | サフィックス `_` |
| プロパティ | PascalCase | public int PlayerIndex { get; private set; }|
| 列挙型 | PascalCase | enum PlayerState { <br />&nbsp;&nbsp;&nbsp;&nbsp;Non = -1, <br />&nbsp;&nbsp;&nbsp;&nbsp;Alive, <br />&nbsp;&nbsp;&nbsp;&nbsp;Dead, <br />} |
#### 変数名省略について
このプロジェクトではフィールドの変数名を省略したものにすることを禁じます。  
具体的にはカウントの変数を`Cnt`と省略すること禁じます。  
ローカル変数や引数はその限りではありません。  
具体的にはゲームオブジェクトを`go`と省略することを許容します。  
しかし、使用されているスコープ内で似通った意味合いを持つ変数等がある場合に省略を多用することは好ましくありません。  
使用範囲が限定されている場合のみ許容されているということを念頭に命名をしてください。 
#### 変数名の順序について
このプロジェクトではある条件下では原則　名詞、形容詞の順に命名します。  
具体的には  
`m_maxValue`と`m_minValue`  
と命名することはせず、  
`m_valueMax`と`m_ValueMin`  
と命名します。  
条件としては特定の変数、仮に`value`の最大値、最小値、現在値等を変数として宣言する場合です。  
これは形容詞が対義語であるなどの場合問題ないですが、2つ以上の変数がある、  
複雑な状態を指している場合等に末尾の名刺を見るまで変数を読み取りづらいためです。  

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
    private void InitializeIndex()
    {
        // 記述...
    }
}

/// このようにクラスやメソッドにはマークダウンを行ってください。
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

#### コードルールについて
* privateを明示的に宣言するものとします。 
  これはStart等のマジックメソッドにデフォルトで実装されるものを消去する手間と、
  いろいろな人に書いといたほうが良いよと言われた為です(一敗)  
``` CSharp
  private int m_hoge;
  private void Start()
　{
　　// 記述...
　}
```
* bool値のif文は`!`での反転を行った判定を禁じます。  
  これは簡潔に記述できるもののコードリーディングを行う際に見落とす可能性を考慮したものです。  
  `==`を`=`にしてしまう記述ミスでのバグが生まれる可能性もありますが、気にしないものとします。
  具体的には反転したい場合'false =='から始まる記述を遵守してください。
  'false'が後部にある場合反転するのかが後部を確認するまでわからない為です。
``` CSharp
  if (m_isOpen)
  {
      // 記述...
　}
  if (false == m_isPlaying)
  {
      // 記述...
  }
```
* Awakeではフィールドの初期化、Startでは参照、取得する行為を含めた初期化を行います。  
  これはAwakeが呼び出されるときに他のオブジェクトの生成処理が終わっていない可能性を考慮し、  
  Awakeでは他のコンポーネントを参照、取得以外の初期化を行い、  
  StartではAwakeでできなかった初期化処理を記述します。  
``` CSharp
  [SerializeField]
  private int m_MapSizeX = 10;
  [SerializeField]
  private int m_MapSizeY = 5;
  private readonly Vector2Int m_MAP_SIZE;
  // MapManager 型
  private readonly MapManager m_MAP_MANAGER;

  private void Awake()
  {
    // 参照、取得なしでできる初期化を行う(Vector2Intを[SerializeField]しろは言わないでね)
    m_MAP_SIZE = new(m_MapSizeX, m_MapSizeY);
  }

  private void Start()
  {
　　// 相手のシングルトンインスタンスを取得する
    // MapManager.Singleton はAwakeで初期化されている
    m_MAP_MANAGER = MapManager.Singleton;
  }
```

#### 変数宣言について
* public変数、プロパティ、カプセル化について。
  このプロジェクトではカプセル化を重視して変数を取り扱うため、アクセス権を限界まで抑えることを意識します。
1. public int PlayerIndex;  
2. [field: SerializeField] int playerIndex { get; set; }  

    1番は2番のように書き換えることができます。  
    しかし、このように書き換えるだけでは行数が増えるだけであり、機能としてはまるで意味を持ちません。  
    重要なのは、不正な代入を防ぐことや不必要なアクセス権を排除することにあります。  
    もし、変数`PlayerIndex`は0以下の値は禁止、値を設定するのは最初だけ。というルールがあるのであれば、以下のように記述します。
    C# ではInitキーワードというプロパティをコンストラクタ等の初期化時限定で値を設定できるreadonly化するようなものがあるが、
    Unityでは実装自体はダミークラスでできるものの推奨されておらず、バージョンの差によっては機能しない為、
    このプロジェクトではInitキーワードは使用していません。
``` CSharp
  // Indexという扱いをする変数なため配列のキーとして使用すること。
  // int型へのキャストが必要になり数値が失われる可能性などが存在する為int型で宣言
  private readonly int m_playerIndex;
  public int PlayerIndex => m_playerIndex;

  // コンストラクタ内
  if (index_ < 0)
  {
    throw new ArgumentException(GetType().ToString() + "で不正な値が代入されました");
  }
  m_playerIndex = index_
```
   次に動的にクラス内部で変更されるが外部公開したい変数、外部から変更の恐れのある変数の記述です。  
   単に公開する場合はプロパティ化しセッターはprivate属性にする事で解決できます。  
   外部から変更の恐れのある変数は単純な代入チェックを行う場合はセッターを改変し、  
   複雑な代入チェックなどが必要になる場合はクラスとして独立させる事や関数かすることで設計を改善することが必要になります。
1. public int HP;
2. public int Attack;
``` CSharp
  // 外部公開する変数
  // もしInspectorに表示したいは、[field: SerializeField]を付与する
  // ダメージないし回復は別にメソッドを定義する事。　変数から参照は追えるが、加算減算をそれぞれ追うことは難しいため
  public int HP { get; private set; }

  // 外部から変更の恐れのある変数
  // 最低限の記述で済ませる場合
  public int Attack
  {
	get;
	set
	{
	  if (value < 0)
	  	Debug.Log("0以下の値が代入されています");
	  else
	  	Attack = value;
	}
  }
```
* varやnew()のような型推論、初期化を省略する書き方について。  
  原則この記述を積極的に活用してコーディングします。  
  これはコードの修正が必要な場面等でクラス型などを使用している場合は修正箇所を減らすことができ、  
  宣言時も型名が著しく長くなってしまい変数名を追いづらくなる事態を避けることができる為です。  
  しかし、int, float, double等の数値の取り扱いをvarで記述すると初期化する値次第でヒューマンエラーが発生します。  
  もちろんvarで宣言したほうが簡潔であったり複数の変数を宣言したい場合名前の開始位置を統一できる等活用すべき時もあります。  
  そのため、この記述に関しては記述者に委ねます。  
  読み手にとって理解しやすいか、読みやすいかを意識して記述することを念頭に置いてください。  
  また、変数宣言を行うときは念入りにバグが発生することのないように読み返すこと。  
``` CSharp
  // この場合はクラス名にも問題はあるがvarを積極採用
  var _classObject = new TooLongClassNameHogeHuga();
  // この場合はvar宣言を推奨するがAStarArgorithm型で宣言しても良い。
  // その場合は new();で初期化することを推奨する。
  var _classObject1 = new AStarAlgorithm();
  AStarAlgorithm _classObject2 = new();

  // この場合は_playerHPは本当に0で初期化していいのか？0.0fじゃなくていいのか？
  var _playerIndex = 0;
  var _playerHP = 0;
 ```

* タプル式について。  
  使用可能ですが、過度な使用は推奨しません。  
  これは匿名の型である以上、大きさと内容が乱雑に違うものが宣言されていては可読性を著しく損なう恐れがある為です。  
  適度でわかりやすく、また積極的に名前が付けられているものであれば尚よいです。  
  
* 文字列結合について。
  一般的にStringBuilderでの結合がメモリアロケーションの観点から見て速度、効率ともに良いという結果があるのですが、
  2023/12/15日時点での10万回の'+='と'.Append()'の比較を試したところ'.Append'のほうがわずかに遅いという結果が得られました。
  これは年月の経過によりVisualStudioがより効率的なコンパイルをするようになった為なのか、  
  それとも速度的には少し劣るが内部的にメモリ効率はいいままなのか、詳しい結果は細かく確認していない為わかりません。  
  しかし、'+='での実装は依然追いにくいため、結合についてはStringBuilderでの実装を推奨します。  
  それ以外の文字列操作については記述者に委ねます。  
  ZStringBuilderというライブラリを開発している方がいらっしゃったので、興味がある人は。  
