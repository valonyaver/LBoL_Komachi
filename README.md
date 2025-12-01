# KomachiMod
A mod for Lost Branch of Legends that adds Komachi as a playable character. Made by Nolavthe/Valonad.
Available languages: EN (English), JP (日本語), ES (Español), and KO (한국어)

# Overview
- Colours: Red and Black.
- Cards: 85 draftable cards excluding tokens.
- Exhibits: 2 exhibits for each of her starting loadouts
- Act 1 boss. Which can be disabled in the mod's configs (See: Configurable options below)

# Main Themes

## Distance manipulation
The main theme of Red Komachi.

Komachi's main powers lie in her ability to change the distance between anything. 
Distance is represented in the mod by a status effect applied to enemies, which represents how far away each enemy is from the player.

The closer an enemy is, the more damage they deal and take. The farther they are, the less damage they deal and take. Enemies start at distance 3, which provides no changes to damage.
The effects of the 5 Distance levels on enemies are:
- Very Close (1): Deals and takes +100% attack damage.
- Close (2): Deals and takes +50% attack damage.
- Normal (3): No damage modifier (default).
- Far (4): Deals and takes -15% attack damage.
- Very Far (5): Deals and takes -30% attack damage.

These values can be configured in the mod's configs (See: Configurable options below)

The keyword "**Displace**", refers to the act of changing the distance of an enemy.

Her cards focus on dancing between the different distance levels, providing bonuses the more you displace enemies, pulling enemies in before attacks, then pushing them back at the end of the turn when defending.

### Notes
- If you want to easily calculate the distance modifiers yourself, simply multiply your damage by 2/1.5/1/0.85/0.7 for distances 1/2/3/4/5.
- These damage multipliers stack multiplicatively rather than additively with other modifiers. Meaning that distance 1 (+100%) + vulnerable (+50%) multiplies your damage by 2 * 1.5 = 3. Rather than 2.5.

## Spirit Manipulation
The main theme of Black Komachi.
Komachi's job as a ferrier of the dead has allowed her to be in tune with the spirits of the dead, allowing her to use them in battle.
There are three types of Spirits:
- **Vengeful Spirits**: Status effect applies on enemies. Has both a count and a duration which usually starts at three. Every turn, Vengeful's duration will be reduced by 1. And when the Vengeful Spirits are removed, the enemy will take double its count as damage. In addition, the **"Detonation"** keyword will immediately remove the vengeful spirits off an enemy, dealing their damage immediately.

Be sure to stack as many Vengeful Spirits on enemies as possible, before detonating them with a big explosion. Alternatively, you can try to detonate them as much as possible with abilities that give you bonuses on enemy detonations.

- **Guided Spirits**: Buff applied on the player. At the end of the player's turn, will deal damage equal to its level to the enemy with the lowest health, then reduce its level by 1.

- **Divine Spirits**: Rare buff applies on the player. At the end of the Player's turn, the player will gain barrier equal to its level, then halve its level rounding down.

Accompanying these 2 latter buff spirits is a new keyword, **"Release"**. Release is an optional kicker cost on some of Komachi's cards which will let you remove a certain amount of Guided or Divine Spirits from yourself to activate an additional effect. Guided Spirits will always be prioritized first in paying the Release cost.

## Subthemes

### Spider Lilies
Red Komachi Subtheme.
Spider Lily is a retainable token that provides RR mana, gives the player some Temporary Firepower, and applies 3 poison on the player.
Use your effects that add Spider Lilies to the hand to retain them until a turn where you can use all of them at once and win the fight through the firepower and mana you acquired.
But if you miscalculate, you will have to deal with the poison they have applies on you.

### Exile Recursion
Black/White Komachi Subtheme.
As a Shinigami and a ferry of the dead, Komachi has the ability to connect with the afterlife, returning the cards that were exiled to be reused once again. With cheaper cards being easier to recur than more expensive ones.

This is mainly a black Komachi ability, however, opening up the white pool will give you access to stronger Black/White cards for this mechanic. There is also an ability that lets you add a random B/W card to your hand without ever having white mana in your pool.

# Configurable Options
You can disable the Komachi act 1 boss or configure the values for distance through the configuration files of the mod.
To change these configs: 
If you're on the steam workshop: Open the mod launcher, go to your mod management list and click on Bepinex. Click View Folder. This should take you to the installation folder for bepinex. From there, the configuration file will be at BepInEx\config\valon.LBoL.character.Komachi.cfg
If you're on r2modman: Go to config editor, and just search either "valon" or "komachi", you should see something like `Bepinex\config\valon.LBOL.character.Komachi.cfg`. Click it to see the configs.
Everything else should be self explanatory.


# Shortcomings and other notes
Some cards and status effects will have missing art, or have placeholder art made by yours truly. Many attack cards also lack an animation while others just reuse existing animations. I hope you can forgive this. I hope to learn how to make custom attack animations eventually.

As this is only my first mod, the balance of the mod might be a bit wack. Any card or effect is subject to change. I tried to keep the balance of the cards as close to vanilla characters as I can. She might be stronger than vanilla characters since I focused on having almost every card synergize with her mechanics or being independently strong, unlike vanilla which has a lot of filler "generic" cards for some of its characters.

By the words of my fellow modder rm -rf Maxx "c": `"keeping it balanced" This game is less balanced than a Jenga tower in the middle of a magnitude 8.5 earthquake, give up all hope.`

If you have any feedback, bug reports, or suggestions, or just want to post about the mod, please drop by the LBoL discord server and ping `@nolavthe`. I regularly frequent the server and love any conversations.

### The 2 Komachi Mods situation is crazy
I began development of this 1 month ago. 2 weeks later Cyaneko shadow dropped a Komachi mod which took me by a big surprise lmao. I think our mods are distinct enough in design that it's fine (Theirs is a B/W and mine is B/R), but due to the obvious nature of using the same character, lots of card arts in this mod may overlap with cyaneko's. 

*I am happy though, that my favourite character is the first one to have 2 separate mods dedicated to her for this game.*

But I do not know if it will be compatible if you play both mods at the same time. I tried to prefix most IDs in my mod with `KomachiModThing`. But there might still be some stuff that cause overlapping so idk. It's probably safer to just disable one when playing the other.

## Art Credits
All art is credited under `Art.MD`. If Art.MD isnt in the mod files, then you can find it in the github repository.

Thanks in particular to IcedLemon for making the icon for the Titanic exhibit for this mod. Thanks to the every touhou artist who has drawn wonderful art of my beloved.

# Special Thanks
- Iced_Lemon: For making some art specifically for the mod.
- Neff: For making the Japanese translation.
- xerox asutnima: For making the Spanish translation.
- coward_montblanc: For making the Korean translation.
- rm -rf Maxx "c": For making the sample character template that made it so easy to develop this mod.
- Vengyre: Providing me a spreadsheet template that made it much easier to organize my cards. Also programming help
- Worldsoul: Making a mod for my other favourite character, Shou, which encouraged me to make this mod. Also having his github repository public was helpful for referencing how some effects are implemented in a mod.
- Saevin_7: Playtesting the mod and providing much helpful feedback.
- Neoshrimp, gluee, and all the other people in #mod-dev: For helping me with many programming problems.
- Lvalon: For his :skull: emojis.