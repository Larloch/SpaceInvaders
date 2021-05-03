using System.Collections;
using System.Collections.Generic;
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
        private InvasionManager invasionManagerMock;

        [OneTimeSetUp]
        public void Setup()
        {
            new GameObject().AddComponent<ServiceLocator>();
            alien = new GameObject().AddComponent<Alien>();
            invasionManagerMock = Substitute.For<InvasionManager>();
            ServiceLocator.Register(invasionManagerMock);
        }

        [UnityTest]
        public IEnumerator Should_ChangeDirection_When_OnTheBorder()
        {
            // Arrange
            invasionManagerMock.AliensDirection.Returns(InvasionManager.Direction.Left);
            invasionManagerMock.IsInPlayState().Returns(true);
            float borderPosition = -10f;
            invasionManagerMock.LeftBorderPosition.Returns(borderPosition);
            yield return new WaitForEndOfFrame();

            // Act
            alien.transform.position = new Vector3(borderPosition, 0f, 0f);
            yield return new WaitForEndOfFrame();
            
            // Assert
            Assert.IsTrue(invasionManagerMock.AliensDirection == InvasionManager.Direction.Right);
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
            ServiceLocator.Unregister<InvasionManager>();
        }
    }
}
