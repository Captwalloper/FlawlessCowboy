; /* Search YouTube from Clipboard */

Run, https://goosh.org/
Sleep, 2000
youtube := "youtube.com "
SendInput, %youtube%
SendInput, ^v
Send, {enter}
Sleep, 1000

open := "open 1"
SendInput, %open%
Send, {enter}
Sleep, 3000

; /* Cast to TV */

; screen cast button
x := 1850
y := 50

MouseMove, x, y
Sleep, 1000
Click x, y

; first listed cast device
x := 1825
y := 120

MouseMove, x, y
Sleep, 1000
Click x, y