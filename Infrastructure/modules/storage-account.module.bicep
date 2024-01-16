targetScope = 'resourceGroup'

param environment string
param location string
param prefix string
param name string

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: '${prefix}${environment}${name}storage'
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'Storage'
  properties: {
    supportsHttpsTrafficOnly: true
    defaultToOAuthAuthentication: true
  }
}

output storageAccountName string = storageAccount.name
