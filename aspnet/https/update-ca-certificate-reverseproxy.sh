#!/bin/sh

echo "startup script is running"

cp /app/https/localhost.crt /usr/local/share/ca-certificates
update-ca-certificates

dotnet MSA.ReverseProxy.dll
