namespace KeyInputViewr.Core

/// マウスボタン
type MouseButtons =
    ///マウス ボタンは押されていません。
    | [<Literal>]None = 0
    ///マウスの左ボタンが押されました。
    | [<Literal>]Left = 1
    ///マウスの右ボタンが押されました。
    | [<Literal>]Right = 2
    ///マウスの中央ボタンが押されました。
    | [<Literal>]Middle = 4
    ///1 番目の XButton が押されました。
    | [<Literal>]XButton1 = 8
    ///2 番目の XButton が押されました。
    | [<Literal>]XButton2 = 16

/// マウスホイール
type MouseWheel =
    /// ホイールなし
    | [<Literal>]None = 0
    /// 上にホイール
    | [<Literal>]Top = 1
    /// 右にホイール
    | [<Literal>]Rigth = 2
    /// 下にホイール
    | [<Literal>]Bottom = 4
    /// 左にホイール
    | [<Literal>]Left = 8
