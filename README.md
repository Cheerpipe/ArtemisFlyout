# ¿What is this?

Artemis Flyout is a remote control for the best OpenSource RGB Control and Engine Effect out there https://artemis-rgb.com/ but it doesn’t control light in a direct way, instead, it is a frontend for the JSON Datamodel Plugin for creating custom Datamodel values that user can use to control lights using Artemis Databinding and Conditions. 
Artemis Flyout UI was created using https://avaloniaui.net/ as a base framework and https://github.com/amwx/FluentAvalonia for the Fluent lookd and feel. It also use other magnific projects like http://wieslawsoltes.github.io/AvaloniaBehaviors/ and https://restsharp.dev/.
It is important to note that i am learning how to write MVVM and Avalonia so architecture may be a mess. Still... it works.

For example:

 * Meeting button will toggle true and false Meeting value in the Global Variables Datamodel. User will be responsible to use that value to turn profiles/layers on and off. This will result in a more work at the beginning but ensures full control.
* Bright slider is another control: You can create a full black layer as a overlay and bind GlobalGright value with layer opacity in an inverse way so when Bright is at value 100 black layer opacity will be 0 (or you can bind bright value to control a folder opacity directly).

This project is a WIP so it will need some user effort and code reading to get it working. Here are some tips for you:

* Device Toggles and custom profile color slots are configured in "appsettings.json" file. Each entry will create a Datamodel value in Device in Devices States and Custom profile colors datamodels.
* Profiles combobox is filled with profile names from the Artemis Category "Ambient". You can query for a different category changing the value AmbientProfileCategoryName in "appsettings.json". Rememer that you have to create display conditions using the Profile Datamodel value.

## ArtemisFlyout for use with Artemis RGB. Must be used with the following plugins:

 * JSON Plugin : https://github.com/Cheerpipe/Artemis.Plugins.Public/tree/master/src/Modules/Artemis.Plugins.Modules.Json
 * Extended REST API : https://github.com/Cheerpipe/Artemis.Plugins.Public/tree/master/src/Collections/Artemis.Plugins.ExtendedWebAPI

## Download links:

* Plugin download links: https://nightly.link/Cheerpipe/Artemis.Plugins.Public/workflows/plugins/master

* Artemis Flyout download link: You have to build it yourself until it is finished :(


## Demo video:

https://user-images.githubusercontent.com/972765/142269836-f5f68be1-1c1e-43d4-b895-caf9d15298c1.mp4

