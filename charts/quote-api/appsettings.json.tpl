{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "Sqlserver": {
    "ServerName": "localhost",
    "DbName": "QuotesDB"
  },
  "Secrets": {
    "DbCredentialsKey": "User Id=sa;Password=Welcome@123;",
    "Authentication": {
      "ClientIdKey": "quoteapi",
      "ClientSecretKey": "apisecret"
    }
  },
  "Authentication": {
    "Authority": "https://localhost:5001/",
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
  {{- with .Values.appsettings.dapr }}
  "Dapr": {
    "statestoreName": {{ .statestoreName }},
    "secretstoreName": {{ .secretstoreName }}

  }
  {{ - end }}
}
