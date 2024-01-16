// =========== main.bicep ===========
targetScope = 'subscription'

param location string
param environment string
param prefix string
param sensorfunctionsAppSettings object
@secure()
param h2oclockProdDbKey string
param h2oclockProdDbEndPointUri string
param logAnalyticsWorkspaceName string

resource rg 'Microsoft.Resources/resourceGroups@2021-01-01' = {
  name: 'rg-h2oclock-test'
  location: location
}

module functionsStorage 'modules/storage-account.module.bicep' = {
  scope: rg
  name: 'functionsStorage'
  params: {
    environment: environment
    location: location
    name: 'func'
    prefix: prefix
  }
}

module appHostPlan 'modules/app-hosting-plan.module.bicep' = {
  scope: rg
  name: 'appHostPlan'
  params: {
    environment: environment
    location: location
    prefix: prefix
    name: 'sensorFunctions'
  }
}

module appInsights 'modules/app-insights.module.bicep' = {
  scope: rg
  name: 'appInsights'
  params: {
    environment: environment
    location: location
    prefix: prefix
    logAnalyticsWorkspaceName: logAnalyticsWorkspaceName
  }
}

module sensorFunctionsApp 'modules/functions-app.module.bicep' = {
  scope: rg
  name: 'sensorFunctionsApp'
  params: {
    environment: environment
    location: location
    prefix: prefix
    name: 'sensor-data'
    serverFarmId: appHostPlan.outputs.serverFarmId
  }
  dependsOn: [
    functionsStorage
  ]
}

module sensorFunctionsSettings 'modules/functions-settings.module.bicep' = {
  scope: rg
  name: 'sensorFunctionsSettings'
  params: {
    functionsAppName: sensorFunctionsApp.outputs.name
    functionsAppSettings: sensorfunctionsAppSettings
    h2oclockProdDbKey: h2oclockProdDbKey
    h2oclockProdDbEndPointUri: h2oclockProdDbEndPointUri
    prefix: prefix
    environment: environment
  }
  dependsOn: [
    functionsStorage
    appInsights
  ]
}
