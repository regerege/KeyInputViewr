namespace KeyInputViewr.Core
#nowarn "9"

open Microsoft.FSharp.NativeInterop
open System
open System.ComponentModel
open System.Diagnostics
open System.Runtime.InteropServices
open System.Text

(*
■参考サイト
P/Invoke
    http://msdn.microsoft.com/en-us/magazine/cc164123.aspx
    http://stackoverflow.com/questions/5682927/how-can-i-pass-an-f-delegate-to-a-p-invoke-method-expecting-a-function-pointer
    http://stackoverflow.com/questions/1689460/f-syntax-for-p-invoke-signature-using-marshalas
Marsharing
    http://d.hatena.ne.jp/Schima/20090514
    http://stackoverflow.com/questions/7267542/how-to-use-structuretoptr-with-f-structure-typeof-trouble
KeyHook
    http://ceoth.blog16.fc2.com/blog-entry-78.html
    http://azumaya.s101.xrea.com/wiki/index.php?%B3%D0%BD%F1%2FC%A2%F4%2F%A5%B0%A5%ED%A1%BC%A5%D0%A5%EB%A5%D5%A5%C3%A5%AF
    http://msdn.microsoft.com/ja-jp/library/ms997649.aspx
    http://stackoverflow.com/questions/5682927/how-can-i-pass-an-f-delegate-to-a-p-invoke-method-expecting-a-function-pointer
Structure
    http://msdn.microsoft.com/en-us/library/ms644967%28VS.85%29.aspx
    http://msdn.microsoft.com/ja-jp/library/windows/desktop/ms644967(v=vs.85).aspx
    http://msdn.microsoft.com/ja-jp/library/windows/desktop/ms644970(v=vs.85).aspx
KeyCode
    http://msdn.microsoft.com/ja-jp/library/system.windows.forms.keys.aspx
*)

/// KeyHookに関するWindowsAPIの定義
module internal WindowsAPI =
//#region 構造体
    /// キーボードフック情報
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type KBDLLHOOKSTRUCT =
        ///仮想キーコード
        val vkCode      : uint32
        ///スキャンコード (使い方不明)
        val scanCode    : uint32
        ///特殊キー
        val flags       : uint32
        ///不明
        val time        : uint32
        ///メッセージの拡張情報らしい
        val dwExtraInfo : nativeint
    
    /// マウスポインタのスクリーン座標
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type POINT =
        val X : int32
        val Y : int32
    /// マウスフック情報
    [<Struct; StructLayout(LayoutKind.Sequential)>]
    type MSLLHOOKSTRUCT =
        ///スクリーン座標
        val pt          : POINT
        ///マウスキー情報
        val mouseData   : uint32
        ///特殊キー
        val flags       : uint32
        ///不明
        val time        : uint32
        ///メッセージの拡張情報らしい
        val dwExtraInfo : nativeint
//#endregion
    
//#region コールバック関数
    /// <summary>
    /// SetWindowsHookEx 関数と共に使われる、アプリケーション定義またはライブラリ定義のコールバック関数です。新しいキーボード入力イベントをスレッドの入力キューへポストしようとするときに、システムは必ずこのフックプロシージャを呼び出します。キーボード入力を発生させるのは、ローカルのキーボードドライバまたは、 関数です。keybd_event を呼び出した場合、キー入力がエミュレートされ、特定のキーを押したのと同様のイベントが発生します。
    /// HOOKPROC 型は、このコールバック関数へのポインタを定義します。LowLevelKeyboardProc or LowLevelMouseProc はアプリケーション定義またはライブラリ定義の関数名のプレースホルダであり、実際にこの関数名を使う必要はありません。
    /// </summary>
    /// <param name="nCode">nCode パラメータの値が 0 未満の場合、このフックプロシージャはメッセージを処理せずにそのメッセージを CallNextHookEx 関数へ渡し、その関数の戻り値を返さなければなりません。</param>
    /// <param name="wParam">メッセージの識別子を指定します。このパラメータで、、、、 のいずれかのメッセージを指定します。</param>
    /// <param name="lParam">1個の構造体へのポインタを指定します。</param>
    /// <returns>
    /// nCode パラメータの値が 0 未満の場合、このフックプロシージャは CallNextHookEx 関数を呼び出し、その関数の戻り値を返さなければなりません。
    /// nCode パラメータの値が 0 以上で、このフックプロシージャがメッセージを処理しなかった場合も、CallNextHookEx 関数を呼び出し、その関数の戻り値を返すことを強く推奨します。CallNextHookEx 関数を呼び出さないと、WH_KEYBOARD_LL フックをインストールしたほかのアプリケーションがフックの通知を受け取れず、誤動作する可能性があります。nCode パラメータの値が 0 以上で、このフックプロシージャがメッセージを処理した場合、0 以外の値を返すと、フックチェーン内の残りのフックプロシージャや目的のウィンドウプロシージャへメッセージを渡すことを防止できます。
    /// </returns>
//    [<UnmanagedFunctionPointerAttribute(CallingConvention.Cdecl)>]
    type LowLevelProc = delegate of int * nativeint * nativeint -> nativeint
//#endregion

//#region WindowsAPIのP/Invoke
    /// <summary>呼び出し側プロセスのアドレス空間に該当ファイルがマップされている場合、指定されたモジュール名のモジュールハンドルを返します。</summary>
    /// <param name="lpModuleName">
    /// ［入力］モジュール（.DLL または .EXE ファイル）の名前を保持する、NULL で終わる文字列へのポインタを指定します。拡張子を記述しなかった場合は、既定で「.DLL」が追加されます。文字列の最後に「.」を記述すると、拡張子なしのモジュール名になります。文字列には、パスを指定しなくてもかまいません。パスを指定する場合は、スラッシュ（/）ではなく円記号（\）で区切ってください。指定されたモジュール名を、呼び出し側プロセスのアドレス空間に現在マップされているモジュール名と比較します（ 大文字と小文字を区別しません）。
    /// NULL を指定すると、呼び出し側プロセスの作成に使われたファイルのハンドルが返ります。
    /// </param>
    /// <returns>
    /// 関数が成功すると、フックプロシージャのハンドルが返ります。
    /// 関数が失敗すると、NULL が返ります。拡張エラー情報を取得するには、 関数を使います。
    /// </returns>
    [<DllImport("kernel32.dll", SetLastError = true)>]
    extern nativeint internal GetModuleHandle(string lpModuleName)
    
    /// <summary>アプリケーション定義のフックプロシージャをフックチェーン内にインストールします。フックプロシージャをインストールすると、特定のイベントタイプを監視できます。監視の対象になるイベントは、特定のスレッド、または呼び出し側スレッドと同じデスクトップ内のすべてのスレッドに関連付けられているものです。</summary>
    /// <param name="idHook">インストール対象のフックタイプを指定します。次の値のいずれかを指定します。</param>
    /// <param name="lpfn">フックプロシージャへのポインタを指定します。dwThreadID パラメータで 0、またはほかのプロセスが作成したスレッドの識別子を指定した場合、lpfn パラメータで、ダイナミックリンクライブラリ（DLL）内に存在するフックプロシージャへのポインタを指定しなければなりません。それ以外の場合、現在のプロセスに関連付けられているコード内に存在するフックプロシージャへのポインタを指定できます。</param>
    /// <param name="hMod">lpfn パラメータが指すフックプロシージャを保持している DLL のハンドルを指定します。dwThreadId パラメータが、現在のプロセスが作成したスレッドを指定していて、フックプロシージャが現在のプロセスに関連付けられているコード内に存在する場合、hMod パラメータで NULL を指定しなければなりません。</param>
    /// <param name="dwThreadId">フックプロシージャを関連付けるべきスレッドの識別子を指定します。0 を指定すると、フックプロシージャは、呼び出し側スレッドと同じデスクトップ内で動作している既存のすべてのスレッドに関連付けられます。</param>
    /// <returns>
    /// 関数が成功すると、フックプロシージャのハンドルが返ります。
    /// 関数が失敗すると、NULL が返ります。拡張エラー情報を取得するには、 関数を使います。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern nativeint SetWindowsHookEx(int idHook, LowLevelProc lpfn, nativeint hMod, uint32 dwThreadId)
    
    /// <summary>現在のフックチェーン内の次のフックプロシージャに、フック情報を渡します。フックプロシージャは、フック情報を処理する前でも、フック情報を処理した後でも、この関数を呼び出せます。</summary>
    /// <param name="hhk">現在のフックのハンドルを指定します。アプリケーションは、SetWindowsHookEx 関数を呼び出した際に取得したハンドルを指定します。</param>
    /// <param name="nCode">現在のフックプロシージャに渡されたフックコードを指定します。次のフックプロシージャはこのフックコードを使って、フック情報の処理方法を決定します。</param>
    /// <param name="wParam">現在のフックプロシージャに渡された wParam 値を指定します。このパラメータの意味は、現在のフックチェーンに関連付けられているフックタイプに依存します。</param>
    /// <param name="lParam">現在のフックプロシージャに渡された lParam 値を指定します。このパラメータの意味は、現在のフックチェーンに関連付けられているフックタイプに依存します。</param>
    /// <returns>
    /// 関数が成功すると、0 以外の値が返ります。
    /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、関数を使います。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern nativeint CallNextHookEx(nativeint hhk, int32 nCode, nativeint wParam, nativeint lParam)

    /// <summary>SetWindowsHookEx 関数を使ってフックチェーン内にインストールされたフックプロシージャを削除します。</summary>
    /// <param name="hhk">
    /// 削除対象のフックプロシージャのハンドルを指定します。
    /// このハンドルは、以前に SetWindowsHookEx 関数を呼び出したときに取得したフックのハンドルです。</param>
    /// <returns>
    /// 関数が成功すると、0 以外の値が返ります。
    /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、関数を使います。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern bool UnhookWindowsHookEx(nativeint hhk)
    
    /// <summary>
    /// フォアグラウンドウィンドウ（ 現在ユーザーが作業しているウィンドウ）のハンドルを返します。
    /// Windows システムは、フォアグラウンドウィンドウを生成したスレッドに対して、
    /// 他のスレッドよりも若干高い優先順位を割り当てます。
    /// </summary>
    /// <returns>
    /// フォアグラウンドウィンドウのハンドルが返ります。
    /// フォアグラウンドウィンドウのハンドルは、
    /// ウィンドウがフォーカスを失ったなどの特定の状況下で NULL になる場合もあります。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern nativeint GetForegroundWindow()

    /// <summary>指定されたウィンドウのタイトルバーのテキストをバッファへコピーします。指定されたウィンドウがコントロールの場合は、コントロールのテキストをコピーします。ただし、他のアプリケーションのコントロールのテキストを取得することはできません。</summary>
    /// <param name="hWnd">ウィンドウ（ またはテキストを持つコントロール）のハンドルを指定します。</param>
    /// <param name="lpString">バッファへのポインタを指定します。このバッファにテキストが格納されます。</param>
    /// <param name="nMaxCount">バッファにコピーする文字の最大数を指定します。テキストのこのサイズを超える部分は、切り捨てられます。NULL 文字も数に含められます。</param>
    /// <returns>
    /// 関数が成功すると、コピーされた文字列の文字数が返ります（ 終端の NULL 文字は含められません）。タイトルバーやテキストがない場合、タイトルバーが空の場合、および hWnd パラメータに指定したウィンドウハンドルまたはコントロールハンドルが無効な場合は 0 が返ります。拡張エラー情報を取得するには、 関数を使います。
    /// 他のアプリケーションのエディットコントロールのテキストをこの関数で取得することはできません。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)>]
    extern int GetWindowText(nativeint hWnd, StringBuilder lpString, int nMaxCount)

    /// <summary>指定されたウィンドウのタイトルバーテキストの文字数を返します（ そのウィンドウがタイトルバーを持つ場合）。指定したウィンドウがコントロールの場合は、コントロール内のテキストの文字数を返します。ただし、GetWindowTextLength 関数で他のアプリケーションのエディットコントロールのテキストの長さを取得することはできません。</summary>
    /// <param name="hWnd">ウィンドウ（ またはテキストを持つコントロール）のハンドルを指定します。</param>
    /// <returns>
    /// 関数が成功すると、テキストの文字数が返ります。特定の条件の下では、実際のテキスト長よりも大きくなります。詳細については、解説を参照してください。
    /// ウィンドウがテキストを持たない場合は、0 が返ります。拡張エラー情報を取得するには、 関数を使います。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern int GetWindowTextLength(nativeint hWnd)

    /// <summary>
    /// 指定されたウィンドウを作成したスレッドの ID を取得します。
    /// 必要であれば、ウィンドウを作成したプロセスの ID も取得できます。
    /// </summary>
    /// <param name="hWnd">ウィンドウのハンドルを指定します。</param>
    /// <param name="lpdwProcessId">
    /// プロセス ID を受け取る変数へのポインタを指定します。
    /// ポインタを指定すると、それが指す変数にプロセス ID がコピーされます。
    /// NULL を指定した場合は、プロセス ID の取得は行われません。
    /// </param>
    /// <returns>ウィンドウを作成したスレッドの ID が返ります。</returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern int GetWindowThreadProcessId(nativeint hWnd, int32& lpdwProcessId)

    /// <summary>256 個の仮想キーの状態を、指定されたバッファへコピーします。</summary>
    /// <param name="lpKeyState">［入力］すべての仮想キーの状態を保持する 256 バイトの配列へのポインタを指定します。</param>
    /// <returns>
    /// 関数が成功すると、0 以外の値が返ります。
    /// 関数が失敗すると、0 が返ります。拡張エラー情報を取得するには、関数を使います。
    /// </returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern bool GetKeyboardState(byte[] lpKeyState)

    /// <summary>
    /// 指定された仮想キーコードおよびキーボード状態を、対応する単数または複数の文字に変換します。関数は、与えられたキーボードレイアウトハンドルで識別される入力言語と物理キーボードレイアウトを使い、コードを変換します。
    /// 与えられたコードを変換するために使うキーボードレイアウトのハンドルを指定するには、ToAsciiEx 関数を使います。
    /// </summary>
    /// <param name="uVirtKey">変換する仮想キーコードを指定します。</param>
    /// <param name="uScanCode">変換するキーのハードウェアスキャンコードを指定します。キーが上がっている（押されていない）場合、この値の上位ビットがセットされます。</param>
    /// <param name="lpKeyState">
    /// 現在のキーボード状態が入る、256 バイトの配列へのポインタを指定します。配列の各要素（バイト）には、１つのキーの状態が入ります。バイトの上位ビットがセットされている場合、キーは下がって（押されて）います。
    /// 下位ビットがセットされている場合、キーはオンに切り替えられていることを表します。この関数では、CAPS LOCK キーのトグルビットだけが有効です。NUM LOCK キーおよび SCROLL LOCK キーのトグル状態は無視されます。
    /// </param>
    /// <param name="lpChar">変換された単数または複数の文字を受けるバッファへのポインタを指定します。</param>
    /// <param name="uFlags">メニューがアクティブかどうかを指定します。このパラメータには、メニューがアクティブの場合は 1、アクティブでない場合は 0 を指定してください。</param>
    /// <returns>指定されたキーがデッドキーの場合、負の値が戻ります。それ以外の場合、次の値のいずれかが返ります。</returns>
    [<DllImport("user32.dll", SetLastError = true)>]
    extern bool ToAscii(uint32 uVirtKey, uint32 uScanCode, byte[] lpKeyState, byte[] lpChar, uint32 flags)
//#endregion

//#region ポインタから構造体へのマーシャリング
    /// <summary>nativeintを構造体に変換</summary>
    /// <remarks>最も遅いやり方</remarks>
    /// <typeparam name="'T">構造体の型</typeparam>
    /// <param name="ptr">構造体のアンマネージポインタ</param>
    let inline toStructure<'T> (ptr:nativeint) =
        Marshal.PtrToStructure(ptr, typeof<'T>)
        
    /// <summary>nativeintをKBDLLHOOKSTRUCTに変換</summary>
    /// <param name="ptr">構造体のアンマネージポインタ</param>
    let inline toKBDLLHOOKSTRUCT (ptr:nativeint) =
        toStructure<KBDLLHOOKSTRUCT>(ptr) :?> KBDLLHOOKSTRUCT
        
    /// <summary>nativeintをKBDLLHOOKSTRUCTに変換</summary>
    /// <param name="ptr">構造体のアンマネージポインタ</param>
    let inline toMSLLHOOKSTRUCT (ptr:nativeint) =
        toStructure<MSLLHOOKSTRUCT>(ptr) :?> MSLLHOOKSTRUCT
//#endregion

//#region 便利関数？？
    /// <summary>指定したモジュールのインスタンスハンドルを返します。</summary>
    /// <param name="t">型</param>
    let inline GetModule() =
        System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName
        |> GetModuleHandle

    let inline failwithWin32 (instance:nativeint) =
        if instance = IntPtr.Zero then
            let code = Marshal.GetLastWin32Error()
            raise <| new Win32Exception(code)
//#endregion

///#region メッセージ定義

    ///#region Hook Codes
    let [<Literal>]WH_KEYBOARD_LL   = 13
    let [<Literal>]WH_MOUSE_LL      = 14
    ///#endregion

    ///#region Hook Codes
    let [<Literal>]HC_ACTION        = 0
    let [<Literal>]HC_GETNEXT       = 1
    let [<Literal>]HC_SKIP          = 2
    let [<Literal>]HC_NOREMOVE      = 3
    let [<Literal>]HC_NOREM         = HC_NOREMOVE
    let [<Literal>]HC_SYSMODALON    = 4
    let [<Literal>]HC_SYSMODALOFF   = 5
    ///#endregion

    ///#region キーメッセージ
    let [<Literal>]WM_KEYFIRST      = 0x0100
    let [<Literal>]WM_KEYDOWN       = 0x0100
    let [<Literal>]WM_KEYUP         = 0x0101
    let [<Literal>]WM_CHAR          = 0x0102
    let [<Literal>]WM_DEADCHAR      = 0x0103
    let [<Literal>]WM_SYSKEYDOWN    = 0x0104
    let [<Literal>]WM_SYSKEYUP      = 0x0105
    let [<Literal>]WM_SYSCHAR       = 0x0106
    let [<Literal>]WM_SYSDEADCHAR   = 0x0107
    ///#endregion

    ///#region マウスメッセージ
    let [<Literal>]WM_MOUSEFIRST    = 0x0200
    let [<Literal>]WM_MOUSEMOVE     = 0x0200
    let [<Literal>]WM_LBUTTONDOWN   = 0x0201
    let [<Literal>]WM_LBUTTONUP     = 0x0202
    let [<Literal>]WM_LBUTTONDBLCLK = 0x0203
    let [<Literal>]WM_RBUTTONDOWN   = 0x0204
    let [<Literal>]WM_RBUTTONUP     = 0x0205
    let [<Literal>]WM_RBUTTONDBLCLK = 0x0206
    let [<Literal>]WM_MBUTTONDOWN   = 0x0207
    let [<Literal>]WM_MBUTTONUP     = 0x0208
    let [<Literal>]WM_MBUTTONDBLCLK = 0x0209
    let [<Literal>]WM_MOUSEWHEEL    = 0x020A
    let [<Literal>]WM_XBUTTONDOWN   = 0x020B
    let [<Literal>]WM_XBUTTONUP     = 0x020C
    let [<Literal>]WM_XBUTTONDBLCLK = 0x020D
    let [<Literal>]WM_MOUSEHWHEEL   = 0x020E
    ///#endregion
    
    ///#region マウス拡張キーメッセージ
    let [<Literal>]XBUTTON1 = 0x1
    let [<Literal>]XBUTTON2 = 0x2
    ///#endregion

///#endregion

//#region フォアグラウンド、プロセス、タイトル
    /// フォアグラウンドのウィンドウタイトル名を取得
    let GetForegroundWindowText() =
        let hwnd = GetForegroundWindow()
        match GetWindowTextLength(hwnd) with
        | 0 -> ""
        | len ->
            let sb = new StringBuilder(len)
            GetWindowText(hwnd, sb, len) |> ignore
            sb.ToString()
            
    /// フォアグラウンドのプロセスを取得
    let GetForegroundProcess() =
        let mutable pid = 0
        let hwnd = GetForegroundWindow()
        let tid = GetWindowThreadProcessId(hwnd, &pid)
        Process.GetProcessById(pid)
//#endregion
