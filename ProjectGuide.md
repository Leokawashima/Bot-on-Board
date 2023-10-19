# Bot on Board Project Guide
* * *
## プロジェクトを正常に開いて使用するために
#### バージョンについて  
 Unity 2022 2.19fでの制作を行っています。  
 他のバージョンでの実行はエラー、クラッシュ、その他不具合を引き起こす可能性があります。  
#### ShaderGraphSetting
 The ShaderGraph Variant Limit is currently set to a value of 128...  
 というwarningが発生することがあります。  
 これはプロジェクトデータ以外のEditorの環境設定に起因するものになり、手動で修正が必要です。  
  1. 画面左上から`Edit`を選択  
  2. 下側にある`Preferences...`を選択  
  3. 下側にある`Shader Graph`を選択  
  4. `Shader Variant Limit` を `256` に変更
<details><summary>注意事項</summary><div>
  
  数値を入れてそのままカーソルで×で閉じるなどをすると  
  デフォルト値128から変更されない場合があります。  
  必ず数値を入力したら`Enter`で入力を終えてください。そのあとは閉じて大丈夫です。  
</div></details>

#### 手動で導入が必要なアセット
 このプロジェクトではProPixelizerという有料のアセットを導入しています。  
 もしProPixelizerをお持ちの方は`Assets/Downloads`の中にProPixelizerという空のフォルダがありますので、  
 そのフォルダを置き換える形で導入していただくとシェーダーを使用しているマテリアルが正常に動作します。  
 もしお持ちでない方は残念ながら有料のアセットであるためお渡しすることはできません。  
 しかしながら、見た目は変わるもののピンク一色に表示される状態は解消する方法が存在します。  
 1. `Assets/Materials/Pixel`にあるマテリアルを全選択  
 2. InspectorからShaderを`Universal Render Pipeline`を選択  
 3. `Lit`に変更
    
 これで見た目は実行ファイルと変わりますが、ピンク一色に表示される状況は改善されます。  
 使用しているProPixelizerのフォルダにはPRO_PIXELIZERシンボルをしこんでいるので、
 フォルダが存在するのにシェーダーを切り替えている場合はエラーログが流れますが、  
 単純にフォルダが存在しない場合はエラーが出ることなく正常に動作すると思われます。  
