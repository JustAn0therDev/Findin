# Find In

A search tool to look for files by their content. You can configure it to look for:

- Specific extensions;
- Search with Regular Expression;

Although the things listed above are not unique to any "file finder" or "find in file" program, feel free to colaborate and/or change it as you see fit. Maybe something different "here and there" fits your workflow better than its current state.

## Technical list

The list below is a technical detail about how some features should be implemented:

- Any kind of file search should be done using multiple threads and locking the files that are being searched in each thread. If a file was already searched, its path should be added to a list so its not searched again. This is definitely doable, the only thing I can't do is include the file names in the ResultListBox if its on a thread that is not the main one. <- This does not have to be done right now, but could be necessary in the future.

- ~~Some information about the location of the content should be shown:~~
	- ~~Line number~~

- ~~Change the "TextChanged" event in the text box for something else, this is related to the "Re-think user experience" task.~~ -> This has been fixed by making the search and Regex match asynchronous;
- ~~Fix the response time or search timing so that the program feels much faster. Search timing could mean clicking on a "Search" button~~;
- ~~Make the debugging experience better, since Visual Studio 2022 can't keep up with the new .NET 6 thread operations~~.

## In Development

It is still in development but it should take long before its ready. Here is the "TODO List" of remaining tasks:

- ~~Search for files with specific extensions~~;
- ~~Search with regular expressions~~;
- ~~Remove regular expression field since the search is done with Regex from the get-go~~;
- ~~Re-think user experience~~;
- ~~Do not allow for search in binary files~~;
- ~~Allow the user to locate a path by searching for it inside the program with a folder helper~~;
- ~~Save the last search in a file that is always updated by each search made (this makes the "Search" button even better)~~;
- ~~_If a search button is really used, it should have a consistent tab index for a good navigation_~~;
- Fix Regex when searching for characters such as "(" and ".";
- Allow for tabs with separate search states.

The list is updated per feature implementation, change request or bug fix.

**This tool will always be open-source and free to use.**