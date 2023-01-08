{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Sqlserver": {
    "ServerName": "sqlserver-mssql-latest.demo.svc.cluster.local", 
    "DbName": "QuotesDB"
  },
  "Secrets": {
    "DbCredentialsKey": "db-credentials",
    "Authentication": {
      "ClientIdKey": "clientId",
      "ClientSecretKey": "clientSecret"
    },
    "AuthenticationKey": "authentication"
  },
  "Authentication": {
    "Authority": "https://quote-identity.dev/",
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
    "statestoreName": "statestore",
    "secretstoreName": "local-k8-secret-store"

  }
}
