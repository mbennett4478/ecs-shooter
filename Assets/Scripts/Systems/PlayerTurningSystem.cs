using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PlayerTurningSystem : ComponentSystem
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
        var mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }

        var camRayLen = SurvivalShooterBootstrap.Settings.CamRayLen;
        var floor = LayerMask.GetMask("Floor");
        
        Entities.With(_entityQuery).ForEach((Entity entity, ref PlayerInputData playerInputData, Rigidbody rigidbody) =>
        {
            var mousePosition = new Vector3(playerInputData.Look.x, playerInputData.Look.y, 0);
            var camRay = mainCamera.ScreenPointToRay(mousePosition);
            RaycastHit floorHit;
            if (Physics.Raycast(camRay, out floorHit, camRayLen, floor))
            {
                var position = rigidbody.gameObject.transform.position;
                var playerToMouse = floorHit.point - new Vector3(position.x, position.y, position.z);
                playerToMouse.y = 0f;
                var newRotation = Quaternion.LookRotation(playerToMouse);
                rigidbody.MoveRotation(newRotation);
            }
        });
    }
}
