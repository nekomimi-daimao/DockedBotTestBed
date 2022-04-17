#!/bin/bash

cd $(dirname $0)
cd ../

hash=$(git rev-parse --short HEAD)
docker build -t "docked-bot:ex" -f Docker/Dockerfile .
