using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitData {
    public int damage;
    public Vector3 push;

    public HitData (int damage) {
        this.damage = damage;
        push = Vector3.zero;
    }
    public HitData(int damage,Vector3 push) {
        this.damage = damage;
        this.push = push;
    }

}

public interface IDamageable {
    public void TakeDamage(HitData hit);
}