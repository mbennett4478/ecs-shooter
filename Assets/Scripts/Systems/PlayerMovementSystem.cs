using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PlayerMovementSystem : ComponentSystem
{
    private EntityQuery _entityQuery;

    protected override void OnCreate()
    {
        _entityQuery = GetEntityQuery(
            ComponentType.ReadOnly<Transform>(),
            ComponentType.ReadOnly<PlayerInputData>(),
            ComponentType.ReadOnly<Rigidbody>()
        );
    }

    protected override void OnUpdate()
    {
        var speed = SurvivalShooterBootstrap.Settings.PlayerMoveSpeed;
        var dt = Time.deltaTime;
        
        Entities.With(_entityQuery).ForEach((Entity entity, Rigidbody rigidbody, ref PlayerInputData playerInputData) =>
        {
            var move = playerInputData.Move;
            var movement = new Vector3(move.x, 0, move.y);
            movement = dt * speed * movement.normalized;
            var gameObject = rigidbody.gameObject;
            var position = gameObject.transform.position;
            var newPosition = new Vector3(position.x, position.y, position.z) + movement;
            rigidbody.MovePosition(newPosition);
        });
    }
}
