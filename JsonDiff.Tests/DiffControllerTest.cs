using System.Net;
using JsonDiff.Controllers;
using JsonDiff.Models;
using JsonDiff.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace JsonDiff.Tests
{
    public class DiffControllerTest
    {
        private readonly DiffController _diffController;
        private readonly Mock<IPersistenceService> _sessionPersistenceServiceMock;

        public DiffControllerTest()
        {
            _sessionPersistenceServiceMock =  new Mock<IPersistenceService>();
            _diffController = new DiffController(new JsonDiffService(), _sessionPersistenceServiceMock.Object);
        }


        [Fact]
        public void Left_InvalidInput_ReturnBadRequest()
        {
            //Arrange
            var encodedData = "not_a_json".Base64Encode();

            //Act
            var response = _diffController.Left(encodedData) as ObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            _sessionPersistenceServiceMock.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Right_InvalidInput_ReturnBadRequest()
        {
            //Arrange
            var encodedData = "not_a_json".Base64Encode();

            //Act
            var response = _diffController.Right(encodedData) as ObjectResult;
            
            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, response.StatusCode);
            _sessionPersistenceServiceMock.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void Left_ValidInput_ReturnOk()
        {
            //Arrange
            var obj = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var json = JsonConvert.SerializeObject(obj);
            var encodedData = json.Base64Encode();

            //Act
            var response = _diffController.Left(encodedData) as ObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            _sessionPersistenceServiceMock.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Right_ValidInput_ReturnOk()
        {
            //Arrange
            var obj = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var json = JsonConvert.SerializeObject(obj);
            var encodedData = json.Base64Encode();

            //Act
            var response = _diffController.Right(encodedData) as ObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            _sessionPersistenceServiceMock.Verify(m => m.Save(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void Diff_ValidInputDifferentJson_ReturnOkButNotEqual()
        {
            //Arrange
            var obj1 = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var json1 = JsonConvert.SerializeObject(obj1);

            var obj2 = new { prop1 = "val2", prop2 = "val2", prop3 = new { nested1 = "n2", nested2 = new { nested3 = "n3" } } };
            var json2 = JsonConvert.SerializeObject(obj2);

            _sessionPersistenceServiceMock.Setup(x => x.Get("LeftData")).Returns(json1);
            _sessionPersistenceServiceMock.Setup(x => x.Get("RightData")).Returns(json2);

            //Act
            var response = _diffController.Get() as ObjectResult;
            var responseValue = response.Value as DiffResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.False(responseValue.AreEqual);
        }

        [Fact]
        public void Diff_ValidInputSameJson_ReturnOkAndEqual()
        {
            //Arrange
            var obj1 = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var json1 = JsonConvert.SerializeObject(obj1);

            _sessionPersistenceServiceMock.Setup(x => x.Get("LeftData")).Returns(json1);
            _sessionPersistenceServiceMock.Setup(x => x.Get("RightData")).Returns(json1);

            //Act
            var response = _diffController.Get() as ObjectResult;
            var responseValue = response.Value as DiffResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, response.StatusCode);
            Assert.True(responseValue.AreEqual);
        }
    }
}
