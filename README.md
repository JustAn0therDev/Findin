# Find In

A search tool to look for files by their content.

I created this program because I use Visual Studio at my job and I do not like it's "search in files" since its very slow and time consuming. It gets worse as the projects get bigger and legacy projects don't even seem to yield some results at all, even when searching for a file (not for content inside them).

**This program aims for good performance and quick usage.**

You can get started by downloading a release and reading the usage section below. Feel free to colaborate and/or change it as you see fit. Maybe something different "here and there" fits your workflow better than its current state.

## How does it work?

The program reads the directory from the Path field and loads it to a `Dictionary<string, StringBuilder>` (hash table) in memory. From there, it'll check the state of the file based on a `FileSystemWatcher`, allowing the content to stay fast to search while up-to-date.

## Usage

When you open up the program for the first time, this is the screen you'll see:

![A screenshot of the Initial Screen](DocsImages/InitialScreen.jpg)

And to make sense of each field and button:

- Path: The path where the files you want to search in are located. You can also click the "Select Folder" button and select the path from there;
- File types: The allowed file types to look for. You can put in a single extension like `cs` or any amount of extensions you want, separated by ";" like `cs;py;c;go`;
- Ignore Directories: The name of directories you would like to ignore during the search. For example, a `node_modules` folder. You can ignore more than one directory like the file type filter: `bin;obj;wwwroot;node_modules`;
- Search: The search pattern you want to look for. The checkbox right after it allows you to make a case-insentive search;
- Set Default Program Path: The program you want to open the files in. For example, if you found the match you were looking for and set the path for _Visual Studio Code_ in your machine, you can double-click the item listed in the white box and the file will be opened in the editor:

![A screenshot showing a highlighted item in the ListBox result](DocsImages/DoubleClickToOpenInYourSetDefaultProgram.jpg)

- The big white box: Is a `ListBox` that shows a line preview of the matched content inside a file.

- Search button: Searches for the search pattern you typed in, but you can also press the "Enter key" while in the search box.

With all fields filled, the form would look something like this:

![A screenshot of the initial screen with all fields filled](DocsImages/AllFieldsFilled.jpg)

**Important: when you close the program's window, it'll save the state in a file called `state.bin`. So do not worry about losing what you searched for after exiting the program.**

When we click search (or press Enter while in the Search box):

![A screenshot with the program loading the select path/directory](DocsImages/LoadingDirectory.jpg)

The _Loading Directory_ label shown in the right below the "Search" button indicates that the program is loading the contents of the selected path
to a hash table in memory. This hash table is updated when a file is renamed, changed, deleted or a new file is created.

**While the directory is loading, you cannot search for files. If you have a hard drive, seeing the _Loading Directory_ label after setting a path will be a bit more common compared to using an SSD.**

When the program finishes loading the directory, you can now search for contents in files:

![A screenshot of the program searching for matches](DocsImages/Searching.jpg)

During the search, one of two things can happen: 

The program will find too many matches and limit the top 250:

![A screenshot of the program showing the match limit](DocsImages/MatchLimit.jpg)

If not, the program will show how many matches it found:

![A screenshot of the program showing the number of matches](DocsImages/MatchesFound.jpg)

-------------
## In Development

Even though this tool is still in development, it is ready to use. Here is a "To Do" list of the remaining tasks:

- ~~Search for files with specific extensions~~;
- ~~Search with regular expressions~~;
- ~~Remove regular expression field since the search is done with Regex from the get-go~~;
- ~~Re-think user experience~~;
- ~~Do not allow for search in binary files~~;
- ~~Allow the user to locate a path by searching for it inside the program with a folder helper~~;
- ~~Save the last search in a file that is always updated by each search made (this makes the "Search" button even better)~~;
- ~~_If a search button is really used, it should have a consistent tab index for a good navigation_~~;
- ~~Fix Regex when searching for characters such as "(" and "."~~;
- ~~Fix file paths with spaces~~;
- ~~Fix performance issue in the first directory loading, where the program must search for every possible directory (try `EnumerateFiles`)~~;
- ~~Allow the user to ignore certain directories~~;
- Allow for tabs with separate search states.

The list is updated per feature implementation, change request or bug fix.

**This tool will always be open-source and free to use.**
