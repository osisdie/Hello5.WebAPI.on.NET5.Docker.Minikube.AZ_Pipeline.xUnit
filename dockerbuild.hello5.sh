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
# export VERSION=$(cat src/Endpoint/Hello5/.version | head -n1)
#   Your docker image's version, ex:
#   - default: the content in the file of ./src/Endpoint/Hello5/.version
#   - or any specific such as 1.0.0.1

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

REPO_NAME=hello5-api
VERSION=${VERSION:-1.0.0.0}
IMAGE_HOST_WITH_TAG=${IMAGE_HOST}/${REPO_NAME}:${VERSION}

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
# push image to Container Registry                            
#================================================================
docker push $IMAGE_HOST_WITH_TAG
