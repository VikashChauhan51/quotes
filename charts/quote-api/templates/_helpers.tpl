{{/*
Expand the name of the chart.
*/}}
{{- define "quote-api.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "quote-api.fullname" -}}
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
{{- define "quote-api.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "quote-api.labels" -}}
helm.sh/chart: {{ include "quote-api.chart" . }}
{{ include "quote-api.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "quote-api.selectorLabels" -}}
app.kubernetes.io/name: {{ include "quote-api.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "quote-api.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "quote-api.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Create ingress annotations
*/}}
{{- define "quote-api.ingressAnnotations" -}}
nginx.ingress.kubernetes.io/backend-protocol: "HTTP"
nginx.ingress.kubernetes.io/ssl-passthrough: "false"
nginx.ingress.kubernetes.io/affinity: "cookie"
{{- end }}

{{/*
Create quote-api pod annotations
*/}}
{{- define "quote-api.podAnnotations" -}}
dapr.io/enabled: "true"
dapr.io/app-id: "quote-api"
dapr.io/app-port: "80"
dapr.io/config: "quote-api-config"
dapr.io/sidecar-liveness-probe-delay-seconds: "100"
dapr.io/sidecar-readiness-probe-delay-seconds: "100"
{{- end }}

{{/*
Certificate configMap name
*/}}
{{- define "quote-api.certConfigmap" -}}
"trusted-root-ca-cert-configmap"
{{- end }}