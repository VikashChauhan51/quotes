{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Sqlserver": {
    "ServerName": {{ .Values.appsettings.sqlServer.serverName | quote }}, 
    "DbName": {{ .Values.appsettings.sqlServer.dbName | quote }}
  },
  "Secrets": {
    "DbCredentialsKey": {{ .Values.appsettings.secrets.dbCredentialsKey | quote }},
    "Authentication": {
      "ClientIdKey": {{ .Values.appsettings.secrets.authentication.clientIdKey | quote }},
      "ClientSecretKey": {{ .Values.appsettings.secrets.authentication.clientSecretKey | quote }}
    },
    "AuthenticationKey": {{ .Values.appsettings.secrets.AuthenticationKey | quote }}
  },
  "Authentication": {
    "Authority": {{ .Values.appsettings.authentication.authority | quote }},
    "NameClaimType": "given_name",
    "RoleClaimType": "role",
    "Scopes": [ "quoteapi.fullaccess", "quoteapi.read", "quoteapi.write" ]
  },
  "ClientApplicationCanWrite": [ "quoteapi.write" ],
  "ClientApplicationCanRead": [ "quoteapi.read" ],
  "ClientApplicationFullAccess": [ "quoteapi.fullaccess" ],
  "RateLimiting": {
    "ConcurrencyLimiter": {
      "PermitLimit": 10,
      "QueueProcessingOrder": 0,
      "QueueLimit": 2
    },
    "TokenBucketRateLimiter": {
      "TokenLimit": 10,
      "QueueProcessingOrder": 0,
      "QueueLimit": 2,
      "ReplenishmentPeriodSeconds": 60,
      "TokensPerPeriod": 10,
      "AutoReplenishment": false
    }
  },
  "Dapr": {
    "statestoreName": {{ .Values.appsettings.dapr.statestoreName | quote }},
    "secretstoreName": {{ .Values.appsettings.dapr.secretstoreName | quote }}
  },
  "ConnectionStrings": {
    "Redis": "redis-master.vik.svc.cluster.local:6379"
  }
}
