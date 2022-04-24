# SusLang
A simple among-us-themed esolang written in C#

I made this in a couple hours because I was bored. I suggest not trying it out, it isn't fun, this language doesn't even support loops
I might add loops when I am bored again.


<h2>The file format</h2>

SusLang is an interpreted language which means that there are no compiled SusLang-files. 
The only file format this language uses is the .sus format for source code files

<h2>The syntax</h2>

Every statement is exactly one line.

There are 18 variables you can use, named after the 18 possible colors of among us players. You can see a list of all colors [here](https://among-us.fandom.com/wiki/Colors). However in contrast to the list, you need to write the color-names in lowercase.
Every color/variable can hold an integer value of the size of one byte so a number from 0 to 255</br>
This value is called the sus-meter of the color.
The sus-meters of all colors/variables is 0 by default.


<h3>List of statements</h3>
  
<h4>The sus statement</h4>

You can select a color/variable using the `sus`-Statement

<h4>The emergencyMeeting statement</h4>

Using the statement `emergencyMeeting` you can output the sus-meter of the currently selected color/variable encoded into ASCII

<h4>Statements for modifying the sus-meter</h4>

To modify the sus-meter of a color/variable, there are 4 different statements all with the same syntax:

```<color name> <statement>```

These statements are used to change the sus-meter:

<h5>killed</h5> increases the sus-meter of a color by 10

<h5>vented</h5> increases the sus-meter of a color by 1

<h5>didVisual</h5> decreases the sus-meter of a color by 10

<h5>wasWithMe</h5> decreases the sus-meter of a color by 1

For example, you can increase cyan's sus-meter by 10 with

```cyan killed```

Since the sus-meter is printed in ASCII and the first letter in ASCII is at number 65, you need to repeat that 6 times and 
use `cyan vented` once to get to a sus-meter of 65 or `A` in ASCII.
You can find an ASCII table [here](https://en.wikipedia.org/wiki/ASCII#Printable_characters).

<h3>Hello World Script</h3>
Using all these statements, we can now create a Hello-World-Script like this:

```sus cyan
cyan killed
cyan killed
cyan killed
cyan killed
cyan killed
cyan killed
cyan killed
cyan vented
cyan vented
emergencyMeeting
cyan killed
cyan killed
cyan killed
cyan wasWithMe
emergencyMeeting
cyan killed
cyan wasWithMe
cyan wasWithMe
cyan wasWithMe
emergencyMeeting
emergencyMeeting
cyan vented
cyan vented
cyan vented
emergencyMeeting
sus red
red killed
red killed
red killed
red vented
red vented
emergencyMeeting
sus cyan
cyan didVisual
cyan didVisual
cyan wasWithMe
cyan wasWithMe
cyan wasWithMe
cyan wasWithMe
emergencyMeeting
cyan killed
cyan killed
cyan vented
cyan vented
cyan vented
cyan vented
emergencyMeeting
cyan vented
cyan vented
cyan vented
emergencyMeeting
cyan didVisual
cyan vented
cyan vented
cyan vented
cyan vented
emergencyMeeting
cyan didVisual
cyan vented
cyan vented
emergencyMeeting
sus red
red vented
emergencyMeeting
```
You can find a version with explanation [here](Examples/helloWorld.sus)
