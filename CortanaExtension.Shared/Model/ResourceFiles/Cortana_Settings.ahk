; /* Config */
resourceDir := "C:\Users\comcc_000\Documents\Visual Studio 2015\Projects\Reveille\Reveille\Model\ResourceFiles"

; /* Functions */

OpenCommandPrompt() {
	run %comspec% 
	Click
}

RunOnCommandLine(line) {
	SendInput, %line%
	Send, {enter} ;
}

OpenSettings() {
	OpenCommandPrompt()
	line := % "cd " . """" . resourceDir . """"
	RunOnCommandLine(line)
	line := "Cortana_Settings.lnk"
	RunOnCommandLine(line)

	sleep, 1000
}

Tab(numTabs) {
	loop, %numTabs% 
	{
		Send, {Tab}
		sleep, 1000
	}
}

ToggleHeyCortana() {
	Tab(4)
	Send, {Space}

	Sleep, 500
}

CloseCommandPrompt() {
	Send, {ALTDOWN}{TAB}{ALTUP}
	Sleep, 500
	SendInput, Exit
	Send, {enter} ;
}

; /* Main */
	
OpenSettings()
ToggleHeyCortana()
CloseCommandPrompt()

