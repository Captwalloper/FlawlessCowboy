; /* Config */

continueTitle := "Continue?"

; /* Main */
	
OpenEmail()
WaitForContinue()
OpenCanvas()
WaitForContinue()
OpenSakai()
WaitForContinue()

ExitApp

; /* Keys */
x::
	ExitApp  ; Assign a hotkey to terminate this script.
Return

Space::
	CloseWaitForContinue()
Return

c::
	WinGetTitle, Title, A
	MsgBox, The active window is "%Title%".
Return

LAlt::
	MinimizeWaitForContinue()
Return

; /* Functions */
OpenEmail() {
	WindowsRun("Thunderbird")
	WinWait, Inbox - Mozilla Thunderbird
}

OpenCanvas() {
	RunIMacro("Canvas")
	WinWait, User Dashboard
}

OpenSakai() {
	RunIMacro("Sakai")
	WinWait, e-Learning : My Workspace : Home
}

WindowsRun(name) {
	Send, {LWin down}{r}{LWin up}
	sleep, 500
	SendInput, %name%
	sleep, 100
	Send, {Enter}
}

WaitForContinue() {
	global continueTitle
	MsgBox,		0, %continueTitle%, Ready to Continue? 
}

CloseWaitForContinue() {
	global continueTitle
	WinClose, %continueTitle%
}

MinimizeWaitForContinue() {
	global continueTitle
	WinMinimize, %continueTitle%
}

OpenCommandPrompt() {
	run %comspec% 
	Click
}

RunOnCommandLine(line) {
	SendInput, %line%
	Send, {enter} ;
}

CD(dir) {
	line := % "cd " . """" . dir . """"			
	RunOnCommandLine(line)
}

Tab(numTabs) {
	loop, %numTabs% 
	{
		Send, {Tab}
		sleep, 1000
	}
}

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

