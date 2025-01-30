using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class BossFightDialogueLine : DialogueLine
{
    public DoorController door;
    public NecromancerAI arya;
    public CameraDock fightDock;

    public override void Deactivate()
    {
        base.Deactivate();
        door.CloseDoor();
        fightDock.Activate();
        CheckpointManager.Instance.Save();
        arya.enabled = true;
    }
}
