//namespace KeyInputViewr
//F# の詳細 (http://fsharp.net)

open KeyInputViewr.Core
open System
open System.Windows.Forms

printfn "キーフックスタート"

// フィルターの追加
GlobalHook.Filter <-
    Some
        <| new HookFilterEvent(fun () -> [HookFilter.ProcessName "devenv"])
// イベントの追加
GlobalHook.KeyMouseEvent.Add(fun x ->
    printfn "Key: %A" x.Key
    printfn "KeyBit: %A" x.KeyBit
    printfn "Mouse: %A" x.Mouse
    printfn "Wheel: %A" x.Wheel
    printfn ""
)

#if COMPILED
[<STAThread>]
do
    System.Windows.Forms.Application.Run()
#endif
