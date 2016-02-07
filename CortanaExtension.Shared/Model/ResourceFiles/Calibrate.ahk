; /* Config */
resourceDir := "C:\Users\comcc_000\Documents\Visual Studio 2015\Projects\Reveille\Revielle\Model\ResourceFiles"

; /* Main */
	
OpenSettings()
SelectRelearnVoice()
; //Close Command Prompt
promptID := "C:\WINDOWS\system32\cmd.exe"
WinWaitActive, %promptID%
WinClose, %promptID%
ExitApp

; /* Keys */
x::
	ExitApp  ; Assign a hotkey to terminate this script.
Return

; /* Functions */
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

OpenSettings() {
	global resourceDir	
	
	OpenCommandPrompt()
	CD(resourceDir)
	RunOnCommandLine("Cortana_Settings.lnk")

	sleep, 1000
}

Tab(numTabs) {
	loop, %numTabs% 
	{
		Send, {Tab}
		sleep, 1000
	}
}

SelectRelearnVoice() {
	Tab(7)
	Send, {Enter}

	Sleep, 200
}
