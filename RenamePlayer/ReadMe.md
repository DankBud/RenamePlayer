## RenamePlayer
Renames Player if the name has any invalid (non-ASCII) characters.


### Usage:
-- none --


### Permissions:
-- none --


### Design:
* Hooks on Greet Player
* Converts copy of player name to ASCII code page
* If name is different
* * Notify player
* * Broadcast to all because this happens after join message

### Notes:
* This is based on the work of Scavenger.


### Version History:

1.0.3.0:
* Changed random letters to "guest_{hash}"

1.0.2.0:
* Replaced Greet with Join

1.0.1.0: 
* Added logging on rename
* Added random number to replace ?

1.0.0.0: Initial Release
* Thanks to Scavenger


### Download:

* Plugin

* Source
