#!/bin/bash

hash=$(git rev-parse --short HEAD)
echo "docked-bot:$hash"
docker build -t "docked-bot:$hash" .
