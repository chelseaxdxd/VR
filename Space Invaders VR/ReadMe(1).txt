Assignment 1 - Space Invader VR
0511606 陳薀涵

1. Design your own (visual/sound/particle) effect when an enemy being shot.

	visual/particle effect:
	In EnemyController, when the third time the enemy is shot, particle system would be triggered and cause an explosion effect.  
	sound effect:
	In EnemyController, when the enemy is shot, an audio clip would be played via Play() in KillEnemy() function.
	

2. Make the bullet’s area of effect wider.

	In Bullet Prefab and its Sphere Collider component, increase its radius to 10.


3. HP system – the enemies should be attacked three times to disappear.

	change the scale of enemy by 0.2 every time it is shot.


4. Your own idea ...

	In GameManager, add an audio source linked with an audio clip, and call Play() in Start().

