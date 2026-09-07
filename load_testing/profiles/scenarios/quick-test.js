export const quickTestScenario = {
  executor: 'ramping-vus',
  startVUs: 1,
  stages: [
    { duration: '5s', target: 10 },
    { duration: '35s', target: 10 },
    { duration: '5s', target: 0 }
  ],
  gracefulRampDown: '5s',
  tags: {
    service: 'load',
    scenario: 'quick',
    description: 'Super quick test - 10 users, 45 seconds'
  }
}
