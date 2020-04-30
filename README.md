# No School, No Peace
Our remote repository location: https://github.com/kenny-kelley/accad-7504.

## To get started...
1. Download Unity Editor Version 2019.1.9f from https://unity3d.com/get-unity/download/archive.
2. Clone our repository using:
`$ git clone https://github.com/kenny-kelley/accad-7504`
3. Open the Unity project within the Unity Editor.

## General Code Overview
We make use of "Box Triggers". These are Box GameObjects which do not render but do listen for collisions. In laymans terms, they are invisible spaces that run a script when they are entered by a player or NPC.

You will notice 13 scripts attached to these Box Triggers. Each one of these scripts is numbered and attached to a unique Box Trigger.

You will also find scripts for the characters Abdoul, Amina, Soldier 1, and Soldier 2, as well as another script to control the movement of the doors. An API of the characters and an outline of each Box Trigger is provided below.

## Box Trigger Outline

### Trigger 1
When the player walks in front of the mirror, Amina asks "Where are you?".

### Trigger 2
When the player approaches the closet door, Amina walks towards the door.

### Trigger 3
When Amina reaches the closet, she stops and says to come along. She then turns around and walks back into the classroom.

### Trigger 4
When Amina reaches the closest desk to the front, she angles herself more towards the front of the classroom and walks there.

### Trigger 5
When Amina reaches the front, she stops, and a timer begins.
The screen fades out after 13 seconds. Half the students are removed. The player is relocated to a desk.
The screen fades in after 15 seconds. Amina takes attendance.
Abdoul tells the story after 31 seconds.
Amina interrupts Abdoul after 44 seconds.
Abdoul finishes telling the story after 45 seconds.
Amina interrupts Abdoul and Mariam again after 55 seconds.
Abdoul apologizes after 57 seconds.
Amina orders Mariam to get chalk at 59 seconds, and repeatedly orders her if she doesn't after 20 seconds.

### Trigger 6
When the player reaches the closet to get chalk, the motorcycle audio plays. A timer begins.
Amina tells the children to hide and the village background audio stops after 10.5 seconds.
Abdoul asks "What's going on" after 14 seconds.
Amina tells the children "The soldiers are back" and the soldiers enter the school after 15.5 seconds.

### Trigger 7
When Amina slides back towards the desk, she stops.

### Trigger 8
When Soldier 1 is adequately in the building, he stops.

### Trigger 9
When Soldier 2 is adequately in the building, he stops.
The four NPCs begin walking out.
Soldier 2 begins walking toward the closet.

### Trigger 10
When Soldier 2 makes it down the aisle, he tells Abdoul to move it.

### Trigger 11
There are four Box Triggers that have this script attached to them. Each of them are placed in the aisle to begin the students movement towards the front.
When the students (including Abdoul) reach these, they begin their movement to the front and out of the classroom.

### Trigger 12
When the students leave the classroom, it updates a field noting that they have made it out of the building.

### Trigger 13
When all of the students have left the classroom, Soldier 1 says to "Burn it" and fire begins to appear.
The scene comes to an end.
