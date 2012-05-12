//namespace KeyInputViewr
//F# の詳細 (http://fsharp.net)

open KeyInputViewr
open KeyInputViewr.Core
open System
open System.Windows

let app = new Application()
let wpf = new KeyboardMouseViewrWPF()

printfn "キーフックスタート"
// フィルターの追加
GlobalHook.Filter <- None
//    Some
//        <| new HookFilterEvent(fun () -> [HookFilter.ProcessName "devenv"])

// イベントの追加
GlobalHook.KeyMouseEvent.Add(fun x ->
//    printfn "Key: %A" x.Key
//    printfn "ScanCode: %A" x.ScanCode
////    printfn "KeyBit: %A" x.KeyBit
////    printfn "Mouse: %A" x.Mouse
////    printfn "Wheel: %A" x.Wheel
//    printfn ""

    // すべてのキーマップと押されたキーのマップをチェックして、
    // 表示 / 非表示を切り替える。
    wpf.Keys
    |> Seq.iter(fun (n,c) ->
        c.Style <- wpf.GetButtonStyle x.KeyBit.[n])
)

#if COMPILED
[<STAThread>]
do
    app.Run(wpf.Base) |> ignore
#endif
