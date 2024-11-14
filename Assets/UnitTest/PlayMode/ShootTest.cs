using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
public class ShootTest : MonoBehaviourBaseTest {

    [UnityTest]
    // No vamos a disparar en diagonal
    [TestCase(new float[] {0.0f,1.0f}, 10, ExpectedResult = 0, TestName = "Range 10 up")]
    [TestCase(new float[] {0.0f,-1.0f}, 20, ExpectedResult = 0, TestName = "Range 20 down")]
    [TestCase(new float[] {1.0f,.0f}, 30, ExpectedResult = 0, TestName = "Range 30 right")]
    [TestCase(new float[] {-1.0f,.0f}, 40, ExpectedResult = 0, TestName = "Range 40 left")]
    public IEnumerator BulletRangeTest(float[] dir, float range) {
        // Configuración inicial de la dirección, origen y rango
        Vector3 direction = new Vector3(dir[0],dir[1],0.0f);
        Vector3 origin = new Vector3(0.0f,0.0f,0.0f);

        Rigidbody2D body = gameObject.AddComponent<Rigidbody2D>();
        body.gravityScale = 0.0f;  // Desactivar la gravedad

        Bullet bullet = gameObject.AddComponent<Bullet>();

        yield return new WaitForEndOfFrame();

        gameObject.SetActive(true);

        // Ejecutar el método shoot para disparar la bala
        bullet.shoot(direction, origin, range, 1);

        // Esperar a que la bala llegue a su rango máximo
        yield return new WaitForSeconds(range);

        // Comprobar que la posición final está en el rango esperado
        Assert.AreEqual(range, bullet.transform.position.magnitude,0.5f, "La bala no alcanzó la posición esperada.");
    }
}
