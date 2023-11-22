using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BattleController : ExplorationController
{
    [HideInInspector] public List<PlayerEntity> companions;
}
