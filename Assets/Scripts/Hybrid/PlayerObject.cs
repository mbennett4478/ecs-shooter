using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerObject : MonoBehaviour, IConvertGameObjectToEntity
{
    public Entity playerEntity;
    public Transform GunPivot;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var settings = SurvivalShooterBootstrap.Settings;
        dstManager.AddComponentData(entity, new PlayerInputData { Move = new float2(0, 0)});
        playerEntity = entity;
    }
}
