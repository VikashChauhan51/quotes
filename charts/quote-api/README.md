
Quote-api
===========

A Helm chart for Kubernetes


## Configuration

The following table lists the configurable parameters of the Quote-api chart and their default values.

| Parameter                | Description             | Default        |
| ------------------------ | ----------------------- | -------------- |
| `global.env.ASPNETCORE_ENVIRONMENT` |  | `"Development"` |
| `replicaCount` |  | `1` |
| `image.repository` |  | `"quotesapi"` |
| `image.pullPolicy` |  | `"IfNotPresent"` |
| `image.tag` |  | `"latest"` |
| `imagePullSecrets` |  | `[]` |
| `nameOverride` |  | `""` |
| `fullnameOverride` |  | `""` |
| `issuer.kind` |  | `"Issuer"` |
| `issuer.name` |  | `"internal-cert-issuer"` |
| `serviceAccount.create` |  | `false` |
| `serviceAccount.annotations` |  | `{}` |
| `serviceAccount.name` |  | `"default"` |
| `podAnnotations` |  | `{}` |
| `podSecurityContext` |  | `{}` |
| `securityContext` |  | `{}` |
| `service.type` |  | `"ClusterIP"` |
| `service.port` |  | `80` |
| `service.targetPort` |  | `null` |
| `ingress.enabled` |  | `true` |
| `ingress.className` |  | `"nginx"` |
| `ingress.annotations` |  | `{}` |
| `ingress.hosts` |  | `[{"host": "quote-api.dev", "paths": [{"path": "/", "pathType": "Prefix"}]}]` |
| `ingress.tls` |  | `[{"secretName": "quote-api-tls", "hosts": ["quote-api.dev"]}]` |
| `resources` |  | `{}` |
| `autoscaling.enabled` |  | `false` |
| `autoscaling.minReplicas` |  | `1` |
| `autoscaling.maxReplicas` |  | `100` |
| `autoscaling.targetCPUUtilizationPercentage` |  | `80` |
| `nodeSelector` |  | `{}` |
| `tolerations` |  | `[]` |
| `affinity` |  | `{}` |
| `monitoring.daprConfigName` |  | `"quote-api-config"` |
| `monitoring.endpointAddress` |  | `"http://zipkin.monitoring.sv.cluster.local:9411/api/v2/spans"` |
| `appsettings.sqlServer.serverName` |  | `"sqlserver-mssql-latest.demo.svc.cluster.local"` |
| `appsettings.sqlServer.dbName` |  | `"QuotesDB"` |
| `appsettings.secrets.dbCredentialsKey` |  | `"db-credentials"` |
| `appsettings.secrets.authentication.clientIdKey` |  | `"clientId"` |
| `appsettings.secrets.authentication.clientSecretKey` |  | `"clientSecret"` |
| `appsettings.secrets.AuthenticationKey` |  | `"authentication"` |
| `appsettings.authentication.authority` |  | `"https://quote-identity.dev/"` |
| `appsettings.dapr.statestoreName` |  | `"state-redis"` |
| `appsettings.dapr.secretstoreName` |  | `"local-k8-secret-store"` |


