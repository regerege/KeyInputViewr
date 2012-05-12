namespace KeyInputViewr

open KeyInputViewr.Core
open System
open System.Windows
open System.Windows.Controls

type KeyboardMouseViewrWPF() as w =
    inherit Window()

    let _uri = new Uri("KeyboardMouseViewrWPF.xaml", UriKind.Relative)
    let _base = Application.LoadComponent(_uri) :?> Window
    
    let getStyle (nm:string) = _base.Resources.[nm] :?> Style
    let _defaultStyle = getStyle "_BorderStyle"
    let _pushedStyle = getStyle "_BorderStylePushed"

//#region キーマッピング関連
    let getKeyName key = sprintf "b_%A" <| enum<Keys> key
    let getControl key = _base.FindName(getKeyName key) :?> Border
    let seqKeys =
        Seq.cache <|
            Seq.init 0xFF (((+)1) >> (fun x -> (x, getControl x)))
            |> Seq.choose (fun t ->
                if (snd t) <> null then Some t
                else None
            )
//#endregion

    do
        _base.MouseLeftButtonDown.Add(fun _ -> _base.DragMove())

    member w.Base = _base
    member w.Keys = seqKeys
    member w.GetButtonStyle b = if b then _pushedStyle else _defaultStyle