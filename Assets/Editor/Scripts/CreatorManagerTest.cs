using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class CreatorManagerTest : MonoBehaviour
{
    [Test]
    public void SetPrefab_Test()
    {
        // ARRANGE
        var creatorManager = new CreatorManager();
        creatorManager.objectType = "chair_1";

        // ACT
        var result = creatorManager.SetPrefab();

        // ASSERT
        Assert.IsNotNull(result);
    }

    [Test]
    public void SelectFeatures_Test()
    {
        // ARRANGE
        var creatorManager = new CreatorManager();
        creatorManager.prefab = new GameObject();

        // ACT
        creatorManager.SelectFeatures();

        // ASSERT
        Assert.IsNotNull(creatorManager.features);
    }
}
