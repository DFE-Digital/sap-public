const loadEnvironments = {
  staging: {
    baseUrl: 'https://sap-public-test.test.teacherservices.cloud',
    name: 'staging-school-profiles',
    service: 'load'
  },
    dev: {
    baseUrl: 'https://localhost:3000',
    name: 'development-school-profiles',
    service: 'load'
  }
}

export function getloadEnvironment () {
  const env = __ENV.ENVIRONMENT || 'staging'
  return loadEnvironments[env] || loadEnvironments.staging
}

export function getloadConfig () {
  return {
    service: 'School Profiles',
    expectedResponseTimes: {
      homepage: 2000,
      search: 3000,
      courseDetails: 2000,
      pagination: 3000
    },
    thresholds: {
      http_req_duration: ['p(95)<3000'],
      http_req_failed: ['rate<0.01'],
      load_error_rate: ['rate<0.01']
    },
    cloudOptions: {
      distribution: {
        distributionLabel1: { loadZone: 'amazon:gb:london', percent: 100 }
      },
      projectID: __ENV.GRAFANA_PROJECT_ID || null,
      name: 'School Profiles Load Test'
    }
  }
}
