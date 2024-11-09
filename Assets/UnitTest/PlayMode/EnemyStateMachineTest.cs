using System;
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class EnemyStateMachineTest : MonoBehaviourBaseTest {

    private GameObject player;
    private StateManager npc;

    [SetUp]
    public override void SetUp(){
        gameObject = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Enemy2"));
        gameObject.transform.position = Vector3.zero;
        npc = gameObject.GetComponent<StateManager>();

        player = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player.transform.position = new Vector3(100,-100,100);
    }

    [TearDown]
    public override void TearDown(){
        if(gameObject != null)
            GameObject.DestroyImmediate(gameObject);

        if(player != null)
            GameObject.DestroyImmediate(player);
    }

    [UnityTest]
    public IEnumerator MoveState() {
        yield return new WaitForEndOfFrame();

        Assert.AreEqual(npc.moveState,npc.currentState);
    }

    [UnityTest]
    public IEnumerator IdleState() {
        GameObject plataforma = new GameObject("plataforma");
        plataforma.AddComponent<BoxCollider2D>().size = new Vector2(1,10);
        plataforma.transform.position = new Vector3(-10,0,0);
        plataforma.tag = "Platform";
        gameObject.transform.position = new Vector3(-9.5f,0,0);

        yield return new WaitForSeconds(1);

        Assert.AreEqual(npc.idleState,npc.currentState);

        GameObject.DestroyImmediate(plataforma);
    }

    [UnityTest]
    public IEnumerator FocusedState() {
        GameObject plataforma = new GameObject("plataforma");
        plataforma.AddComponent<BoxCollider2D>().size = new Vector2(10,1);
        plataforma.transform.position = new Vector3(10,0,0);
        plataforma.tag = "Platform";

        player.transform.position = new Vector3(5,1f,0);

        yield return new WaitForSeconds(1);

        Assert.AreEqual(npc.focusedState,npc.currentState);

        GameObject.DestroyImmediate(plataforma);
    }

    [UnityTest]
    public IEnumerator DeathState() {
        Vector3 direction = new Vector3(1,0,0);
        Vector3 origin = new Vector3(-5,0,0);

        GameObject bulletGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));
        bulletGO.transform.position = origin;
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        yield return new WaitForEndOfFrame();
        
        bulletGO.SetActive(true);

        // Ejecutar el método shoot para disparar la bala
        bullet.shoot(direction, origin, 10000, 1000);

        yield return new WaitForSeconds(2);

        Assert.AreEqual(npc.deadState,npc.currentState);

        GameObject.DestroyImmediate(bulletGO);
    }
}
