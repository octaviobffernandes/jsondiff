# jsondiff

## Specs
.NET Core 2.0 Web API application

Nuget Dependencies
-Newtonsoft.Json
-Moq
-xUnit

## Instructions to run the application:
1. Open the project in Visual Studio 2017
2. Build Solution (F6)
3. Execute on IIS Express (F5)
4. The website will be published to IIS express and run on a custom port (52801)

## Instructions to test the application
1. Use a HTTP client application, like Postman

2. To send the first JSon object to the left endpoint, do a POST request like:
`{
	POST /api/v1/diff/left HTTP/1.1
	Host: localhost:52801
	Accept: application/json
	Content-Type: application/json
	Cache-Control: no-cache
	"eyJwcm9wMSI6ICJ2YWwxIiwgInByb3AyIjogInZhbDJhIiwgInByb3AzIjogInZhbDMifQ=="
}`
where the last line is the json object converted to base64 string, being passed on the body
If input is invalid, a BadRequest response will be returned
If it was accepted, an OK response will be returned

3. To send the second JSon object to the right endpoint, do a POST request like:
`{
	POST /api/v1/diff/right HTTP/1.1
	Host: localhost:52801
	Content-Type: application/json
	Cache-Control: no-cache
	"eyJwcm9wMSI6ICJ2YWwxIiwgInByb3AyIjogInZhbDQiLCAicHJvcDQiOiAidmFsNCJ9"
}`
where the last line is the json object converted to base64 string, being passed on the body
If input is invalid, a BadRequest response will be returned
If it was accepted, an OK response will be returned

4. Before performing the diff, you can override any of the two json values by sending new POST requests

5. To compare both objects, do a GET request like:
`{
	GET /api/v1/diff/ HTTP/1.1
	Host: localhost:52801
	Cache-Control: no-cache
}`
Response will be like:
`{
    "areEqual": false,
    "differences": [
        "prop2",
        "prop3",
        "prop4"
    ]
}`
