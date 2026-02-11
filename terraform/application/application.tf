data "azurerm_key_vault" "app_key_vault" {
  name                = local.key_vault_name
  resource_group_name = local.resource_group_name
}

data "azurerm_key_vault_secret" "googletagmanager" {
  name         = "AnalyticsGoogleTagManager" //Name in KeyVault
  key_vault_id = data.azurerm_key_vault.app_key_vault.id
}

data "azurerm_key_vault_secret" "microsoftclarity" {
  name         = "AnalyticsMicrosoftClarity" //Name in KeyVault
  key_vault_id = data.azurerm_key_vault.app_key_vault.id
}

data "azurerm_key_vault_secret" "emailgatewaytemplate" {
  name         = "EmailGatewayTemplate" //Name in KeyVault
  key_vault_id = data.azurerm_key_vault.app_key_vault.id
}

data "azurerm_key_vault_secret" "emailgatewayapikey" {
  name         = "EmailApiKey" //Name in KeyVault
  key_vault_id = data.azurerm_key_vault.app_key_vault.id
}

data "azurerm_key_vault_secret" "gatewayenabled" {
  name         = "GatewayEnabled" //Name in KeyVault
  key_vault_id = data.azurerm_key_vault.app_key_vault.id
  content_type = "boolean"
}

module "application_configuration" {
  source = "./vendor/modules/aks//aks/application_configuration"

  namespace              = var.namespace
  environment            = var.environment
  azure_resource_prefix  = var.azure_resource_prefix
  service_short          = var.service_short
  config_short           = var.config_short
  secret_key_vault_short = "app"
  config_variables_path  = "${path.module}/config/${var.config}.yml"

  config_variables = {
    ENVIRONMENT_NAME = var.environment
  }

  secret_variables = {
    DATABASE_URL            = module.postgres.url
    StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=${module.storage.name};AccountKey=${module.storage.primary_access_key}"
	  ConnectionStrings__PostgresConnectionString = module.postgres.dotnet_connection_string
    Analytics__GoogleTagManagerId = data.azurerm_key_vault_secret.googletagmanager.value
    Email__Template = data.azurerm_key_vault_secret.emailgatewaytemplate.value,
    Email__ApiKey = data.azurerm_key_vault_secret.emailgatewayapikey.value,
    Gateway__Enabled = data.azurerm_key_vault_secret.gatewayenabled.value,
  }

}

module "web_application" {
  source = "./vendor/modules/aks//aks/application"

  is_web = true

  namespace       = var.namespace
  environment     = var.environment
  service_name    = var.service_name
  run_as_non_root = true

  cluster_configuration_map  = module.cluster_data.configuration_map
  kubernetes_config_map_name = module.application_configuration.kubernetes_config_map_name
  kubernetes_secret_name     = module.application_configuration.kubernetes_secret_name

  docker_image                     = var.docker_image
  enable_logit                     = true
  probe_path                       = var.probe_path
  send_traffic_to_maintenance_page = var.send_traffic_to_maintenance_page
}