export const baselineScenario = {
  executor: 'ramping-vus',
  startVUs: 10,
  stages: [
    { duration: '2m', target: 50 }, // Ramp up to normal load
    { duration: '10m', target: 200 }, // Steady normal operations (load service)
    { duration: '2m', target: 50 } // Ramp down
  ],
  gracefulRampDown: '30s',
  tags: {
    service: 'load',
    scenario: 'baseline',
    description: 'Normal operations - 200 concurrent users'
  }
}
