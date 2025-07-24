## 0.3.3
- Scythe of Exorcism's release effect now gives you an amulet.
- Shinigami's Finesse now gains accurate on close distances.
- Fixed a bug with Abundant Floating Spirits.
- Lock on amount applied by Endless Way increased from 2/4 to 3/6.
- Close Quarter Defence now applies temporary firepower down on a close target rather than giving you additional block.
- Fixed a bug with Exchange of the living and the dead.

## 0.3.2
- Fixed Equalize Distance not drawing the correct amount of cards.

## 0.3.1
- Added a new keyword, **Auto-Discard**. If your hand would reach its limit, discard auto discard cards before that happens!
- Spider Lily now has Auto-Discard.
*It's also the only card that's gonna use it lmao. Your hands won't be clogged by flowers any longer.*
- Talkative Ferryman now gives you the correct amount of the buff. Used to give you 4 regardless of upgrade.
- Fixed a bug where the guided spirits tooltip would give out an error under niche circumstances.

That's all folks.

## 0.3
### Bug fixes
- Fixed a bug with Reaper's due that stopped you from spending non W, B, or P mana from using it. Added a QoL number to the card text that shows the spirits you will gain from your pending mana.

### Reworks
I added lots of secondary release effects to cards.
- Spirits of the Firm gained a second release effect. `You can release 7 (6) spirits to add |Detonation| to your hand, in addition to its bonus Vengeful Spirits infliction.`
*This is now the second card to add a detonation token to the hand, besides grudging strike. The additional cost for this is the same as for Grudging strike.*
- Chain Detonation changed:
-- Spirits applied changed 5 (7) -> 4 (6)
-- First release cost "Detonate that target again" changed 5 (3) -> 4 (3)
-- Now has a second release cost: "|Release| 8: Apply |Vengeful Spirits| equal to the amount detonated, again."
*I always found the concept of this card detonating multiple times a turn to be funny but found the original release effect to be underwhelming, but that detonate+apply again on the same cost would be overpowered. But making the second application of spirits to be an even more expensive release effect works I think. Though if it may be too cheap do tell me.*
- Afterlife Passage+ can now exile cards from the discard pile in addition to the hand.
*Just a slight buff to make the upgrade actually worth something. I think keeping it only to discard instead of discard+draw like other cards is a good balancing measure for how good its effect is.*
- Abundant floating spirits reworked:
Old text:
```
    Deal 14 (18) damage to each enemy.
    Apply 4 (8) |Vengeful Spirits| to each enemy.
    |Release| 5: This attack becomes |Accurate|.
```
New text
```
    Deal 14 (18) damage to each enemy.
    Apply 4 (6) |Vengeful Spirits| to each enemy.
    |Release| 6: |Choose|: This attack becomes |Accurate| or Apply 4 (6) more |Vengeful Spirits| to each enemy.
    |Release| 12: Deal 12 (15) |Accurate| damage 2 times to each enemy, then apply 12 (18) |Vengeful Spirits| to each enemy instead.
```
*This was always just a "3 mana Spirits of the firm that also does damage" in my mind. Not interesting. SO I thought about giving it a multi attacking effect to really hone in that this card wrecks grazers. Also gave a choice to the first release effect to make it slightly less useless against non grazing enemies.*
- Double Hooking reworked.
Old text:
`Discard {Value1} card, then target up to {Value2} cards in |Exile| whose total cost is less than {Value3} each; Return them to the hand. They temporarily cost 0.`
New text:
`Discard {Value1} card, then target up to {Value2} cards in |Exile| whose total cost is less than {Value3} each; Return the first to the hand and return the second to the discard pile. They temporarily cost 0.`
*This card was just busted. But I wanted to keep in the effect of recurring two cards to reference its namesake card from yugioh. I think sending the second one to discard is a good compromise.*
- Scythe of Final Judgement reworked.
Old text:
```
    Deal 30 (35) damage. Deals double damage if the enemy has |Barrier| or |Block|. Damage reduction of far distances increases this card's damage instead.
    When an enemy is defeated using this card, add 1 (2) |Spider Lily| to the hand.
```
New text:
```
    |Displace| to any distance.
    Deal 21 (28) damage. Damage reduction of far distances increases this card's damage by double the amount instead.
    When an enemy is defeated using this card, add 1 (2) |Spider Lily| to the hand.
```
Its cost is changed from 2RR (1RR) to just 2RR. Just so it wouldn't powercreep taste of death.
*This card had the problem of both having the coolest name ever but also wanting to keep it faithful to the same spell in SWR. But the original felt really underwhelming. It intentionally doesn't have accurate because its description in soku specifically says it's easily grazed. The defence wrecking was just a tacked on thing. But I think reducing its base damage while making a card to just choose your enemy's distance captures how impactful it could be pretty well. While doubling the far distance bonus is just because why not? Maybe overtuned?*
- Shiki Eiki Teammate's ultimate skill changed.
Old Effect:
`Return up to 2 (3) exiled cards to the hand. Their cost becomes 0 this combat. {PlayerName} doesn't pay the |Release| cost of cards this combat.`
New effect:
`Choose up to 1 (2) card(s) in the |battlefield| and add them to the hand.Their cost becomes 0 this combat. {PlayerName} doesn't pay the |Release| cost of cards this combat.`
*This is mainly the reason for most of the new release effects above. I wanted to make Eiki's ultimate effect more easily usable and more impactful. It now can search and 0 out any card from the battlefield so that if you want a release card to use immediately, you can. Otherwise you can still pick back up a broken exiled card if you want.*
- Ritual of Ecstasy's Will now give you the option of which effect to use if the target is at a distance of 3. 
Updated text
```
    Invert target's |Distance|, then apply 1 of these effects.
    If the target's distance is {three}, you can choose the effect.
    If the distance was reduced, draw {Value1} cards. If it was increased, gain {Value2} graze and {Block} block.
```
Old text: `If the distance was reduced, draw {Value1} cards. Otherwise, gain {Value2} graze and {Block} block.`
*Changed because the old text was ambiguous to some players. This makes it both clearer and more flexible, to let you draw if you need it.*

### Simple changes
- Close Quarter Technique+'s Guided Spirit gain changed 8 -> 6. Its bonus defence changed 8 (11) -> 10 (14)
- Ethically Sourced Break's heal changed 5 (6) -> 10 (15). Upgraded Barrier changed 50 -> 45.
- Endless Way+ cost changed from 1B/R to B/RB/R (Aka, two hybrid Red-Black mana)
- Equalize distance rarity changed Uncommon -> Common. . Unupgraded version draws 2 cards per displacement.
- Grudging Strike's unupgraded release cost changed 4 -> 3
- Lifespan's End+ cost increased from 0 to W
- Moving Defence now only adds a "Manipulate Distance" for every enemy whose distance is at 4 (3), up from 3 (2)
- Move and Shoot damage changed 14 (17) -> 12 (16)
- Shinigami's Finesse rarity changed Uncommon -> Common.
- Taste of Death's unupgraded damage changed 16 -> 15. Taste of Death+'s cost is now changed 2R -> 1RR (Now unchanged from the unupgraded version).

## 0.2.1
- Fixed a bug with Residual Aether not giving the correct amount of Mana shown on the card, only giving 1 mana no matter what.
- Fixed Gold Sarcopha-galley only showing the top 5 cards of the deck instead of the entire deck.
- Fixed Grudging Strike not having the release keyword being related to it. (Ie, not explained on the side of the card)
- Fixed Grudging Strike not removing Guided Spirits when using its release effect.
- Scythe of Wandering spirit renamed to Scythe of Draining Spirit, to make it clearer from the other card, Scythe of Roaming Spirit

## 0.2.0
- Added Art for all cards
- Changed names of cards: Retreating Sanzu Boat->Protective Sanzu Boat. Ghost Wall-> Spirit Backing
- Human Spirits passing by+ gives 6 Guided Spirits down from 7.
- Fixed Scythe of Exorcism relating to itself rather than to spider liies.

## 0.0.2

Bugfixing

- Fixed a bug with eye of the storm where activating the release effect to pay for divine spirits just pays using the guided spirits you just gained from the card itself instead.
- Upgraded Reaper's due nerfed. The spirits gained and applied from coloured mana is 8->6 now. The guided spirits gained from any mana is 6->5 now.
- Nerfed upgraded damage of move and shoot from 20->17
- Downscaled some arts so that the mod isn't huge for no reason. Size was halved.
- Fixed colour sorting in the collection. White cards will now correctly be at the end of the collection alongside the rest of the off colours.