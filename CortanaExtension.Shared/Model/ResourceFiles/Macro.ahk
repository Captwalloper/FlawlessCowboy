; /* Functions */

LaunchGoogle()
{
	Run, https://www.google.com/
	
	WinWait, Google
	Sleep, 1000
}

FocusAddressBar()
{
    Send, !f
	Send, !d
	Sleep, 250
}

RunIMacroOnAddressBar(name)
{
	SendInput, bm
	Send, {Tab}
	Sleep, 500
	
	clipboard=%name%
	Send, ^v
	Sleep, 500
	Send, {Enter}
}

RunIMacro(name) {
	LaunchGoogle()
	FocusAddressBar()
	RunIMacroOnAddressBar(name)
}

; /* Main */

scriptName := "Test"

RunIMacro(scriptName)

