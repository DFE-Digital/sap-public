import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'

export function homepageJourney (environment, config) {
  group('load: Homepage Journey', function () {
    group('Homepage Load', function () {
      const response = http.get(`${environment.baseUrl}/`)
      const isSuccess = loadPerformanceCheck(response, 'Homepage', config.expectedResponseTimes.homepage)

      loadContentCheck(response, 'main-heading', 'load teacher training courses')
      loadContentCheck(response, 'search-button', 'Search')

      if (!isSuccess) {
        loadErrorHandler(response, 'Homepage')
      }

      sleep(2)
    })
  })
}
