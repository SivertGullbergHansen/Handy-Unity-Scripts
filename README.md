# Handy-Unity-Scripts
Some scripts that I've made for unity that simplifies workflow.

**All scripts should be placed in the Assets folder of your project. I would recommend collecting them in a folder called "Plugins".**

## 🍓 Add_Dynamic_Bone.cs:
Script that adds the dynamic bone script to array of objects.

Made for Dynamic Bone version 1.2.2

Current version: 1.2

This script adds the dynamic-bone script to an array of objects. Contains all the parameters that the dynamic bone script includes.

🎥Video Preview: https://youtu.be/dZWyMJzwZr0

🆕Update video for v1.2: https://youtu.be/eMA8fxI9Lww

### How to use: 

1. Open the tool by going to Tools → Sivert → "Dynamic Bone Adder".
2. Populate the Array at the top of the script with the gameobjects you want to add the dynamic bone script component to.
3. Change settings as you wish.
4. Press the button close to the bottom of the tool-window that says "Add script to bone(s)".

### Extra:

5. To update every script added to the bones, press the button that says "Update bone(s)".
6. To copy the settings from the bones to the tool, press the button that says "Copy settings from bone(s)".
7. To remove the dynamic bone script from the bones, press the button that says "Remove script from bone(s)".

## 🍓 playRandomSound.cs:
Script that spawns an empty gameobject during runtime with an audiosource attached, inside of an area, by a timer.

Made for Unity 2020+ (Most likely works with older unity-versions)

Current version: 1.1

🎥Video Preview: https://youtu.be/Yxr0_egPvcI

### How to use: 

1. Create an empty gameobject.
2. Add the script-component "Play Random Sound".
3. Adjust the Size, Timing and AudioSource-settings in the script-component.
4. Add your audioclips to the Clips-array.
5. Hit play and test out your settings 😊.

### Extra:

6. You can edit the settings on the go whilst in runtime. Meaning you do not have to adjust settings before hitting play.
7. Adjusting the settings during runtime also applies to changing the array of clips playing.
