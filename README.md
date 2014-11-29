# Oracle Hack

A decoder for the password system used in the Legend of Zelda Oracle of Ages and Oracle of Seasons games. Inspired by the [original password generator](https://www.dropbox.com/s/nqkrp95gvs223re/ZeldaPasswords.exe) written by Paulygon back in 2001.

### Features

 * Decodes game and ring secrets
 * Partially generates game, ring, and memory secrets
 * Allows the user to save the decoded game information for later use

### TODO

 * Include a debugging screen to get raw information about secrets
 * Finish GTK interface for Linux users
 * Determine logic used for checksums

### Special Thanks
 * Paulygon - Created the [original secret generator](http://home.earthlink.net/~paul3/zeldagbc.html) way back in 2001
 * 39ster - Rediscovered [how to decode game secrets](http://www.gamefaqs.com/boards/472313-the-legend-of-zelda-oracle-of-ages/66934363) using paulygon's program
 * [LunarCookies](https://github.com/LunarCookies) - Discovered the correct cipher and checksum logic used to generate secrets
