using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class PlayerFallingTest : MonoBehaviourBaseTest {

    [UnityTest]
    [TestCase(1.0f,  ExpectedResult = 0, TestName = "Fall 1 second")]
    [TestCase(10.0f, ExpectedResult = 0, TestName = "Fall 10 seconds")]
    [TestCase(30.0f, ExpectedResult = 0, TestName = "Fall 30 seconds")]
    [TestCase(60.0f, ExpectedResult = 0, TestName = "Fall 60 seconds")]
    public IEnumerator PlayerMaxVelocityTest(float seconds) {

      Rigidbody2D body = gameObject.AddComponent<Rigidbody2D>();

      gameObject.AddComponent<CapsuleCollider2D>();
      gameObject.AddComponent<Jump>();

      yield return new WaitForEndOfFrame();

      gameObject.SetActive(true);
      
      yield return new WaitForSeconds(seconds);
      
      Assert.LessOrEqual(-45.0f,body.velocity.y, "El jugador va mas rapido");
    }
}
