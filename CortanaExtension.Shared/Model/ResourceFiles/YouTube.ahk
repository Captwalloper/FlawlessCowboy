; /* Config */
gooshTitle := "goosh.org - the unofficial google shell. - Google Chrome"
youtubeTitle := "YouTube - Google Chrome"


; /* Main */
searchString := clipboard ;"batman"
YouTube(searchString)
CastToTV()
ExitApp


; /* Keys */
LCtrl::
	ExitApp  ; Assign a hotkey to terminate this script.
Return

LAlt::
	WinGetTitle, Title, A
	MsgBox, The active window is "%Title%".
	clipboard = %Title%
Return


; /* Functions */
Close(title) {
	WinClose, %title%
}

YouTube(searchString) {
	global gooshTitle
	global youtubeTitle

	Run, https://goosh.org/
	WinWaitActive, %gooshTitle%
	sleep, 500
	line := % "youtube.com " . searchString
	RunLineOnGoosh(line)

	RunLineOnGoosh("open 1")
	DerpWait(500)
}

RunLineOnGoosh(line) {
	SendInput, %line%
	sleep, 250
	Send, {Enter}
	Sleep, 1000
}

CastToTV() {
	; screen cast button
	x := 1720
	y := 50

	MouseMove, x, y
	Sleep, 1000
	Click x, y

	; first listed cast device
	x := x - 25
	y := 120

	MouseMove, x, y
	Sleep, 1000
	Click x, y
}

DerpWait(minSeconds) {
	Sleep, %minSeconds%
	WinGetTitle, Title, A
	WinWait, %Title%
}