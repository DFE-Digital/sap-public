export const stressTestScenario = {
  executor: 'ramping-vus',
  startVUs: 400,
  stages: [
    { duration: '2m', target: 400 }, // Build up load
    { duration: '5m', target: 1500 }, // Increase to stress level
    { duration: '10m', target: 2000 }, // Maximum stress test
    { duration: '5m', target: 2000 }, // Sustain stress load
    { duration: '3m', target: 0 } // Gradual ramp down
  ],
  gracefulRampDown: '30s',
  tags: {
    service: 'load',
    scenario: 'stress',
    description: 'load stress test - 2000 concurrent users, breaking point'
  }
}
