![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/zenonet/SusLang/dotnetPublish.yml)

# SusLang

A simple among-us-themed esolang written in C#.<br>
You can find an online interpreter [here](http://api.zenonet.de/SusLang/0.4/). There is also an experimental online interpreter for the not yet released version 0.5 [here](http://api.zenonet.de/SusLang/0.5/).

## The concept

Every player color from Among Us is a one-byte variable. This byte stores how sus that person is. You can increase variables by saying people did suspicious things like venting or killing. 

## The file format

SusLang is an interpreted language which means that there are no compiled SusLang-files. 
The only file format this language uses is the .sus format for source code files.

## The syntax

The language sytax is documented [here](https://github.com/zenonet/SusLang/blob/master/syntax.md).

## The wiki

If you want to learn more about SusLang, you can do so in our wiki: https://github.com/zenonet/SusLang/wiki

## Hello World Script

A simple hello-world-script in SusLang looks like this:

```suslang
sus cyan
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
You can find a version with explanation [here](Examples/helloWorld.sus).
