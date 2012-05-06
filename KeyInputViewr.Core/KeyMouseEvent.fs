namespace KeyInputViewr.Core
open WindowsAPI

/// フックフィルター
type HookFilter =
/// プロセス名
| ProcessName of string
| WindowTitle of string
/// <summary>プロセス名またはウィンドウタイトルに完全一致したイベントを発行する。</summary>
/// <returns>フック対象のフィルター一覧を返す。</returns>
type HookFilterEvent = delegate of unit -> HookFilter list

type internal KeyMouseType =
| AddKey of Keys
| AddMouse of MouseButtons
| DelKey of Keys
| DelMouse of MouseButtons
| Wheel of MouseWheel

open System.Collections

/// キーマウスの押下情報
type KeyMouseEventArgs(key:Keys, mouse:MouseButtons, wheel:MouseWheel) =
    let mutable _key = key
    let mutable _mouse = mouse
    let mutable _wheel = wheel
    let _keybit = new BitArray(255)
    /// キーを取得する。
    member x.Key
        with get() = _key
//        and internal set v = _key <- v
    /// キーのビット表を取得する。
    member x.KeyBit
        with get() = _keybit
//        and internal set v = _keybit <- v
    /// マウスボタンを取得する。
    member x.Mouse
        with get() = _mouse
        and internal set v = _mouse <- v
    /// ホイールの回転数を取得する。
    member x.Wheel
        with get() = _wheel
        and internal set v = _wheel <- v
    new() = new KeyMouseEventArgs(Keys.None, MouseButtons.None, MouseWheel.None)
    member internal x.AddMouse v = _mouse <- _mouse ||| v
    member internal x.DelMouse v = _mouse <- _mouse ^^^ v
    member internal x.AddKey v =
        _key <- v
        _keybit.[int v] <- true
    member internal x.DelKey v =
        _key <- v
        _keybit.[int v] <- false
