using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RunFixedUpdateSystems : MonoBehaviour
{
    private PlayerInputSystem _playerInputSystem;
    private PlayerMovementSystem _playerMovementSystem;
    private PlayerTurningSystem _playerTurningSystem;

    private void Start()
    {
        _playerInputSystem = World.Active.GetOrCreateSystem<PlayerInputSystem>();
        _playerMovementSystem = World.Active.GetOrCreateSystem<PlayerMovementSystem>();
        _playerTurningSystem = World.Active.GetOrCreateSystem<PlayerTurningSystem>();
    }

    private void FixedUpdate()
    {
        _playerInputSystem.Update();
        _playerMovementSystem.Update();
        _playerTurningSystem.Update();
    }
}
