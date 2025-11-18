![](thumbnail.png?raw=true)

# Diorama

Lore of a Nomai named Ping. Explore why there are modding jams in the Outer Wilds universe.

An entry for Outer Wilds modding Jam 5.

## Notes for judges

**The jam submission version is Diorama v0.4.1 and Mod Jam 5 v1.0.3.** (Of course, you could get better experience in the latest version)

There seem to be two issues. I have never encountered them, but these are randomly occurring in some computers maybe depending on their specs.

1. Randomly broken tractor beams or broken initial warp pad (**fixed from v0.5.1**): **This is fixed in the latest version**, but the jam submission version still has this issue. If you encounter this, it breaks all things of this mod, so please try quiting and restarting the game or use another computer.  
(This is caused by the initialization which tries to find a specific base game object by `GameObject.Find`, but NH disables that object randomly before the initialization, so the initilization cannot finish forever. This is fixed by using `SearchUtilities.Find` from v0.5.1.)
2. Randomly broken size of the hatchling: Only Wyrm has seen it when they used the first warp pad from the central station to the diorama interface. I suspect the small warp pad might cause such a phenomenon. It could be fixed by the meditation.

## Brightness setting

From v0.5.3, there is the brightness setting for the specific place.

## Credits

* orclecle: the author

Playtesters (in time order)

* nepyanagi
* 8m410
* pomishi
* Ixrec
* Ultrinik
* pikpik\_carrot

Technical helps (in time order)

* JohnCorby: told me how to use unity profiler in outer wilds to resolve the lagging issue
* Ixrec: gave me a specific critical idea
* pikpik\_carrot: gave me a specific model with correct colliders

Ernesto

* Ernesto: loves the location frequently displaying coodinates in some mods.
