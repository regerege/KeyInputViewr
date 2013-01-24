namespace KeyInputViewr

open KeyInputViewr.Core
open System
open System.Windows
open System.Windows.Controls

type KeyboardMouseViewrWPF() =
    inherit Window()

    let _uri = new Uri("KeyboardMouseViewrWPF.xaml", UriKind.Relative)
    let _base = Application.LoadComponent(_uri) :?> Window
    
    let getStyle (nm:string) = _base.Resources.[nm] :?> Style
    let _defaultStyle = getStyle "_BorderStyle"
    let _pushedStyle = getStyle "_BorderStylePushed"

    /// 汎用的なコントロール取得シーケンス
    let createSeq n f =
        Seq.cache <|
            Seq.init n (((+)1) >> (fun x -> (x, f x)))
            |> Seq.choose (fun t ->
                if (snd t) <> null then Some t
                else None
            )
    /// 汎用的にコントロールを取得する。
    let getCtrl nm f key = sprintf nm <| f key |> _base.FindName
//#region キーマッピング関連
    let seqKeys =
        createSeq 0xFF <| (fun (key:int32) -> getCtrl "b_%A" enum<Keys> key :?> Border)
//#endregion
//#region マウスマッピング関連
    let seqMouseButtons =
        createSeq 0x1E <| (fun (key:int32) -> getCtrl "img_b_%A" enum<MouseButtons> key :?> Image)
    let seqMouseWheels =
        createSeq 0x0E <| (fun (key:int32) -> getCtrl "img_h_%A" enum<MouseWheel> key :?> Image)
//#endregion

    do
        _base.MouseLeftButtonDown.Add(fun _ -> _base.DragMove())

    member w.Base = _base
    /// Keyboardのキーマッピング
    member w.Keys = seqKeys
    /// Mouseのキーマッピング
    member w.MouseButtons = seqMouseButtons
    /// MouseWheelマッピング
    member w.MouseWheel = seqMouseWheels
    /// MouseWheelのボタン
    member w.MouseWhellButton = _base.FindName("img_b_Middle") :?> Image

    /// コントロールのON/OFF判定時のStyleを取得する。
    member w.GetButtonStyle b = if b then _pushedStyle else _defaultStyle
    /// イメージのON/OFF判定時のVisibilityを取得する。
    member w.GetImageVisible b = if b then Visibility.Visible else Visibility.Hidden

