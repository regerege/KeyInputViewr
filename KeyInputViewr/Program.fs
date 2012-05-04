//namespace KeyInputViewr
//F# の詳細 (http://fsharp.net)

open KeyInputViewr.Core
open System
open System.Windows.Forms

printfn "キーフックスタート"
GlobalHook.KeyDown.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "KeyDown[%s]: %A" pn e.KeyCode
    printfn "Shift: %A" e.Shift
    printfn "Alt: %A" e.Alt
    printfn "Control: %A" e.Control
)
GlobalHook.KeyPress.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "KeyPress[%s]: %A" pn e.KeyChar
)
GlobalHook.KeyUp.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "KeyUp[%s]: %A" pn e.KeyCode
    printfn "Shift: %A" e.Shift
    printfn "Alt: %A" e.Alt
    printfn "Control: %A" e.Control
)
GlobalHook.MouseDown.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "MouseDown[%s]: %A" pn e.Button
)
//GlobalHook.MouseMove.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
//    printfn "MouseMove[%s]: %A" pn e
//)
GlobalHook.MouseUp.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "MouseUp[%s]: %A" pn e.Button
)
GlobalHook.MouseHWheel.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "MouseHWheel[%s]: %A" pn e.Delta
)
GlobalHook.MouseWWheel.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "MouseWWheel[%s]: %A" pn e.Delta
)
GlobalHook.MouseDoubleClick.Add(fun e ->
//    let pn = GlobalHook.GetForegroundWindowText()
    let pn = ""
    printfn "MouseDoubleClick[%s]: %A" pn e.Button
)

#if COMPILED
[<STAThread>]
do
    Application.Run()
#endif
