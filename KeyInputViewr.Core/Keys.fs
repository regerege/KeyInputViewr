namespace KeyInputViewr.Core

type Keys =
//    ///キー値から修飾子を抽出するビット マスク。
//    | [<Literal>]Modifiers = -65536
    ///キー入力なし
    | [<Literal>]None = 0
    ///マウスの左ボタン
    | [<Literal>]LButton = 1
    ///マウスの右ボタン
    | [<Literal>]RButton = 2
    ///Cancel キー
    | [<Literal>]Cancel = 3
    ///マウスの中央ボタン (3 ボタン マウスの場合)
    | [<Literal>]MButton = 4
    ///x マウスの 1 番目のボタン (5 ボタン マウスの場合)
    | [<Literal>]XButton1 = 5
    ///x マウスの 2 番目のボタン (5 ボタン マウスの場合)
    | [<Literal>]XButton2 = 6
    ///BackSpace キー
    | [<Literal>]Back = 8
    ///The TAB key.
    | [<Literal>]Tab = 9
    ///ライン フィード キー
    | [<Literal>]LineFeed = 10
    ///Clear キー
    | [<Literal>]Clear = 12
    ///Enter キー
    | [<Literal>]Enter = 13
    ///Return キー
    | [<Literal>]Return = 13
    ///Shift キー
    | [<Literal>]ShiftKey = 16
    ///The CTRL key.
    | [<Literal>]ControlKey = 17
    ///Alt キー
    | [<Literal>]Menu = 18
    ///Pause キー
    | [<Literal>]Pause = 19
    ///The CAPS LOCK key.
    | [<Literal>]CapsLock = 20
    ///The CAPS LOCK key.
    | [<Literal>]Capital = 20
    ///IME かなモード キー
    | [<Literal>]KanaMode = 21
    ///IME ハングル モード キー(互換性を保つために保持されています。HangulMode を使用します)
    | [<Literal>]HanguelMode = 21
    ///IME ハングル モード キー
    | [<Literal>]HangulMode = 21
    ///IME Junja モード キー
    | [<Literal>]JunjaMode = 23
    ///IME Final モード キー
    | [<Literal>]FinalMode = 24
    ///IME 漢字モード キー
    | [<Literal>]KanjiMode = 25
    ///IME Hanja モード キー
    | [<Literal>]HanjaMode = 25
    ///The ESC key.
    | [<Literal>]Escape = 27
    ///IME 変換キー
    | [<Literal>]IMEConvert = 28
    ///IME 無変換キー
    | [<Literal>]IMENonconvert = 29
    ///IME Accept キー互換性を維持するために残されています。代わりに System.Windows.Forms.Keys.IMEAccept
    ///を使用してください。
    | [<Literal>]IMEAceept = 30
    ///IME Accept キー (System.Windows.Forms.Keys.IMEAceept の代わりに使用します)
    | [<Literal>]IMEAccept = 30
    ///IME モード変更キー
    | [<Literal>]IMEModeChange = 31
    ///Space キー
    | [<Literal>]Space = 32
    ///PageUp キー
    | [<Literal>]Prior = 33
    ///PageUp キー
    | [<Literal>]PageUp = 33
    ///The PAGE DOWN key.
    | [<Literal>]Next = 34
    ///The PAGE DOWN key.
    | [<Literal>]PageDown = 34
    ///The END key.
    | [<Literal>]End = 35
    ///The HOME key.
    | [<Literal>]Home = 36
    ///← キー
    | [<Literal>]Left = 37
    ///↑ キー
    | [<Literal>]Up = 38
    ///→ キー
    | [<Literal>]Right = 39
    ///↓ キー
    | [<Literal>]Down = 40
    ///Select キー
    | [<Literal>]Select = 41
    ///Print キー
    | [<Literal>]Print = 42
    ///Execute キー
    | [<Literal>]Execute = 43
    ///PrintScreen キー
    | [<Literal>]PrintScreen = 44
    ///PrintScreen キー
    | [<Literal>]Snapshot = 44
    ///The INS key.
    | [<Literal>]Insert = 45
    ///The DEL key.
    | [<Literal>]Delete = 46
    ///The HELP key.
    | [<Literal>]Help = 47
    ///The 0 key.
    | [<Literal>]D0 = 48
    ///The 1 key.
    | [<Literal>]D1 = 49
    ///The 2 key.
    | [<Literal>]D2 = 50
    ///The 3 key.
    | [<Literal>]D3 = 51
    ///The 4 key.
    | [<Literal>]D4 = 52
    ///The 5 key.
    | [<Literal>]D5 = 53
    ///The 6 key.
    | [<Literal>]D6 = 54
    ///The 7 key.
    | [<Literal>]D7 = 55
    ///The 8 key.
    | [<Literal>]D8 = 56
    ///The 9 key.
    | [<Literal>]D9 = 57
    ///A キー
    | [<Literal>]A = 65
    ///B キー
    | [<Literal>]B = 66
    ///C キー
    | [<Literal>]C = 67
    ///D キー
    | [<Literal>]D = 68
    ///E キー
    | [<Literal>]E = 69
    ///F キー
    | [<Literal>]F = 70
    ///G キー
    | [<Literal>]G = 71
    ///H キー
    | [<Literal>]H = 72
    ///I キー
    | [<Literal>]I = 73
    ///J キー
    | [<Literal>]J = 74
    ///K キー
    | [<Literal>]K = 75
    ///L キー
    | [<Literal>]L = 76
    ///M キー
    | [<Literal>]M = 77
    ///N キー
    | [<Literal>]N = 78
    ///O キー
    | [<Literal>]O = 79
    ///P キー
    | [<Literal>]P = 80
    ///Q キー
    | [<Literal>]Q = 81
    ///R キー
    | [<Literal>]R = 82
    ///S キー
    | [<Literal>]S = 83
    ///T キー
    | [<Literal>]T = 84
    ///U キー
    | [<Literal>]U = 85
    ///V キー
    | [<Literal>]V = 86
    ///W キー
    | [<Literal>]W = 87
    ///X キー
    | [<Literal>]X = 88
    ///Y キー
    | [<Literal>]Y = 89
    ///Z キー
    | [<Literal>]Z = 90
    ///左の Windows ロゴ キー (Microsoft Natural Keyboard)
    | [<Literal>]LWin = 91
    ///右の Windows ロゴ キー (Microsoft Natural Keyboard)
    | [<Literal>]RWin = 92
    ///アプリケーション キー (Microsoft Natural Keyboard)
    | [<Literal>]Apps = 93
    ///コンピューターのスリープ キー
    | [<Literal>]Sleep = 95
    ///The 0 key on the numeric keypad.
    | [<Literal>]NumPad0 = 96
    ///The 1 key on the numeric keypad.
    | [<Literal>]NumPad1 = 97
    ///数値キーパッドの 2 キー
    | [<Literal>]NumPad2 = 98
    ///数値キーパッドの 3 キー
    | [<Literal>]NumPad3 = 99
    ///数値キーパッドの 4 キー
    | [<Literal>]NumPad4 = 100
    ///数値キーパッドの 5 キー
    | [<Literal>]NumPad5 = 101
    ///数値キーパッドの 6 キー
    | [<Literal>]NumPad6 = 102
    ///数値キーパッドの 7 キー
    | [<Literal>]NumPad7 = 103
    ///The 8 key on the numeric keypad.
    | [<Literal>]NumPad8 = 104
    ///The 9 key on the numeric keypad.
    | [<Literal>]NumPad9 = 105
    ///乗算記号 (*) キー
    | [<Literal>]Multiply = 106
    ///Add キー
    | [<Literal>]Add = 107
    ///区切り記号キー
    | [<Literal>]Separator = 108
    ///減算記号 (-) キー
    | [<Literal>]Subtract = 109
    ///小数点キー
    | [<Literal>]Decimal = 110
    ///除算記号 (/) キー
    | [<Literal>]Divide = 111
    ///The F1 key.
    | [<Literal>]F1 = 112
    ///The F2 key.
    | [<Literal>]F2 = 113
    ///The F3 key.
    | [<Literal>]F3 = 114
    ///The F4 key.
    | [<Literal>]F4 = 115
    ///The F5 key.
    | [<Literal>]F5 = 116
    ///The F6 key.
    | [<Literal>]F6 = 117
    ///The F7 key.
    | [<Literal>]F7 = 118
    ///The F8 key.
    | [<Literal>]F8 = 119
    ///The F9 key.
    | [<Literal>]F9 = 120
    ///The F10 key.
    | [<Literal>]F10 = 121
    ///The F11 key.
    | [<Literal>]F11 = 122
    ///The F12 key.
    | [<Literal>]F12 = 123
    ///The F13 key.
    | [<Literal>]F13 = 124
    ///The F14 key.
    | [<Literal>]F14 = 125
    ///The F15 key.
    | [<Literal>]F15 = 126
    ///The F16 key.
    | [<Literal>]F16 = 127
    ///The F17 key.
    | [<Literal>]F17 = 128
    ///The F18 key.
    | [<Literal>]F18 = 129
    ///The F19 key.
    | [<Literal>]F19 = 130
    ///The F20 key.
    | [<Literal>]F20 = 131
    ///The F21 key.
    | [<Literal>]F21 = 132
    ///The F22 key.
    | [<Literal>]F22 = 133
    ///The F23 key.
    | [<Literal>]F23 = 134
    ///The F24 key.
    | [<Literal>]F24 = 135
    ///The NUM LOCK key.
    | [<Literal>]NumLock = 144
    ///ScrollLock キー
    | [<Literal>]Scroll = 145
    ///左の Shift キー
    | [<Literal>]LShiftKey = 160
    ///右の Shift キー
    | [<Literal>]RShiftKey = 161
    ///左の Ctrl キー
    | [<Literal>]LControlKey = 162
    ///右の Ctrl キー
    | [<Literal>]RControlKey = 163
    ///左の Alt キー
    | [<Literal>]LMenu = 164
    ///右の Alt キー
    | [<Literal>]RMenu = 165
    ///戻るキー (Windows 2000 以降)
    | [<Literal>]BrowserBack = 166
    ///進むキー (Windows 2000 以降)
    | [<Literal>]BrowserForward = 167
    ///更新キー (Windows 2000 以降)
    | [<Literal>]BrowserRefresh = 168
    ///中止キー (Windows 2000 以降)
    | [<Literal>]BrowserStop = 169
    ///検索キー (Windows 2000 以降)
    | [<Literal>]BrowserSearch = 170
    ///お気に入りキー (Windows 2000 以降)
    | [<Literal>]BrowserFavorites = 171
    ///ホーム キー (Windows 2000 以降)
    | [<Literal>]BrowserHome = 172
    ///ミュート キー (Windows 2000 以降)
    | [<Literal>]VolumeMute = 173
    ///音量 - キー (Windows 2000 以降)
    | [<Literal>]VolumeDown = 174
    ///音量 + キー (Windows 2000 以降)
    | [<Literal>]VolumeUp = 175
    ///次のトラック キー (Windows 2000 以降)
    | [<Literal>]MediaNextTrack = 176
    ///前のトラック キー (Windows 2000 以降)
    | [<Literal>]MediaPreviousTrack = 177
    ///停止キー (Windows 2000 以降)
    | [<Literal>]MediaStop = 178
    ///再生/一時停止キー (Windows 2000 以降)
    | [<Literal>]MediaPlayPause = 179
    ///メール ホット キー (Windows 2000 以降)
    | [<Literal>]LaunchMail = 180
    ///メディア キー (Windows 2000 以降)
    | [<Literal>]SelectMedia = 181
    ///カスタム ホット キー 1 (Windows 2000 以降)
    | [<Literal>]LaunchApplication1 = 182
    ///カスタム ホット キー 2 (Windows 2000 以降)
    | [<Literal>]LaunchApplication2 = 183
    ///The OEM 1 key.
    | [<Literal>]Oem1 = 186
    ///米国標準キーボード上の OEM セミコロン キー (Windows 2000 以降)
    | [<Literal>]OemSemicolon = 186
    ///国または地域別キーボード上の OEM プラス キー (Windows 2000 以降)
    | [<Literal>]Oemplus = 187
    ///国または地域別キーボード上の OEM コンマ キー (Windows 2000 以降)
    | [<Literal>]Oemcomma = 188
    ///国または地域別キーボード上の OEM マイナス キー (Windows 2000 以降)
    | [<Literal>]OemMinus = 189
    ///国または地域別キーボード上の OEM ピリオド キー (Windows 2000 以降)
    | [<Literal>]OemPeriod = 190
    ///米国標準キーボード上の OEM 疑問符キー (Windows 2000 以降)
    | [<Literal>]OemQuestion = 191
    ///The OEM 2 key.
    | [<Literal>]Oem2 = 191
    ///米国標準キーボード上の OEM ティルダ キー (Windows 2000 以降)
    | [<Literal>]Oemtilde = 192
    ///The OEM 3 key.
    | [<Literal>]Oem3 = 192
    ///The OEM 4 key.
    | [<Literal>]Oem4 = 219
    ///米国標準キーボード上の OEM 左角かっこキー (Windows 2000 以降)
    | [<Literal>]OemOpenBrackets = 219
    ///米国標準キーボード上の OEM Pipe キー (Windows 2000 以降)
    | [<Literal>]OemPipe = 220
    ///The OEM 5 key.
    | [<Literal>]Oem5 = 220
    ///The OEM 6 key.
    | [<Literal>]Oem6 = 221
    ///米国標準キーボード上の OEM 右角かっこキー (Windows 2000 以降)
    | [<Literal>]OemCloseBrackets = 221
    ///The OEM 7 key.
    | [<Literal>]Oem7 = 222
    ///米国標準キーボード上の OEM 一重/二重引用符キー (Windows 2000 以降)
    | [<Literal>]OemQuotes = 222
    ///The OEM 8 key.
    | [<Literal>]Oem8 = 223
    ///The OEM 102 key.
    | [<Literal>]Oem102 = 226
    ///RT 102 キーのキーボード上の OEM 山かっこキーまたは円記号キー (Windows 2000 以降)
    | [<Literal>]OemBackslash = 226
    ///ProcessKey キー
    | [<Literal>]ProcessKey = 229
    ///Unicode 文字がキーストロークであるかのように渡されます。Packet のキー値は、キーボード以外の入力手段に使用される 32 ビット仮想キー値の下位ワードです。
    | [<Literal>]Packet = 231

    ///CapsLock
    | [<Literal>]OemCapsLock = 240
    ///カタカナひらがな
    | [<Literal>]KANA = 242
    ///全角から半角に変換する際の仮想キーコード
    | [<Literal>]ONE = 243
    ///半角から全角に変換する際の仮想キーコード
    | [<Literal>]ONJ = 244

    ///The ATTN key.
    | [<Literal>]Attn = 246
    ///Crsel キー
    | [<Literal>]Crsel = 247
    ///Exsel キー
    | [<Literal>]Exsel = 248
    ///EraseEof キー
    | [<Literal>]EraseEof = 249
    ///The PLAY key.
    | [<Literal>]Play = 250
    ///The ZOOM key.
    | [<Literal>]Zoom = 251
    ///今後使用するために予約されている定数
    | [<Literal>]NoName = 252
    ///PA1 キー
    | [<Literal>]Pa1 = 253
    ///Clear キー
    | [<Literal>]OemClear = 254
//    ///キー値からキー コードを抽出するビット マスク。
//    | [<Literal>]KeyCode = 65535
//    ///Shift 修飾子キー
//    | [<Literal>]Shift = 65536
//    ///Ctrl 修飾子キー
//    | [<Literal>]Control = 131072
//    ///Alt 修飾子キー
//    | [<Literal>]Alt = 262144
