param location string
param environment string
param prefix string
param name string

resource appHostingPlan 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: '${prefix}-${environment}-${name}-plan'
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  properties: {}
}

output serverFarmId string = appHostingPlan.id
