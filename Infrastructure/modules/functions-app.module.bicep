targetScope = 'resourceGroup'

param environment string
param location string
param prefix string
param name string
param serverFarmId string
param linuxFxVersion string

resource functionApp 'Microsoft.Web/sites@2021-03-01' = {
  name: '${prefix}-${environment}-${name}-function'
  kind: 'functionapp'
  location: location
  properties: {
    serverFarmId: serverFarmId
    siteConfig: {
      linuxFxVersion: linuxFxVersion
    }
}
}

output name string = functionApp.name
