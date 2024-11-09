using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;

[TestFixture]
public class PlayerStateMachine :  InputTestFixture {

    private GameObject gamePlayer;
    private Player player;
    private GameObject plataforma;
    private Keyboard keyboard;

    [SetUp]
    public void SetUp(){
        keyboard = InputSystem.AddDevice<Keyboard>();
        
        gamePlayer = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        player = gamePlayer.GetComponent<Player>();

        plataforma = new GameObject("plataforma");
        plataforma.AddComponent<BoxCollider2D>().size = new Vector2(10,1);
        plataforma.transform.position = new Vector3(0,-1,0);
        plataforma.layer = LayerMask.NameToLayer("Ground");

    }

    
    [TearDown]
    public override void TearDown(){
        // Limpia el teclado y reinicia el sistema de entrada
        if (keyboard != null) {
            Release(keyboard.spaceKey);
            Release(keyboard.aKey);
            InputSystem.RemoveDevice(keyboard);
            keyboard = null;
        }

        // Resetea el sistema de entrada y espera un cuadro para estabilidad
        InputSystem.ResetHaptics();

        if(gamePlayer != null){
            GameObject.DestroyImmediate(gamePlayer);
            player = null;
        }

        if(plataforma != null)
            GameObject.DestroyImmediate(plataforma);
    }
 
    [UnityTest]
    public IEnumerator AirState(){
        gamePlayer.transform.position = new Vector3(10,0,0);
        
        yield return new WaitForSeconds(1);

        Assert.AreEqual(player.InAirState,player.StateMachine.CurrentState);
    }

    [UnityTest]
    public IEnumerator IdleState(){  
        yield return new WaitForSeconds(1);

        Assert.AreEqual(player.IdleState,player.StateMachine.CurrentState);
    }

    [UnityTest]
    public IEnumerator JumpState(){
        keyboard = InputSystem.GetDevice<Keyboard>();

        yield return new WaitForSeconds(1);

        Press(keyboard.spaceKey);
        InputSystem.Update();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.AreEqual(player.JumpState,player.StateMachine.CurrentState);
    }

    [UnityTest]
    public IEnumerator MoveState(){
        keyboard = InputSystem.GetDevice<Keyboard>();

        yield return new WaitForSeconds(1);

        Press(keyboard.aKey);
        InputSystem.Update();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.AreEqual(player.MoveState,player.StateMachine.CurrentState);
    }

    [UnityTest]
    public IEnumerator DashState(){
        keyboard = InputSystem.GetDevice<Keyboard>();

        yield return new WaitForSeconds(1);

        PressAndRelease(keyboard.eKey);
        InputSystem.Update();

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        Assert.AreEqual(player.DashState,player.StateMachine.CurrentState);
    }
}
