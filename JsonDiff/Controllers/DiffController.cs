using System;
using JsonDiff.Services;
using Microsoft.AspNetCore.Mvc;

namespace JsonDiff.Controllers
{
    [Route("api/v1/diff")]
    public class DiffController : Controller
    {
        private readonly IDiffService _diffService;
        private readonly IPersistenceService _persistenceService;

        public DiffController(IDiffService diffService, IPersistenceService persistenceService)
        {
            _diffService = diffService;
            _persistenceService = persistenceService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var leftData = _persistenceService.Get("LeftData");
            if (string.IsNullOrEmpty(leftData))
            {
                return BadRequest("Missing left data");
            }

            var rightData = _persistenceService.Get("RightData");
            if (string.IsNullOrEmpty(rightData))
            {
                return BadRequest("Missing right data");
            }

            var result = _diffService.DiffJson(leftData, rightData);
            return Ok(result);
        }

        [HttpPost("Left")]
        public IActionResult Left([FromBody]string data)
        {
            try
            {
                if (_diffService.ValidateInput(data))
                {
                    var decodedData = data.Base64Decode();
                    _persistenceService.Save("LeftData", decodedData);

                    return Ok("Left data saved successfully");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Invalid input");
        }

        [HttpPost("Right")]
        public IActionResult Right([FromBody]string data)
        {
            try
            {
                if (_diffService.ValidateInput(data))
                {
                    var decodedData = data.Base64Decode();
                    _persistenceService.Save("RightData", decodedData);

                    return Ok("Right data saved successfully");
                }
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            return BadRequest("Invalid input");
        }
    }
}
