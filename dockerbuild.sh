#================================================================
# About API
#================================================================
# - Document
#   - Debug         http://localhost:18731/swagger
# - HealthCheck
#   - Debug         http://localhost:18731/health

#================================================================
# About Dockerbuild
#================================================================
# export VERSION=$(cat src/Endpoint/Hello8/.version | head -n1)
#   Your docker image's version, ex:
#   - default: the content in the file of ./src/Endpoint/Hello8/.version
#   - or any specific such as 2.0.1

# export IMAGE_HOST=docker.io/[ACCOUNT-ID]
#   Your Container Registry, ex:
#   - docker.io: docker.io/[ACCOUNT-ID]
#   - or AWS format: [ACCOUNT-ID].dkr.ecr.[REGION].amazonaws.com
#   - or GCP format: gcr.io/[PROJECT-ID]

#!/bin/bash
#================================================================
# Variables
#================================================================
if [ -z ${IMAGE_HOST+x} ]; then echo "IMAGE_HOST is unset" && exit 1; else echo "IMAGE_HOST is set to '$IMAGE_HOST'"; fi
if [ -z ${VERSION+x} ]; then echo "VERSION is unset" && exit 1; else echo "VERSION is set to '$VERSION'"; fi

REPO_NAME=hello8-api
VERSION=${VERSION:-2.0.1}
IMAGE_HOST_WITH_TAG=${IMAGE_HOST}/${REPO_NAME}:${VERSION}
BUILD_VER=$(echo "$RANDOM")

for i in {\
IMAGE_HOST,VERSION,REPO_NAME,IMAGE_HOST_WITH_TAG\
}; do
  echo "$i = ${!i}"
done

#================================================================
# docker commands
#================================================================
docker build . -t $IMAGE_HOST_WITH_TAG -f Dockerfile

#================================================================
# AWS Login
#================================================================
# aws ecr get-login --no-include-email --region us-west-2
# echo <your-password> | docker login ${IMAGE_HOST} -u AWS --password-stdin
# export AWS_PROFILE="default"
# echo $(aws ecr get-authorization-token --region us-west-2 --output text --query 'authorizationData[].authorizationToken' | base64 -d | cut -d: -f2) | docker login -u AWS $IMAGE_HOST --password-stdin

#================================================================
# (or) Docker Hub Login
#================================================================
# docker login -u [username] -p [password]
# password is suggested to use Access Token and even enable SSO, referring to https://docs.docker.com/go/access-tokens/
# Password:
# Login Succeeded

#================================================================
# push image to Container Registry
#================================================================
docker push $IMAGE_HOST_WITH_TAG

# The push refers to repository [docker.io/****/hello8-api]
# 24003397305c: Pushed
# 80f29463b0d4: Pushed
# 5f70bf18a086: Mounted from nodered/node-red
# 0756635a14f3: Pushed
# d9cbcb220d03: Pushed
# 623621fea071: Pushed
# eec44343af9f: Pushed
# 5255a12b962a: Pushed
# 787862deafcb: Pushed
# 8e2ab394fabf: Pushed
# 2.0.1: digest: sha256:e418687e0d80b65e078e0481bc802c517ac94146e80642f818ee4080c42bd547 size: 2411