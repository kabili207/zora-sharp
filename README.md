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
 * Allows import and export of game data using a json-based `.zora` file
 
### The .zora save file
The `.zora` file contains all relevent information to recreate a player's game and ring secrets. Data is saved
as a JSON object, with the intention that it can be used with other implementations of the password system.
The file gets it's name from a contraction of **Z**elda **Ora**cle and from the Zora, one of the games'
races and enemies.

```json
{
    "Hero": "Kabi",
    "GameID": 21353,
    "Game": "Ages",
    "Child": "Derp",
    "Animal": "Moosh",
    "Behavior": "Infant",
    "IsLinkedGame": true,
    "IsHeroQuest": false,
    "Rings": -5188093993887465139
}
```

Valid values for `Behavior` are: `Infant`,
`BouncyA`, `BouncyB`, `BouncyC`, `BouncyD`, `BouncyE`,
`ShyA`, `ShyB`, `ShyC`, `ShyD`, `ShyE`,
`HyperA`, `HyperB`, `HyperC`, `HyperD`, `HyperE`.

Valid values for `Game` are `Ages` or `Seasons`. This value refers to the _target_ game.

Valid values for `Animal` are `Ricky`, `Dimitri`, or `Moosh`.

The rings are saved as a 64 bit signed integer. A signed integer was chosen to maintain compatibility with
languages that don't support unsigned integers.

None of the fields are required; the OracleHack library will load whatever is present, however the same
cannot be guaranteed for other libraries that implement the `.zora` save file.

### Special Thanks
 * Paulygon - Created the [original secret generator](http://home.earthlink.net/~paul3/zeldagbc.html) way back in 2001
 * 39ster - Rediscovered [how to decode game secrets](http://www.gamefaqs.com/boards/472313-the-legend-of-zelda-oracle-of-ages/66934363) using paulygon's program
 * [LunarCookies](https://github.com/LunarCookies) - Discovered the correct cipher and checksum logic used to generate secrets
