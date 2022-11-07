using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDmgable
{
    public void ApplyDamage(int dmg, ElementType elementType);
    public void Died();
}
