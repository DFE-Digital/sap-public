import { htmlReport } from 'https://raw.githubusercontent.com/benc-uk/k6-reporter/main/dist/bundle.js'
import { textSummary } from 'https://jslib.k6.io/k6-summary/0.0.2/index.js'
import http from 'k6/http';

import { sleep, group } from 'k6'
import { getloadEnvironment, getloadConfig } from './config/environment.js'
import { homepageJourney } from './journeys/homepage.js'
import { ks2FullJourney } from './journeys/full-journey-ks2.js'
import { ks4FullJourney } from './journeys/full-journey-ks4.js'
import { ks5FullJourney } from './journeys/full-journey-ks5.js'
import { searchAndFilterJourney } from './journeys/search-and-filter.js'
import { ks4MySchools } from './journeys/myschools-ks4.js'
import { baselineScenario } from './scenarios/baseline.js'
import { peakSurgeScenario } from './scenarios/peak-surge.js'
import { stressTestScenario } from './scenarios/stress-test.js'
import { quickTestScenario } from './scenarios/quick-test.js'

function getSelectedScenario () {
  const scenario = __ENV.SCENARIO || 'quick'

  switch (scenario) {
    case 'baseline':
      return { load_baseline: baselineScenario }
    case 'peak-surge':
      return { load_peak_surge: peakSurgeScenario }
    case 'stress':
      return { load_stress: stressTestScenario }
    case 'quick':
      return { load_quick: quickTestScenario }
    default:
      return { load_baseline: baselineScenario }
  }
}

export const options = {
  scenarios: getSelectedScenario(),
  thresholds: getloadConfig().thresholds,
  cloud: {
    distribution: {
      distributionLabel1: { loadZone: 'amazon:gb:london', percent: 100 }
    }
  },
  tags: {
    service: 'load',
    testType: 'load'
  }
}

export function setup () {
  const environment = getloadEnvironment()
  const config = getloadConfig()
  console.log(`Testing ${config.service} - ${environment.name}: ${environment.baseUrl}`)
  return { environment, config }
}

export default function (data) {
  const { environment, config } = data
  const jar = http.cookieJar();

  jar.set(
      `${environment.baseUrl}`,
      'gateway',
      __ENV.SESSION_ID
  );

  jar.set(
      `${environment.baseUrl}`,
      'MySchoolsList',
      '138858%2C119052%2C137421%2C145724%2C149364%2C134283%2C145089'
  );


  // load service specific user journey distribution based on historical data
  const journeyChoice = Math.random()

  group('User Journeys', function () {
    if (journeyChoice < 0.15){
      homepageJourney(environment, config)
      ks2FullJourney(environment, config)

    } else if (journeyChoice < 0.30) {
      homepageJourney(environment, config)
      ks4FullJourney(environment, config)

    } else if (journeyChoice < 0.45) {
      homepageJourney(environment, config)
      ks5FullJourney(environment, config)

    } else if (journeyChoice < 0.70) {
      homepageJourney(environment, config)
      searchAndFilterJourney(environment, config)
    }
    else {
      homepageJourney(environment, config)
      ks4MySchools(environment, config)
    }
  })

  // Think time between actions (2-5 seconds)
  sleep(Math.random() * 3 + 2)
}

export function handleSummary (data) {
  return {
    'load-load-test-summary.json': JSON.stringify(data, null, 2),
    'load-load-test-report.html': htmlReport(data, {
      title: 'School Profiles Load Test Report'
    }),
    stdout: textSummary(data, { indent: ' ', enableColors: true })
  }
}
