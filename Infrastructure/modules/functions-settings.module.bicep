param functionsAppName string
param functionsAppSettings object
param prefix string
param environment string
@secure()
param h2oclockProdDbKey string
param h2oclockProdDbEndPointUri string

resource functionsStorageRef 'Microsoft.Storage/storageAccounts@2022-09-01' existing = {
  name: '${prefix}${environment}funcstorage'
}

resource appInsightsRef 'Microsoft.Insights/components@2020-02-02' existing = {
  name: '${prefix}-${environment}-appinsights'
}

resource functionsAppConfigSettings 'Microsoft.Web/sites/config@2020-12-01' = {
  name: '${functionsAppName}/appsettings'
  properties: union(functionsAppSettings,
     {
      AzureWebJobsStorage: 'DefaultEndpointsProtocol=https;AccountName=${functionsStorageRef.name};EndpointSuffix=${az.environment().suffixes.storage};AccountKey=${functionsStorageRef.listKeys().keys[0].value}'
      WEBSITE_CONTENTAZUREFILECONNECTIONSTRING: 'DefaultEndpointsProtocol=https;AccountName=${functionsStorageRef.name};EndpointSuffix=${az.environment().suffixes.storage};AccountKey=${functionsStorageRef.listKeys().keys[0].value}'
      WEBSITE_CONTENTSHARE: toLower('${prefix}-${environment}-sensor-data-function')
      APPINSIGHTS_INSTRUMENTATIONKEY: appInsightsRef.properties.InstrumentationKey
      'DB_Settings:Key': h2oclockProdDbKey
      'DB_Settings:EndPointUri': h2oclockProdDbEndPointUri
    })
}
