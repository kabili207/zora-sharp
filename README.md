# OracleHack

A library for working with the password system used in the Legend of Zelda Oracle of Ages and Oracle of Seasons games.
Inspired by the [original password generator](http://home.earthlink.net/~paul3/zeldagbc.html) written by
Paul D. Shoener III a.k.a. Paulygon back in 2001.

### User Interfaces
OracleHack is only a library for manipulating the codes from the Oracle series games, meant for use by developers.
General users should instead use of the the following user interfaces:
 * Windows WPF - https://github.com/kabili207/oracle-hack-win
 * Linux GTK - https://github.com/kabili207/oracle-hack-gtk

### Features
 * Decodes game and ring secrets
 * Generates game, ring, and memory secrets
 * Allows the user to save the decoded game information for later use

### Special Thanks
 * Paulygon - Created the [original secret generator](http://home.earthlink.net/~paul3/zeldagbc.html) way back in 2001
 * 39ster - Rediscovered [how to decode game secrets](http://www.gamefaqs.com/boards/472313-the-legend-of-zelda-oracle-of-ages/66934363) using paulygon's program
 * [LunarCookies](https://github.com/LunarCookies) - Discovered the correct cipher and checksum logic used to generate secrets
