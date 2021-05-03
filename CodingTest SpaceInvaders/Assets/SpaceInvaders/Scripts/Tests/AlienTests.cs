using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SpaceInvaders.Scripts.Invasion;
using UnityEngine;
using UnityEngine.TestTools;

namespace SpaceInvaders.Scripts.Tests
{
    public class AlienTests
    {
        private Alien alien;

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
        public IEnumerator Should_ChangeDirection_IfOnTheBorder()
        {
            // Arrange
            invasionManagerMock.AliensDirection.Returns(InvasionManager.Direction.Left);
            invasionManagerMock.IsInPlayState().Returns(true);
            float borderPosition = -10f;
            invasionManagerMock.LeftBorderPosition.Returns(borderPosition);

            // Act
            yield return new WaitForEndOfFrame();
            alien.transform.position = new Vector3(borderPosition, 0f, 0f);
            yield return new WaitForEndOfFrame();
            
            // Assert
            Assert.IsTrue(invasionManagerMock.AliensDirection == InvasionManager.Direction.Right);
        }
    }
}