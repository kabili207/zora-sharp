# Oracle Hack

A decoder for the password system used in the Legend of Zelda Oracle of Ages and Oracle of Seasons games. Inspired by the [original password generator](https://www.dropbox.com/s/nqkrp95gvs223re/ZeldaPasswords.exe) written by Paulygon back in 2001.

Current feature list:

 * Can decode game, ring, and memory secrets
 * Allows the user to save the decoded game information for later use

Planned features:

 * Ability to generate codes
 * Include a debugging screen to get raw information about secrets
 * Finish GTK interface for Linux users

### Current Problems

Right now the biggest problem preventing me from generating codes is the checksum used at the end of each code. Information on what is known so far in available in the docs folder.

### Special Thanks

A big thanks to 39ster over at [GameFAQs](http://www.gamefaqs.com/boards/472313-the-legend-of-zelda-oracle-of-ages/66934363) for figuring out how to decode the game secrets. I was able to expand his work to include ring secrets and memory secrets as well.
