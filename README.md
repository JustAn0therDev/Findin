# Find In

!['Find in's logo](Findin/findin.ico)

Find in: A search tool to find files by content.

I created this program because I use Visual Studio at my job and I do not like it's search; it is generally very slow and time consuming, especially on larger repositories. It gets worse as the projects grow bigger and legacy projects don't even seem to yield some results at all, even when searching for a file (not only for content inside them).

You can get started by downloading the latest release and reading the usage section below. Feel free to make pull requests and/or change it as you see fit. Maybe something different "here and there" fits your workflow better than the program's current state.

## How does it work?

You can see how it works in detail by reading the `MainWindow.cs` file. The algorithm is pretty straight-forward.

## Usage

When you run the program for the first time, this is the screen you'll see:

![A screenshot of the Initial Screen](DocsImages/InitialScreen.jpg)

To make sense of each field and button:

- Path: The path where the files you want to search in are located. You can also click the "Select Folder" button and select the path from there;

- File Patterns: The allowed file patterns to look for. You can put in a single extension like `*.cs` or any amount of patterns you want, separated by `;`. For example: `*.cs;*test.py;*.c;*score.go`;

- Ignore Directories: The directories you'd like to ignore during the search, like a `node_modules` directory. You can also ignore more than one directory: `bin;obj;wwwroot;node_modules`;

- Regex Search: The search pattern you want to look for using a regular expression;

- The big table: Its a `ListView` that shows a table with the matches. It displays each line with three columns containing the path to the file, the line number and part of the content matched in the line if it surpasses the character limit, otherwise only part of it is shown;

- Search button: Searches for the pattern you typed in, but you can also press `Enter` in any text field to do that.

With all fields filled, the form should look something like this:

![A screenshot of the initial screen with all fields filled](DocsImages/AllFieldsFilled.jpg)

**Important: when you close the program, it'll save the state in a file called `state.bin`. So do not worry about losing what you have searched for after exiting the program.**

After searching, the matches should be displayed:

![A screenshot of the program searching for matches](DocsImages/MatchesFound.jpg)

In the bottom left corner, you can see how many occurrences and lines the program found. **The number of lines shown is limited to 200, but all desired files are searched so you can know how many occurrences were found**.

 If the program takes a bit longer to find all matches, you'll see a `Searching...` label in that same corner before the results, occurrences and lines are displayed.


**This tool will always be open-source and free to use. The next milestone to release a Mac OS version.**
