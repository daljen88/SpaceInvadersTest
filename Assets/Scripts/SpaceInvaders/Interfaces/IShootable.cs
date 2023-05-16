using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable
{
    void Shoot(int damage);
    void Shoot(Vector3 direction, int damage);



}
