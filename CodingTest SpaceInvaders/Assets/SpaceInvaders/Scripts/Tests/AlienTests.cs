using System.Collections;
using NSubstitute;
using NUnit.Framework;
using SpaceInvaders.Scripts.Invasion;
using UnityEngine;
using UnityEngine.TestTools;

namespace SpaceInvaders.Scripts.Tests
{
    /// <summary>
    ///     Test class for <see cref="Alien"/>
    /// </summary>
    public class AlienTests
    {
        /// <summary>
        ///     Alient instance used for the tests.
        /// </summary>
        private Alien alien;

        /// <summary>
        ///     Mock (substitution) of the InvasionManager.
        /// </summary>
        private AbstractInvasion invasionManagerMock;

        [OneTimeSetUp]
        public void Setup()
        {
            alien = new GameObject().AddComponent<Alien>();
            invasionManagerMock = Substitute.For<AbstractInvasion>();
            ServiceLocator.Register(invasionManagerMock);
        }

        [UnityTest]
        public IEnumerator Should_ChangeDirection_When_OnTheBorder()
        {
            // Arrange
            invasionManagerMock.IsInPlayState().Returns(true);
            invasionManagerMock.AliensDirection = AbstractInvasion.Direction.Left;
            float borderPosition = -10f;
            invasionManagerMock.LeftBorderPosition.Returns(borderPosition);
            yield return new WaitForEndOfFrame();

            // Act
            alien.transform.position = new Vector3(borderPosition, 0f, 0f);
            yield return new WaitForEndOfFrame();
            
            // Assert
            Assert.IsTrue(invasionManagerMock.AliensDirection == AbstractInvasion.Direction.Right);
        }

        [UnityTest]
        public IEnumerator Should_Die_When_HitByProjectile()
        {
            // Arrange
            UserInterfaceManager userInterfaceManagerMock = Substitute.For<UserInterfaceManager>();
            ServiceLocator.Register(userInterfaceManagerMock);
            yield return new WaitForEndOfFrame();

            // Act
            alien.OnProjectileHit();
            yield return new WaitForEndOfFrame();

            // Assert
            Assert.IsFalse(alien.gameObject.activeInHierarchy);
            ServiceLocator.Unregister<UserInterfaceManager>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            ServiceLocator.Unregister<AbstractInvasion>();
        }
    }
}
