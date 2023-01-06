{{/*
Expand the name of the chart.
*/}}
{{- define "quote-app.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Create a default fully qualified app name.
We truncate at 63 chars because some Kubernetes name fields are limited to this (by the DNS naming spec).
If release name contains chart name it will be used as a full name.
*/}}
{{- define "quote-app.fullname" -}}
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
{{- define "quote-app.chart" -}}
{{- printf "%s-%s" .Chart.Name .Chart.Version | replace "+" "_" | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "quote-app.labels" -}}
helm.sh/chart: {{ include "quote-app.chart" . }}
{{ include "quote-app.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "quote-app.selectorLabels" -}}
app.kubernetes.io/name: {{ include "quote-app.name" . }}
app.kubernetes.io/instance: {{ .Release.Name }}
{{- end }}

{{/*
Create the name of the service account to use
*/}}
{{- define "quote-app.serviceAccountName" -}}
{{- if .Values.serviceAccount.create }}
{{- default (include "quote-app.fullname" .) .Values.serviceAccount.name }}
{{- else }}
{{- default "default" .Values.serviceAccount.name }}
{{- end }}
{{- end }}

{{/*
Create ingress annotations
*/}}
{{- define "quote-app.ingressAnnotations" -}}
nginx.ingress.kubernetes.io/backend-protocol: "HTTP"
nginx.ingress.kubernetes.io/ssl-passthrough: "false"
nginx.ingress.kubernetes.io/affinity: "cookie"
{{- end }}

{{/*
Create quote-app pod annotations
*/}}
{{- define "quote-app.podAnnotations" -}}
dapr.io/enabled: "true"
dapr.io/app-ssl: "false"
dapr.io/app-id: "quote-app-id"
dapr.io/app-port: "80"
dapr.io/config: "quote-app-config"
dapr.io/log-level: "debug"
dapr.io/sidecar-liveness-probe-delay-seconds: "5"
dapr.io/sidecar-readiness-probe-delay-seconds: "3"
{{- end }}

{{/*
Certificate configMap name
*/}}
{{- define "quote-app.certConfigmap" -}}
"trusted-root-ca-cert-configmap"
{{- end }}