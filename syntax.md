# Syntax Explanation

There are 18 variables you can use, named after the 18 possible colors of among us players. You can see a list of all colors [here](https://among-us.fandom.com/wiki/Colors#List_of_colors). However, you need to write the color-names in lowercase.
Every color/variable can hold a byte so a number from 0 to 255</br>
The values of all colors/variables are 0 by default.

## List of statements
  
### The sus statement

You can select a color/variable using the `sus`-Statement
What "selecting" means, is explained in the following:

<br>

### Statements for modifying the sus-meter

To modify the sus-meter of a color/variable, there are 4 different statements all with the same syntax:

```<color name> <statement>```

These statements are used to change the value of the variables:

**killed** increases the sus-meter of a color by 10

**vented** increases the sus-meter of a color by 1

**didVisual** decreases the sus-meter of a color by 10

**wasWithMe** decreases the sus-meter of a color by 1

For example, you can increase cyan's sus-meter by 10 using

```cyan killed```

<br>

### Output Statements

You can output the values of variable in 2 different ways:

**emergencyMeeting** prints the selected variable to the terminal encoded in ASCII

**report** prints the selected variable to the terminal as a number

Since `emergencyMeeting` prints a variable's value in ASCII and the first letter in ASCII is at number 65, you need to first increase a variable to 65 (capital A in ASCII) to print text.
From 65, ASCII goes through the alphabet first in uppercase then in lowercase.
You can find an ASCII table [here](https://en.wikipedia.org/wiki/ASCII#Printable_characters).

If you don't want to always write:

```
cyan killed
cyan killed
cyan killed
cyan killed
cyan killed
cyan killed
cyan vented
cyan vented
cyan vented
cyan vented
cyan vented
```
to get to `A` in ASCII, you can use `#define suspect` as described [here](https://github.com/zenonet/SusLang/wiki/Define-Expressions#define-suspect).

<br>

### The Who?-Statement

Using the line `who?` you can let the interpreter wait for the user to input a color. This color will then be selected.

However this Statement only works in console builds. Also, `who?` is sadly currently the only way of having the user input data.

<br>

### Loops

You can put parts of your code in brackets (`[]`) to create a loop. This loop will run until the selected colors value turns 0.
If you want to learn more about SusLang-Loops, you can read [this](https://github.com/zenonet/SusLang/wiki/Loops) wiki article.

<br>

### The wasWith Statement

You can use `<colorA> wasWith <colorB>` to assign colorB's value to colorA.<br>
This is simmilar to the `=` Statement is other (less sussy) languages.

However, you can also use `wasWith` to to create a pointer to a keyword as described [here](https://github.com/zenonet/SusLang/wiki/Keyword-Pointers).

<br>

### Comments

The interpreter will ignore any text in a source file that is behind `trashtalk` or `//`. You can use this to explain your code.

For example you could write `trashtalk this is for writing an A` and the interpreter will completely ignore it

## He-Syntax

Instead of writing the name of a color, you can also use `he` or `she` to use the currently selected color.<br>
Example:

```
red sus
he killed // This will increase red by 10
```
