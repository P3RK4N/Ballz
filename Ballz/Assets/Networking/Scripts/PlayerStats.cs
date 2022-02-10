using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float mass;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        mass = rb.mass * 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateMassTag()
    {
        mass = rb.mass * 100;
    }

    public void damagePlayer(float damage)
    {
        GetComponentInChildren<MultiplayerMovement>().increaseDamage(damage);
        updateMassTag();
        GetComponentInChildren<MultiplayerMovement>().recalculateResistanceFactor();
    }
}
