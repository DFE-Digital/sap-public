import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'
import { loadRandomThinkTime } from '../utils/helpers.js'

export function homepageJourney (environment, config) {
  group('profiles: Homepage', function () {
      const response = http.get(`${environment.baseUrl}`)
      const isSuccess = loadPerformanceCheck(response, 'Homepage', config.expectedResponseTimes.homepage)
      if (isSuccess) {
        loadContentCheck(response, 'main-heading', 'School Profiles')
        loadContentCheck(response, 'search-button', 'Start now')
      }

      if (!isSuccess) {
        loadErrorHandler(response, 'Homepage')
      }

      sleep(loadRandomThinkTime(1, 5))
  })
}
