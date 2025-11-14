using UnityEngine;

public class Jefe1 : Enemy
{
    // The Awake method is called when the script instance is being loaded.
    // We call base.Awake() to ensure any initialization in the parent class (Enemy) is performed.
    protected override void Awake()
    {
        base.Awake();

        // Add specific initialization logic for Jefe1 here
        // For example:
        // health = 500;
        // speed = 2.0f; 
    }

    // The Update method is called every frame.
    // We call base.Update() to ensure any frame-to-frame logic in the parent class (Enemy) is performed.
    protected override void Update()
    {
        base.Update();
        
        // Add specific update/game loop logic for Jefe1 here
        // For example:
        // if (target != null)
        // {
        //     MoveTowards(target.position);
        // }
    }
    
    // You can add more boss-specific methods here, like:
    // private void ShootSpecialAbility() 
    // {
    //     // Logic for a special attack
    // }
}