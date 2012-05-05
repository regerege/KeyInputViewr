namespace KeyInputViewr.Core

(*
.NET向けのキーフックモジュール

■参考サイト
http://azumaya.s101.xrea.com/wiki/index.php?%B3%D0%BD%F1%2FC%A2%F4%2F%A5%B0%A5%ED%A1%BC%A5%D0%A5%EB%A5%D5%A5%C3%A5%AF
http://www.tech-archive.net/Archive/InetSDK/microsoft.public.inetsdk.programming.webbrowser_ctl/2006-04/msg00043.html
http://hiroakishibuki.wordpress.com/2007/11/16/lowlevelmousehook-%E3%82%B3%E3%83%B3%E3%83%9D%E3%83%BC%E3%83%8D%E3%83%B3%E3%83%88/
*)

open System
open System.Diagnostics
open System.Windows.Forms
open System.Text

/// キーとマウスのフック
module GlobalHook =
    /// Keyboardフックポインタ
    let mutable private k_hook = IntPtr.Zero
    /// Mouseフックポインタ
    let mutable private m_hook = IntPtr.Zero

///#region キーボードフック関連
    ///KeyDownイベント
    let private _keyDownEvent = new Event<KeyEventArgs>()
    ///KeyPressイベント
    let private _keyPressEvent = new Event<KeyPressEventArgs>()
    ///KeyUpイベント
    let private _keyUpEvent = new Event<KeyEventArgs>()
    ///キーが押されると発生します。
    let KeyDown = _keyDownEvent.Publish
    ///キーが押されると発生します。
    let KeyPress = _keyPressEvent.Publish
    ///キーが離されると発生します。
    let KeyUp = _keyUpEvent.Publish

    ///キーボードフックのコールバック関数
    let private KeyboardHookProc nCode (wParam:nativeint) lParam =
//        let keydownobj = box KeyDown
//        let keypressobj = box KeyPress
//        let keyupobj = box KeyUp
//        if 0 <= nCode && (keydownobj <> null || keypressobj <> null || keyupobj <> null) then
        if nCode = 0 then
            let key = WindowsAPI.toKBDLLHOOKSTRUCT lParam
            let msg = wParam.ToInt32()
            Debug.WriteLine <| sprintf "msg: %d" msg
            Debug.WriteLine <| sprintf "vkCode: %d" key.vkCode
            Debug.WriteLine <| sprintf "scanCode: %d" key.scanCode
            Debug.WriteLine <| sprintf "flags: %d" key.flags
            
            // KeyDownイベントを発行
            if msg = WindowsAPI.WM_KEYDOWN || msg = WindowsAPI.WM_SYSKEYDOWN then
                let keys = unbox<Keys>(int key.vkCode)
                let e = new KeyEventArgs(keys)
                Debug.WriteLine <| sprintf "KeyboardHook[down]: %A" e.KeyCode
                _keyDownEvent.Trigger e
            // KeyPressイベントを発行
            if msg = WindowsAPI.WM_KEYDOWN then
                let mutable keyState:byte[] = Array.zeroCreate 256
                let mutable inBuffer:byte[] = Array.zeroCreate 2
                let ascii = WindowsAPI.ToAscii(key.vkCode, key.scanCode, keyState, inBuffer, key.flags)
                if ascii then
                    let e = new KeyPressEventArgs(char inBuffer.[0])
                    Debug.WriteLine <| sprintf "KeyboardHook[press]: %A" e
                    _keyPressEvent.Trigger e
            // KeyUpイベントを発行
            if msg = WindowsAPI.WM_KEYUP then
                let keys = unbox<Keys>(int key.vkCode)
                let e = new KeyEventArgs(keys)
                Debug.WriteLine <| sprintf "KeyboardHook[up]: %A" e
                _keyUpEvent.Trigger e

            Debug.WriteLine <| new String('-', 50)
            Debug.WriteLine ""
        WindowsAPI.CallNextHookEx(k_hook, nCode, wParam, lParam)
    let private keyboardHook = WindowsAPI.LowLevelProc(KeyboardHookProc)
///#endregion

///#region マウスフック関連
    ///MouseDownイベント
    let private _mouseDownEvent = new Event<MouseEventArgs>();
    ///MouseMoveイベント
    let private _mouseMoveEvent = new Event<MouseEventArgs>();
    ///MouseDownイベント
    let private _mouseUpEvent = new Event<MouseEventArgs>();
    ///MouseWheelイベント
    let private _mouseHWheelEvent = new Event<MouseEventArgs>();
    ///MouseWheelイベント
    let private _mouseWWheelEvent = new Event<MouseEventArgs>();
    ///MouseDoubleClickイベント
    let private _mouseDoubleClickEvent = new Event<MouseEventArgs>();
    ///マウス ボタンがクリックされると発生します。
    let MouseDown = _mouseDownEvent.Publish
    ///マウス ボタンが離されると発生します。
    let MouseMove = _mouseMoveEvent.Publish
    ///マウス ボタンが離されると発生します。
    let MouseUp = _mouseUpEvent.Publish
    ///マウス ホイールが動くと発生します。
    let MouseHWheel = _mouseHWheelEvent.Publish
    ///マウス ホイールが動くと発生します。
    let MouseWWheel = _mouseWWheelEvent.Publish
    ///マウス ボタンがダブルクリックされると発生します。
    let MouseDoubleClick = _mouseDoubleClickEvent.Publish

    ///マウスのコールバック関数
    let private MouseHookProc nCode (wParam:nativeint) lParam =
        if nCode = 0 then
            let mouse = WindowsAPI.toMSLLHOOKSTRUCT lParam
            let mea mb c =
                new MouseEventArgs(
                    mb,
                    c,
                    mouse.pt.X,
                    mouse.pt.Y,
                    int mouse.mouseData >>> 16
                )
            let wp = wParam.ToInt32()
            let e =
                    match wp with
                    | WindowsAPI.WM_LBUTTONDOWN
                    | WindowsAPI.WM_LBUTTONUP
                    | WindowsAPI.WM_LBUTTONDBLCLK -> mea MouseButtons.Left 1
                    | WindowsAPI.WM_RBUTTONDOWN
                    | WindowsAPI.WM_RBUTTONUP
                    | WindowsAPI.WM_RBUTTONDBLCLK -> mea MouseButtons.Right 1
                    | WindowsAPI.WM_MBUTTONDOWN
                    | WindowsAPI.WM_MBUTTONUP
                    | WindowsAPI.WM_MBUTTONDBLCLK -> mea MouseButtons.Middle 1
                    | WindowsAPI.WM_XBUTTONDOWN
                    | WindowsAPI.WM_XBUTTONUP
                    | WindowsAPI.WM_XBUTTONDBLCLK ->
                        match mouse.mouseData with
                        | 131072u -> mea MouseButtons.XButton1 1
                        | 65536u -> mea MouseButtons.XButton2 1
                        | _ -> mea MouseButtons.None 1
                    | _ -> mea MouseButtons.None 0
//            Debug.WriteLine <| sprintf "MouseButton: %A" e.Button
//            Debug.WriteLine <| sprintf "MouseMsg: %A" wp
//            if wp = WindowsAPI.WM_XBUTTONDOWN || wp = WindowsAPI.WM_XBUTTONUP then
//                printfn "mouseData: %A" mouse.mouseData
//                printfn "flags: %A" mouse.flags
////                printfn "MouseMsg: %A" wp
            match wp with
            | WindowsAPI.WM_LBUTTONDOWN
            | WindowsAPI.WM_RBUTTONDOWN
            | WindowsAPI.WM_MBUTTONDOWN
            | WindowsAPI.WM_XBUTTONDOWN -> _mouseDownEvent.Trigger e
            | WindowsAPI.WM_LBUTTONUP
            | WindowsAPI.WM_RBUTTONUP
            | WindowsAPI.WM_MBUTTONUP
            | WindowsAPI.WM_XBUTTONUP -> _mouseUpEvent.Trigger e
            | WindowsAPI.WM_LBUTTONDBLCLK
            | WindowsAPI.WM_RBUTTONDBLCLK
            | WindowsAPI.WM_MBUTTONDBLCLK
            | WindowsAPI.WM_XBUTTONDBLCLK -> _mouseDoubleClickEvent.Trigger e
            | WindowsAPI.WM_MOUSEHWHEEL -> _mouseHWheelEvent.Trigger e
            | WindowsAPI.WM_MOUSEWHEEL -> _mouseWWheelEvent.Trigger e
            | _ -> _mouseMoveEvent.Trigger e
        WindowsAPI.CallNextHookEx(m_hook, nCode, wParam, lParam)
    let private mouseHook = WindowsAPI.LowLevelProc(MouseHookProc)
///#endregion

//#region キーフックを実行
    do
        let hmod = WindowsAPI.GetModule()
        k_hook <- WindowsAPI.SetWindowsHookEx(
            WindowsAPI.WH_KEYBOARD_LL,
            keyboardHook,
            hmod,
            0u)
        WindowsAPI.failwithWin32 k_hook
        m_hook <- WindowsAPI.SetWindowsHookEx(
            WindowsAPI.WH_MOUSE_LL,
            mouseHook,
            hmod,
            0u)
        WindowsAPI.failwithWin32 m_hook
        AppDomain.CurrentDomain.DomainUnload.Add(fun _ ->
            let unhook instance =
                if instance <> IntPtr.Zero then
                    WindowsAPI.UnhookWindowsHookEx instance |> ignore
            [k_hook;m_hook] |> List.iter unhook
            GC.Collect()
        )
//#endregion

//#region フォアグラウンド、プロセス、タイトル
    /// フォアグラウンドのウィンドウタイトル名を取得
    let GetForegroundWindowText() =
        let hwnd = WindowsAPI.GetForegroundWindow()
        match WindowsAPI.GetWindowTextLength(hwnd) with
        | 0 -> ""
        | len ->
            let sb = new StringBuilder(len)
            WindowsAPI.GetWindowText(hwnd, sb, len) |> ignore
            sb.ToString()
            
    /// フォアグラウンドのプロセスを取得
    let GetForegroundProcess() =
        let mutable pid = 0
        let hwnd = WindowsAPI.GetForegroundWindow()
        let tid = WindowsAPI.GetWindowThreadProcessId(hwnd, &pid)
        Process.GetProcessById(pid)
//#endregion
