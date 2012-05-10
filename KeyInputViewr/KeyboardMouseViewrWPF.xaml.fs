namespace KeyInputViewr

open KeyInputViewr.Core
open System
open System.IO
open System.Windows
open System.Drawing
open System.Windows.Markup
open System.Windows.Shapes

type KeyboardMouseViewrWPF() as w =
    inherit Window()

    let _uri = new Uri("KeyboardMouseViewrWPF.xaml", UriKind.Relative)
    let _base = Application.LoadComponent(_uri) :?> Window
    
//#region キーマッピング関連
    let getKeyName key = sprintf "rect_%A" <| enum<Keys> key
    let getRectangle key = _base.FindName(getKeyName key) :?> Rectangle
    let seqKeys =
        Seq.cache <| Seq.init 0xFF (fun x -> (x, getRectangle x))
//#endregion

    do
        w.Init()

    member w.Init() =
        _base.MouseLeftButtonDown.Add(fun _ -> _base.DragMove())

    member w.Base = _base
    member w.Keys = seqKeys
