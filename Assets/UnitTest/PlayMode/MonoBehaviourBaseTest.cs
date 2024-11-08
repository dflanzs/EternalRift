using System.Linq;
using NUnit.Framework;
using UnityEngine;

public abstract class MonoBehaviourBaseTest {

    protected GameObject gameObject;

    [SetUp]
    public virtual void SetUp(){
        gameObject = new GameObject("object");
    }

    [TearDown]
    public virtual void TearDown(){
        if(gameObject != null)
            Object.DestroyImmediate(gameObject);
    }
}
