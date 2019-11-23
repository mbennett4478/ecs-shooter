using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[DisableAutoCreation]
public class PlayerInputSystem : JobComponentSystem
{
    private EndSimulationEntityCommandBufferSystem barrier;
    
    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _shootAction;

    private float2 _moveInput;
    private float2 _lookInput;
    private float _shootInput;
    
    protected override void OnCreate()
    {
        barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnStartRunning()
    {
        _moveAction = new InputAction("move", binding:"<Gamepad>/rightStick");
        _moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard/a")
            .With("Right", "<Keyboard>/d");

        _moveAction.performed += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.canceled += context => { _moveInput = context.ReadValue<Vector2>(); };
        _moveAction.Enable();
        
        _lookAction = new InputAction("look", binding: "<Mouse>/position");
        _lookAction.performed += context => { _lookInput = context.ReadValue<Vector2>(); };
        _lookAction.canceled += context => { _lookInput = context.ReadValue<Vector2>(); };
        _lookAction.Enable();
        
        _shootAction = new InputAction("shoot", binding: "<Mouse>/leftButton");
        _shootAction.performed += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.canceled += context => { _shootInput = context.ReadValue<float>(); };
        _shootAction.Enable();       
    }

    protected override void OnStopRunning()
    {
        _shootAction.Disable();
        _lookAction.Disable();
        _moveAction.Disable();
    }

    [BurstCompile]
    private struct PlayerInputJob : IJobForEach<PlayerInputData>
    {
        [ReadOnly]
        public float2 MoveInput;
        [ReadOnly]
        public float2 LookInput;
        [ReadOnly]
        public float ShootInput;

        public void Execute(ref PlayerInputData inputData)
        {
            inputData.Move = MoveInput;
            inputData.Look = LookInput;
            inputData.Shoot = ShootInput;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new PlayerInputJob
        {
            MoveInput = _moveInput,
            LookInput = _lookInput,
            ShootInput = _shootInput
        };

        inputDeps = job.Schedule(this, inputDeps);
        barrier.AddJobHandleForProducer(inputDeps);
        return inputDeps;
    }
}
