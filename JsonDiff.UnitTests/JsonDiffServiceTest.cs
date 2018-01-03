using System;
using JsonDiff.Services;
using Newtonsoft.Json;
using Xunit;

namespace JsonDiff.Tests
{
    public class JsonDiffServiceTest
    {
        private readonly JsonDiffService _jsonDiffService;

        public JsonDiffServiceTest()
        {
            _jsonDiffService = new JsonDiffService();
        }


        [Fact]

        public void ValidateInput_DataIsNotBase64_ThrowsException()
        {
            //Arrange
            var data = "non_base64_string";

            //Act 

            //Assert
            Assert.Throws<ArgumentException>(() => _jsonDiffService.ValidateInput(data));

        }

        [Fact]
        public void ValidateInput_InvalidJson_ThrowsException()
        {
            //Arrange
            var encodedData = "not_a_json".Base64Encode();

            //Act 

            //Assert
            Assert.Throws<ArgumentException>(() => _jsonDiffService.ValidateInput(encodedData));

        }

        [Fact]
        public void ValidateInput_ValidSimpleJson_ReturnTrue()
        {
            //Arrange
            var obj = new { prop1 = "val1", prop2 = "val2" };
            var json = JsonConvert.SerializeObject(obj);
            var encodedData = json.Base64Encode();

            //Act 
            var result = _jsonDiffService.ValidateInput(encodedData);

            //Assert
            Assert.True(result);

        }

        [Fact]
        public void ValidateInput_ValidNestedJson_ReturnTrue()
        {
            //Arrange
            var obj = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var json = JsonConvert.SerializeObject(obj);
            var encodedData = json.Base64Encode();

            //Act 
            var result = _jsonDiffService.ValidateInput(encodedData);

            //Assert
            Assert.True(result);
        }

        [Fact]
        public void DiffJson_MissingLeft_ThrowException()
        {
            //Arrange
            var objRight = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var jsonRight = JsonConvert.SerializeObject(objRight);

            //Act 

            //Assert
            Assert.Throws<ArgumentNullException>(() => _jsonDiffService.DiffJson(null, jsonRight));
        }

        [Fact]
        public void DiffJson_MissingRight_ThrowException()
        {
            //Arrange
            var objLeft = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var jsonLeft = JsonConvert.SerializeObject(objLeft);

            //Act 

            //Assert
            Assert.Throws<ArgumentNullException>(() => _jsonDiffService.DiffJson(jsonLeft, null));
        }

        [Fact]
        public void DiffJson_SameObject_ReturnTrue()
        {
            //Arrange
            var obj = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var json = JsonConvert.SerializeObject(obj);

            //Act 
            var result = _jsonDiffService.DiffJson(json, json);

            //Assert
            Assert.True(result.AreEqual);
            Assert.Equal(0, result.Differences.Count);
        }

        [Fact]
        public void DiffJson_DifferentObjects_ReturnFalse()
        {
            //Arrange
            var objLeft = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var jsonLeft = JsonConvert.SerializeObject(objLeft);

            var objRight = new { prop1 = "val2", prop2 = "val2", prop3 = new { nested1 = "n2", nested2 = new { nested3 = "n3" } } };
            var jsonRight = JsonConvert.SerializeObject(objRight);


            //Act 
            var result = _jsonDiffService.DiffJson(jsonLeft, jsonRight);

            //Assert
            Assert.False(result.AreEqual);
            Assert.Equal(2, result.Differences.Count);
            Assert.Contains("prop1", result.Differences);
            Assert.Contains("prop3", result.Differences);
            Assert.DoesNotContain("prop2", result.Differences);
        }

        [Fact]
        public void DiffJson_MissingPropertyRight_ReturnFalse()
        {
            //Arrange
            var objLeft = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var jsonLeft = JsonConvert.SerializeObject(objLeft);

            var objRight = new { prop1 = "val1", prop2 = "val2" };
            var jsonRight = JsonConvert.SerializeObject(objRight);


            //Act 
            var result = _jsonDiffService.DiffJson(jsonLeft, jsonRight);

            //Assert
            Assert.False(result.AreEqual);
            Assert.Equal(1, result.Differences.Count);
            Assert.Contains("prop3", result.Differences);
            Assert.DoesNotContain("prop1", result.Differences);
            Assert.DoesNotContain("prop2", result.Differences);
        }

        public void DiffJson_MissingPropertyLeft_ReturnFalse()
        {
            //Arrange
            var objLeft = new { prop1 = "val1", prop2 = "val2" };
            var jsonLeft = JsonConvert.SerializeObject(objLeft);

            var objRight = new { prop1 = "val1", prop2 = "val2", prop3 = new { nested1 = "n1", nested2 = new { nested3 = "n3" } } };
            var jsonRight = JsonConvert.SerializeObject(objRight);


            //Act 
            var result = _jsonDiffService.DiffJson(jsonLeft, jsonRight);

            //Assert
            Assert.False(result.AreEqual);
            Assert.Equal(1, result.Differences.Count);
            Assert.Contains("prop3", result.Differences);
            Assert.DoesNotContain("prop1", result.Differences);
            Assert.DoesNotContain("prop2", result.Differences);
        }
    }
}
