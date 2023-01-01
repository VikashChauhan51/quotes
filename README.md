# DotNet Core 7 Web API Sample
Sample HATEOS API with swagger OpenAPI documentation and Identity Server. 


## Pre-requisite

- Engineering laptop or desktop computer with **16-64GB** RAM and Windows 10 or later.
- System administrator rights on laptop or desktop computer.

### Install Chocolatey

#### [Chocolatey](https://chocolatey.org/install)

- For existing choco, upgrade it

    ```powershell
    # Execute on PowerShell in admin mode
        choco upgrade Chocolatey -y
    ```  

- For new installation with windows command prompt [CMD]

    ```command
      #Execute on CMD in admin mode

      @"%SystemRoot%\System32\WindowsPowerShell\v1.0\powershell.exe" -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "[System.Net.ServicePointManager]::SecurityProtocol = 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))" && SET "PATH=%PATH%;%ALLUSERSPROFILE%\chocolatey\bin"

     ```

- For new installation with PowerShell

    ```powershell
      # Execute on PowerShell in admin mode

      Set-ExecutionPolicy Bypass -Scope Process -Force; 
      [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

    ```  

### Install Dotnet 7 SDK

#### [Dotnet & SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

- For manual installation, [download](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) and install it.

- For existing dotnet sdk, upgrade it

  ```powershell
  # Execute on PowerShell in admin mode
  choco upgrade dotnet-7.0-sdk -y
  ```
  
- With Chocolatey

  ```powershell
  # Execute on PowerShell in admin mode
  choco install dotnet-7.0-sdk -y
  ```

### Install Git

#### [Git](https://git-scm.com/)

- For manual installation, [download](https://git-scm.com/download/win) and install it.

- For new installation with Chocolatey, [Reference](https://community.chocolatey.org/packages/git).

    ```powershell
    # Execute on PowerShell in admin mode
    choco install git -y
    ```

### Install WSL

#### [WSL](https://docs.microsoft.com/en-us/windows/wsl/install)

- Install WSL2 on Windows 10

    ```powershell
      # Execute on PowerShell in admin mode
      wsl --install
    ```

- Restart your computer to finish the WSL installation on Windows 10 or later.
- Set up your Linux username and password, [Reference](https://docs.microsoft.com/en-us/windows/wsl/setup/environment#set-up-your-linux-username-and-password).

- Confirm platform

    ```powershell
      # Execute on PowerShell in admin mode
        wsl --list --verbose
        or
        wsl -l -v
    ```

- Enable Windows Subsystem for Linux 2, Ignore if already set

    ```powershell
      # Execute on PowerShell in admin mode
      wsl --set-default-version 2
      or
      wsl --set-version Ubuntu 2
    ```

- [Reference](https://pureinfotech.com/install-windows-subsystem-linux-2-windows-10/) to install WSL2 on windows 10 or later.

### Install Docker Desktop

#### [Docker Desktop](https://docs.docker.com/desktop/install/windows-install/)

- For manual installation, [download](https://docs.docker.com/desktop/install/windows-install/) and install it.

- With Chocolatey, [Docker Desktop](https://community.chocolatey.org/packages/docker-desktop) and [Docker Engine](https://community.chocolatey.org/packages/docker-engine),

  ```powershell
  # Execute on PowerShell in admin mode
  choco install docker-desktop -y
  ```

- After installation,restart your system and run the Docker desktop.

- Enable Kubernetes(**Setting->Kubernetes->Enable Kubernetes**).

  > Note: After installation, keep docker desktop in running state.

### Install Kubernetes cli (kubectl)

### [kubectl](https://kubernetes.io/docs/tasks/tools/)

- For existing kubectl, upgrade it

  ```powershell
  # Execute on PowerShell in admin mode
   choco upgrade Kubernetes-cli -y 

  # Verify installation
   kubectl version --client
  ```

- For new installation.

  ```powershell
  # Execute on PowerShell in admin mode
   choco install kubernetes-cli -y

  # Verify installation
   kubectl version --client
  ```

- [Reference](https://kubernetes.io/docs/tasks/tools/install-kubectl-windows/) document for installation.

### Install Helm cli (helm)

#### [helm](https://helm.sh/)

- For new installation.

  ```powershell
  # Execute on PowerShell in admin mode
   choco install kubernetes-helm -y

  # Verify installation
   helm version
  ```

- For existing helm, upgrade it

  ```powershell
  # Execute on PowerShell in admin mode
  choco upgrade Kubernetes-helm -y

  # Verify installation
   helm version
  ```
- [Reference](https://helm.sh/docs/intro/install/) document for installation.

### Install Sql Server Management Studio(SSMS)

- Manually [download](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16) and install it.

- With Chocolatey.

  ```powershell
  # Execute on PowerShell in admin mode
   choco install ssms -y
  ```

- Start docker desktop [Ignore if it's already running].
- Start SSMS and try to connect with sql server:
    > Server: localhost,1433
    User: sa
    Password: Welcome@123

### Install Visual Studio

#### [Visual Studio](https://visualstudio.microsoft.com/downloads/)

- For manual installation, [download](https://visualstudio.microsoft.com/downloads/) and install it [**Recommended**].

- For new installation with Chocolatey, [Reference](https://community.chocolatey.org/packages/visualstudio2022community).

    ```powershell
    choco install visualstudio2022community
    # or
    choco install visualstudio2022community --package-parameters "--allWorkloads --includeRecommended --includeOptional --passive --locale en-US"
    ```

#### Install Windows Terminal

##### [Terminal](https://aka.ms/terminal)
- With Chocolatey.

  ```powershell
  # Execute on PowerShell in admin mode
   choco install microsoft-windows-terminal -y
  ```

- [Reference](https://docs.microsoft.com/en-us/windows/terminal/install) document to set up it.

#### Install PowerShell 7

##### [PowerShell7](https://docs.microsoft.com/en-us/shows/it-ops-talk/how-to-install-powershell-7)

- For manual installation, [Download](https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-on-windows?WT.mc_id=THOMASMAURER-blog-thmaure&view=powershell-7) and install it.

- With Chocolatey.

  ```powershell
  # Execute on PowerShell in admin mode
   choco install powershell-core -y
  ```
#### Install OpenSsl

##### [OpenSsl](https://www.openssl.org/)

- For manual installation, [Download](https://www.openssl.org/source/) and install it.

- With Chocolatey.

  ```powershell
  #Execute on PowerShell in admin mode
   choco install openssl -y
  ```  

#### Install Github Desktop

##### [Github Desktop](https://desktop.github.com/)

- For manual installation, [download](https://desktop.github.com/) and install it.

- For new installation with Chocolatey, [Reference](https://community.chocolatey.org/packages/github-desktop).

    ```powershell
    choco install github-desktop -y
    ```

- Configure it with your github account.
  > Note: Use any Git UI tool as per your choice.


#### Install Lens for Kubernetes

##### [Lens](https://k8slens.dev/)

- For manual installation, [download](https://k8slens.dev/) and install it.

- For new installation with Chocolatey, [Reference](https://community.chocolatey.org/packages/lens).

     ```powershell
     #Execute on PowerShell in admin mode
    choco install lens -y
    ``` 


### Install the Dapr CLI

#### [Dapr](https://dapr.io/)

- For new installation.

  ```powershell
  #Execute on PowerShell in admin mode
   Set-ExecutionPolicy RemoteSigned -scope CurrentUser;
   powershell -Command "iwr -useb https://raw.githubusercontent.com/dapr/cli/master/install/install.ps1 | iex"


  #Verify installation (run this in new terminal)
   dapr
  ```
- For existing dapr, upgrade it

  ```powershell
  #Execute on PowerShell in admin mode
   dapr upgrade
  ```

## Dapr initialization in self-hosted mode

```powershell
#Execute on PowerShell in admin mode
dapr init
# or
dapr init --slim
#Verify Dapr version
dapr --version
```
> Here, dapr self-hosted mode initialization created **dapr_redis,dapr_zipkin**. You need to delete them for forever as we will deploy these separately later on.

## Dapr initialization in local K8s

```powershell
#Execute on PowerShell in admin mode
dapr init -k
#Verify Dapr version
dapr status -k
```
## Create namespace in local k8s

```powershell
 kubectl create ns vik
```

## Add helm repos
```powershell
helm repo add bitnami https://charts.bitnami.com/bitnami 
helm repo add jetstack https://charts.jetstack.io
helm repo add kafka-ui https://provectus.github.io/kafka-ui
helm repo update
helm repo list
```
### Install MsSqlServer

- Start docker desktop [Ignore if it's already running].  

  ```powershell
  # Execute on PowerShell in admin mode
  docker pull mcr.microsoft.com/mssql/server:2019-latest
  cd C:\install

  # Git clone this:
  git clone https://github.com/microsoft/mssql-docker

  # Navigate to linux based chart.
  cd .\mssql-docker\linux\sample-helm-chart\

  # Helm install this:
  helm install sqlserver . --set sa_password=Welcome@123 --set pvc.StorageClass=hostpath -n vik
  # Installed in k8s vik namespace
  ```

## Install Kafka 
- Zookeeper

  ```powershell

  helm install zookeeper bitnami/zookeeper --set replicaCount=1 --set auth.enabled=false --set allowAnonymousLogin=true -n vik

  #ZooKeeper can be accessed via port 2181 on the following DNS name from within your cluster:
  # zookeeper.vik.svc.cluster.local

  #To connect to your ZooKeeper server from outside the cluster execute the following commands:

    kubectl port-forward --namespace vik svc/zookeeper 2181:2181 & zkCli.sh 127.0.0.1:2181

  ```
- Kafka

  ```powershell
  helm install kafka bitnami/kafka --set zookeeper.enabled=false --set replicaCount=1 --set externalZookeeper.servers=zookeeper.vik.svc.cluster.local --set externalAccess.enabled=true --set externalAccess.service.type=LoadBalancer --set externalAccess.autoDiscovery.enabled=true --set rbac.create=true --set autoCreateTopicsEnable=true --set deleteTopicEnable=true -n vik
  #Kafka can be accessed by consumers via port 9092 on the following DNS name from within your cluster:

   # kafka.vik.svc.cluster.local

  #Each Kafka broker can be accessed by producers via port 9092 on the following DNS name(s) from within your cluster:

   # kafka-0.kafka-headless.vik.svc.cluster.local:9092
   #  Kafka Brokers port: 9094
  ```
- Create Kafka topic

  ```powershell
  kubectl --namespace vik exec -it kafka-0 -- kafka-topics.sh --create --topic mytopic --replication-factor 1 --partitions 1 --bootstrap-server kafka.vik.svc.cluster.local:9092
  ```  

## Install Redis

```powershell
helm install redis bitnami/redis --set auth.enabled=false -n vik
# create load balancer
kubectl expose service redis-master -n vik --port=6379 --target-port=6379 --name=redis-external --type=LoadBalancer

```
## Install Zipkin

```powershell
# Pull Zipkin image
docker pull openzipkin/zipkin
# Create Zipkin deployment in k8s
kubectl create deployment zipkin --image openzipkin/zipkin -n vik
# Create Zipkin service
kubectl expose deployment zipkin --type ClusterIP --port 9411 -n vik
# Create Zipkin load balancer
kubectl expose service zipkin -n vik --port=9411 --target-port=9411 --name=zipkin-external --type=LoadBalancer
# http://localhost:9411/zipkin/

```
## Install Kafka UI

```powershell
# Pull Kafka-UI image
docker pull provectuslabs/kafka-ui
# Create Kafka-UI deployment in k8s
kubectl run kafka-ui --image provectuslabs/kafka-ui --env="KAFKA_CLUSTERS_0_BOOTSTRAPSERVERS=kafka-0.kafka-headless.vik.svc.cluster.local:9093" --port=8080 -n vik
# Create Kafka-UI service
kubectl expose pod kafka-ui --type ClusterIP --port 8080 -n vik
# Create Kafka-UI load balancer
kubectl expose service kafka-ui -n vik --port=8080 --target-port=8080 --name=kafka-ui-external --type=LoadBalancer
# http://localhost:8080
```

## Generate Self-signed certificate *If Expired*
```powershell
# Execute on PowerShell in admin mode
openssl req  -nodes -new -x509  -keyout ca.key -out ca.crt
```

## Import Issuer certificate to `Trusted Root Certificate Authorities` 
```powershell
# Execute on PowerShell in admin mode
Import-Certificate -FilePath C:\quotes\k8\certs\ca.crt -CertStoreLocation 'Cert:\LocalMachine\Root'
```
## Setup shared storage for Quote
```powershell
# Execute on PowerShell in admin mode
mkdir c:\mounted-k8
cd C:\quotes\k8\volume
kubectl apply -f .\local-persistent-volume.yaml -n vik

```

## Create Secret `internal-ca-key-pair` and ConfigMap for Trusted CA
```powershell
cd C:\quotes\k8\certs
kubectl create secret tls internal-ca-key-pair --cert=ca.crt --key=ca.key -n vik
kubectl apply -f .\issuer.yaml -n vik
kubectl create configmap trusted-root-ca-cert-config -- from-file=self-signed-ca.crt=ca.crt -n vik
```

## Create Secrets
```powershell
# Execute on PowerShell in admin mode
cd C:\quotes\local-k8\secrets
.\secrets.ps1
```