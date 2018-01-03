using System;
using System.Collections.Generic;
using System.Linq;
using JsonDiff.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonDiff.Services
{
    public class JsonDiffService : IDiffService
    {
        /// <summary>
        /// Validate data to check if it is a valida base64 string
        /// and on a valid json format
        /// </summary>
        /// <param name="data"></param>
        public bool ValidateInput(string data)
        {
            if (!data.IsBase64())
            {
                throw new ArgumentException("Data is not encoded in base64 format");
            }

            var decodedData = data.Base64Decode();
            if (!decodedData.IsValidJson())
            {
                throw new ArgumentException("Data is not a valid json value");
            }

            return true;
        }


        /// <summary>
        /// Perform the diff operation comparing two objects
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns>DiffResult indicating if objects are equal and pointing the differences</returns>
        public DiffResult DiffJson(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (string.IsNullOrEmpty(right))
            {
                throw new ArgumentNullException(nameof(right));
            }

            var diffResult = new DiffResult
            {
                AreEqual = true
            };

            var sourceJObject = JsonConvert.DeserializeObject<JObject>(left);
            var targetJObject = JsonConvert.DeserializeObject<JObject>(right);

            if (!JToken.DeepEquals(sourceJObject, targetJObject))
            {
                //Use left object as base to compare
                //any missing property or having different value on the right object will be added
                //to the list of differences
                foreach (var sourceProperty in sourceJObject)
                {
                    var targetProp = targetJObject.Property(sourceProperty.Key);

                    if (targetProp == null || !JToken.DeepEquals(sourceProperty.Value, targetProp.Value))
                    {
                        diffResult.AreEqual = false;
                        diffResult.Differences.Add(sourceProperty.Key);
                    }
                }

                //Check if there is any property missing on the left object
                foreach (var targetProperty in targetJObject)
                {
                    var sourceProp = sourceJObject.Property(targetProperty.Key);

                    if (sourceProp == null || !JToken.DeepEquals(targetProperty.Value, sourceProp.Value))
                    {
                        diffResult.AreEqual = false;
                        if (diffResult.Differences.All(d => d != targetProperty.Key))
                        {
                            diffResult.Differences.Add(targetProperty.Key);
                        }
                    }
                }
            }

            return diffResult;
        }
    }
}
