# King of Kitchen
KOK is the VR-Game that was impromented by students in the falculty of ICT, Mahidol. It can play with HTC Vive which is the most popular device of VR controller. The story of this game is about a mouse finding and stealing foods in the kitchen. In this game, the player will be immersed to be a mouse in the game. The player has to steal foods in the kitchen as much as possible in the limited time. There is the challenge for the player to avoid collecting the spoiled foods that would deduct the score.

See the demo video here :mouse: :hamburger: :

<a href="http://www.youtube.com/watch?feature=player_embedded&v=OVrd-KMD-dM" target="_blank"><img src="http://img.youtube.com/vi/OVrd-KMD-dM/0.jpg" alt="IMAGE ALT TEXT HERE" width="800" height="420" border="10" /></a>

# Propose of the project
This game is project from ITCS496_Special Topics in Multimedia Systems. The propose to imprement VR-Game and for learning in the VR techniquies which are Ray-Casting and Homer as selection techniques.


# Design
According to the game story, the objectives of this game was designed to let the player find and steal foods easily. The main techniques are separated into 2 parts which are selection techniques and navigation technique.

For selection techniques, we choose Simple Ray-Casting and Homer technique because we need to design the First-person view (FPV) that can simulate a player controller by his hands. When the player moves the hands, controllers will generate the ray pointing out from the controller. The player can point to the food that they want and then select it. From this point, the game was designed two modes for selection. If the player selects the food by Simple Ray-Casting mode, the food location will change follow the controller in the real distance. If the player selects the food by Homer mode, the food location will change follow the controller in the scale distance. For the scale, it was calculated from the distance of the positions between the controller and headset that ignore the Y-axis. Additional, the player can switch the mode during moving or selecting.

For navigation technique, we choose Grab World technique because it can make the player feel moving in the real world by using the VR controller. In the scene, we set the object that allows the player can select for move forward and jump up. If the player selects on the movable object and drags the controller, the player will move forward and it will faster follow the drag distance. If the player selects on the jumpable object and drags the controller, the player will jump up as well.

# Implementation Details
For the game concept, there are two modes for selection which are simple ray-casting mode and homer mode. These two modes are used for selecting foods in the scene. The player can switch to homer mode to pull the objects that are far from the player. To switch selection mode, the player has to press the grip on the side of the controller. When food is selected, the player will see the particle feedback on the object that helps the player to know whether the object is picked. In order to walk around and climb the furniture in the scene, we use Grab World technique that the player has to point to the place the player wants to move to. Then, press the trigger on the controller to move. The functions implemented for Grab World technique. In the next paragraph, it is the explanation for implementing the interaction techniques by using Unity and SteamVR

![Screenshot](\resources\images\Kitchen_1.PNG)

Unity is a game maker that has a free version for personal use. For create VR game, Unity also has free package name “SteamVR” for connecting Unity and VR devices such as HTC Vive to work together. SteamVR package has a lot of that very useful for create VR game. There are scripts basic models, materials, resources, scripts, and prefabs. Although SteamVR has a sample scene to learn about basic technique such as teleport or throw an object, It not enough to implement a unique VR game. We have to learn how SteamVR works in Unity program and then create own scripts follow game concept as well. First, we try to get input from VR controller by import SteamVR library call “Valve.VR” and then get input value from “SteamVR_Input” class. After we get input from the controllers, we try to create ray-casting for getting any object that ray hit by using “RaycastHit” from Unity. This function can get common value from an object that was hit are distance, position point, normal point, and Game object. From this stage, we plan to create scripts for controlling objects in this project. Scripts can separate into seven scripts are follow:
- **GameInfo** - link all scripts together and contain a common method
- **GenerateFood** - generate random food from the position set
- **Hand** - ray process, selection and navigation technique, and holding process
- **Player** - control the rigid body of the player object
- **Bag** - food collecting process and add the score
- **Food** - control food movement and control food score
- **EndScript** - link information between scenes

Next, it is the process of creating the scenes and objects are follow:

- Scenes:
    - **Start_Scene** - menu scene before the game start

    ![Screenshot](\resources\images\startScene_1.PNG)
    - **Main_Scene** - the main scene to play the game

    ![Screenshot](\resources\images\Select_RayCasting_1.PNG)
    - **End_Scene** - scene for restart game and show score

    ![Screenshot](\resources\images\endScene_1.PNG)
- Objects:
    - **Food objects** - a score object

    ![Screenshot](\resources\images\Food_colliderObject_1.PNG)
    - Kitchen room object
        - **Movable** - an object that player can move'

        ![Screenshot](\resources\images\Movable_1.PNG)
        - **Jumpable** - an object that player can jump

        ![Screenshot](\resources\images\Jumpable_1.PNG)

#Discussion
- **lessons learned**
    - How VR work in the real world
    - Selection technique in VR
    - How to implement a game in Unity
    - Improve programming skill
- **suggested guideline**
    - Setup Program for VR-Game making
        - Install Unity 2018.12.x version for personal used
            - After installing Unity already, create a new project and install SteamVR Package in this project
        - Install Steam v018 or newer version
            - After installing Steam already, Install SteamVR 1.2.3 version
        - **Additional**, Install Visual Studio 2017 or newer
    - Implementation part
        - Input and Output from HTC-Vive controller
            - Import SteamVR library
            ```c#
            using Valve.VR;
            ```
            - Call SteamVR input function (return boolean type)
            ```c#
            SteamVR_Input._default.inActions.__INPUTNAME__.GetStateDown(SteamVR_Input_Sources.__HANDNAME__);
            ```
                “__INPUTNAME__” can change to any input type from the controller
                “__HANDNAME__” can change to specific hand such as “Any”, “Left” and “Right”

- Main challenges during the implementation
    - Computer and HTC-Vive (Only one machine)
    - Selection structure design in own script
    - Difference SteamVR version
    - Local pivot point rotation
- Current limitations
    - Cannot select the food during moving (the food will follow the player)

# Future work
- Improve Game Graphics
    - Models and Scene
    - Animation of the objects
    - Lighting
    - Particle effect in the scene
    - Menu UI
- Improve Controlling
    - Smooth moving and jumping
    - Smooth Homer selecting
- Improve Menu and Setting
    - Select map,  level, and Time 
    - Graphics setting
    - Credit
- Increase challenges
    - Enemy
    - Extreme scene
- Multi-player
    - Two player modes

# Team Member
Parintorn  Pooyoi           5888149
Suchakree  Sawangwong       5888170
Witchayaporn  Suriyun       5888186
Amonnat  Tengputtipong      5888202
Faculty of Information and Communication Technology, Mahidol University
# Status
**Completed**
