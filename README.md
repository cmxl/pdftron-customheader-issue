# PDFTron CustomHeader Issue

When serving PDF files with range processing enabled a HEAD request will be sent before downloading the PDF file in chunks.
But when setting `customHeaders`, the HEAD request does not send those.

## Prerequisite
* dotnet SDK 6.0.101 (https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
* nodejs latest LTS 16.13.2 (https://nodejs.org/en/download/)
* add license to app.component.ts => `INSERT_LICENCE_HERE`

## Reproducing the issue
* navigate to ./client
* execute `npm ci && npm start`
* navigate to ./server
* execute `dotnet run`
* open http://localhost:4200

=> in the devtools you'll see the `HEAD` request fail with status `401`, due to missing headers in the request.
