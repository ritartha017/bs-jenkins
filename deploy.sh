#! /bin/bash

whoami
# make dir if not exists
mkdir -p bs-deploy
curl -OL https://raw.githubusercontent.com/ritartha017/bs-jenkins/blob/main/docker-compose.yml > bs-deploy
curl -OL https://raw.githubusercontent.com/ritartha017/bs-jenkins/blob/main/Dockerfile > bs-deploy
