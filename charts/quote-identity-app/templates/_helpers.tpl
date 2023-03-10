{{/*
Expand the name of the chart.
*/}}
{{- define "quote-identity-app.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "quote-identity-app.fullname" -}}
{{- if .Values.fullnameOverride }}
{{- .Values.fullnameOverride | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- $name := default .Chart.Name .Values.nameOverride }}
{{- if contains $name .Release.Name }}
{{- .Release.Name | trunc 63 | trimSuffix "-" }}
{{- else }}
{{- printf "%s-%s" .Release.Name $name | trunc 63 | trimSuffix "-" }}
{{- end }}
{{- end }}
{{- end }}

{{/*
Create chart name and version as used by the chart label.
*/}}
{{- define "quote-identity-app.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "quote-identity-app.labels" -}}
helm.sh/chart: {{ include "quote-identity-app.chart" . }}
{{ include "quote-identity-app.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "quote-identity-app.selectorLabels" -}}
app.kubernetes.io/name: {{ include "quote-identity-app.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "quote-identity-app.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "quote-identity-app.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}


{{/*
Create ingress annotations
*/}}
{{- define "quote-identity-app.ingressAnnotations" -}}
nginx.ingress.kubernetes.io/backend-protocol: "HTTP"
nginx.ingress.kubernetes.io/ssl-passthrough: "false"
nginx.ingress.kubernetes.io/affinity: "cookie"
{{- end }}

{{/*
Create quote-identity-app pod annotations
*/}}
{{- define "quote-identity-app.podAnnotations" -}}
dapr.io/enabled: "true"
dapr.io/app-ssl: "false"
dapr.io/app-id: "quote-identity-app-id"
dapr.io/app-port: "80"
dapr.io/config: "quote-identity-config"
dapr.io/log-level: "debug"
dapr.io/sidecar-liveness-probe-delay-seconds: "5"
dapr.io/sidecar-readiness-probe-delay-seconds: "3"
{{- end }}

{{/*
Certificate configMap name
*/}}
{{- define "quote-identity-app.certConfigmap" -}}
"trusted-root-ca-cert-configmap"
{{- end }}
