using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
  public new string name;

  public Sprite artwork;

  public float _damage;
  public float _cooldown;
  public float _speed;
  public float _range;

}
