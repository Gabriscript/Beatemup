using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIhealthbar : MonoBehaviour
{
    public Transform target;
   public  Vector3 offset;
    public Gradient gradient;
    public Image fill;
    public Slider slider;
    void LateUpdate()
    {
        Vector3 direction = (target.position - Camera.main.transform.position).normalized;
        bool isBehind = Vector3.Dot(direction, Camera.main.transform.forward) <= 0f;
        fill.enabled = !isBehind;
      transform.position = Camera.main.WorldToScreenPoint(target.position+offset);
    }
    public void SetHealth(int health) {
        slider.value = health;
       fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    public void SetMaxHealth(int health) {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);

    }
}
