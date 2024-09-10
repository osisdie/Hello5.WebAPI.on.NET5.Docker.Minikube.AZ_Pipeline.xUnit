#================================================================
# About API
#================================================================
# - Document
#   - Development         http://localhost:28731/swagger
# - HealthCheck
#   - Development         http://localhost:28731/health

#================================================================
# About minikube deployment
#================================================================
# Pre-requisite minikube installation
#   - curl -LO https://storage.googleapis.com/minikube/releases/latest/minikube-linux-amd64
#   - sudo install minikube-linux-amd64 /usr/local/bin/minikube
#   - minikube start --driver=docker
#   - minikube status

# export VERSION=$(cat src/Endpoint/Hello8/.version | head -n1)
#   Your docker image's version, ex:
#   - default: the content in the file of ./src/Endpoint/Hello8/.version
#   - or any specific such as 2.0.1

# export KUBE_DEPLOY_FILE='minikube/deployment.yaml'
#   Your deployment yaml file, ex:
#   - local folder/path
#   - or git repo/folder/path
#   - or azure devops artifacts/folder/path

#!/bin/bash
#================================================================
# Variables
#================================================================
if [ -z ${KUBE_DEPLOY_FILE+x} ]; then echo "KUBE_DEPLOY_FILE is unset" && exit 1; else echo "KUBE_DEPLOY_FILE is set to '$KUBE_DEPLOY_FILE'"; fi
if [ -z ${VERSION+x} ]; then echo "VERSION is unset" && exit 1; else echo "VERSION is set to '$VERSION'"; fi

REPO_NAME=hello8-api
KUBE_DEPLOY_FILE='minikube/deployment.yaml'
IMAGE_TAG_REGEX='[0-9]\+.[0-9]\+.[0-9]\+[.-]\(build\)\?[0-9]\+'
BUILD_VER=$(echo "$RANDOM")

for i in {\
VERSION,REPO_NAME,KUBE_DEPLOY_FILE,IMAGE_TAG_REGEX\
}; do
  echo "$i = ${!i}"
done

#================================================================
# Update deployment version
#================================================================
sed -i "s/\/${REPO_NAME}:${IMAGE_TAG_REGEX}/\/${REPO_NAME}:${VERSION}/" ${KUBE_DEPLOY_FILE}


#================================================================
# For private docker image, be sure your deployment.yaml configured sufficent credentials
#================================================================
# kubectl create secret docker-registry regcred \
#   --docker-server=https://index.docker.io/v1/ \
#   --docker-username=<your-docker-username> \
#   --docker-password=<your-docker-password> \
#   --docker-email=<your-email>

# deployment.yaml
# ---
# apiVersion: apps/v1
# kind: Deployment
# metadata:
#   name: hello8-api-dev
# spec:
#   replicas: 1
#   selector:
#     matchLabels:
#       app: hello8-api-dev
#   template:
#     metadata:
#       labels:
#         app: hello8-api-dev
#     spec:
#       containers:
#       - name: hello8-api-dev
#         image: ${IMAGE_HOST}/hello8-api:2.0.1
#       imagePullSecrets:
#       - name: regcred  # Add this line to specify the secret

#================================================================
# Start deployment
#================================================================
envsubst < minikube/deployment.yaml | kubectl apply -f -
# deployment.apps/hello8-api-dev created

kubectl apply -f minikube/service.yaml
# service/hello8-api-dev-svc created

#================================================================
# Expose endpoint
#================================================================
minikube service hello8-api-dev-svc --url
# service default/hello8-api-dev-svc has no node port
# 🏃  Starting tunnel for service hello8-api-dev-svc.
# |-----------|------------|-------------|------------------------|
# | NAMESPACE |    NAME    | TARGET PORT |          URL           |
# |-----------|------------|-------------|------------------------|
# | default   | hello8-svc |             | http://127.0.0.1:36883 |
# |-----------|------------|-------------|------------------------|
# http://127.0.0.1:36883
# ❗  Because you are using a Docker driver on linux, the terminal needs to be open to run it.
# ^C✋  Stopping tunnel for service hello8-api-dev-svc.
# 
# ❌  Exiting due to SVC_TUNNEL_STOP: stopping ssh tunnel: os: process already finished
# 
# 😿  If the above advice does not help, please let us know:
# 👉  https://github.com/kubernetes/minikube/issues/new/choose


#================================================================
# Check deployment status
#================================================================
kubectl rollout status deployment/hello8-api-dev
# deployment "hello8-api-dev" successfully rolled out


#================================================================
# Check API status with HealthCheck endpoint
#================================================================
curl http://127.0.0.1:33009/health
# Healthy


#================================================================
# Trace kubectl commands
#================================================================
kubectl get deploy -n default # or deployment, or deployments
# NAME             READY   UP-TO-DATE   AVAILABLE   AGE
# hello8-api-dev   2/2     2            2           16m

kubectl get po -n default # or pod, or pods
# NAME                              READY   STATUS    RESTARTS   AGE
# hello8-api-dev-8587dff54d-6djtd   1/1     Running   0          3m39s
# hello8-api-dev-8587dff54d-99s9r   1/1     Running   0          2m52s

kubectl get svc -n default # or service, or services
# NAME                 TYPE        CLUSTER-IP      EXTERNAL-IP   PORT(S)   AGE
# hello8-api-dev-svc   ClusterIP   10.101.34.169   <none>        80/TCP    16m
# kubernetes           ClusterIP   10.96.0.1       <none>        443/TCP   78m

kubectl get ing -n default # or ingress, or ingresses
# No resources found in default namespace

kubectl get all -n default
# NAME                                  READY   STATUS    RESTARTS   AGE
# pod/hello8-api-dev-8587dff54d-6djtd   1/1     Running   0          6m42s
# pod/hello8-api-dev-8587dff54d-99s9r   1/1     Running   0          5m55s

# NAME                         TYPE        CLUSTER-IP      EXTERNAL-IP   PORT(S)   AGE
# service/hello8-api-dev-svc   ClusterIP   10.101.34.169   <none>        80/TCP    18m
# service/kubernetes           ClusterIP   10.96.0.1       <none>        443/TCP   80m

# NAME                             READY   UP-TO-DATE   AVAILABLE   AGE
# deployment.apps/hello8-api-dev   2/2     2            2           19m

# NAME                                        DESIRED   CURRENT   READY   AGE
# replicaset.apps/hello8-api-dev-6cb4fcfbfb   0         0         0       19m
# replicaset.apps/hello8-api-dev-8587dff54d   2         2         2       6m42s
# kevinwu@HP:~$ k get all -n default
# NAME                                  READY   STATUS    RESTARTS   AGE
# pod/hello8-api-dev-8587dff54d-6djtd   1/1     Running   0          7m16s
# pod/hello8-api-dev-8587dff54d-99s9r   1/1     Running   0          6m29s

# NAME                         TYPE        CLUSTER-IP      EXTERNAL-IP   PORT(S)   AGE
# service/hello8-api-dev-svc   ClusterIP   10.101.34.169   <none>        80/TCP    19m
# service/kubernetes           ClusterIP   10.96.0.1       <none>        443/TCP   81m

# NAME                             READY   UP-TO-DATE   AVAILABLE   AGE
# deployment.apps/hello8-api-dev   2/2     2            2           19m

# NAME                                        DESIRED   CURRENT   READY   AGE
# replicaset.apps/hello8-api-dev-6cb4fcfbfb   0         0         0       19m
# replicaset.apps/hello8-api-dev-8587dff54d   2         2         2       7m16s


#================================================================
# Troubleshooting if the service is inaccessible (the real port actually exposed)
#================================================================
kubectl exec -it  pod/hello8-api-dev-8587dff54d-99s9r -- bash
# root@hello8-api-dev-8587dff54d-99s9r:/app
# root@hello8-api-dev-8587dff54d-99s9r:/app apt-get update && apt-get install -y net-tools
# root@hello8-api-dev-8587dff54d-99s9r:/app netstat -tuln
# Active Internet connections (only servers)
# Proto Recv-Q Send-Q Local Address           Foreign Address         State
# tcp6       0      0 :::8080                 :::*                    LISTEN

# After re-deployed with additional environment variable ASPNETCORE_URLS=http://+:80, then port 80 is serving
# Proto Recv-Q Send-Q Local Address           Foreign Address         State
# tcp6       0      0 :::80                   :::*                    LISTEN