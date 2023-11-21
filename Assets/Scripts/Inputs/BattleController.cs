using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleController : ExplorationController
{
    [SerializeField] protected List<Entity> companions = new ();
}
