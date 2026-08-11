import http from 'k6/http'
import { sleep, group } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'

export function paginationJourney (environment, config) {
  group('load Pagination Journey', function () {
    const maxPages = 10

    for (let page = 1; page <= maxPages; page++) {
      const response = http.get(`${environment.baseUrl}/results?page=${page}`)

      const isSuccess = loadPerformanceCheck(
        response,
        `load Pagination Page ${page}`,
        config.expectedResponseTimes.pagination
      )

      loadContentCheck(response, 'Pagination', 'courses found')

      if (!isSuccess) {
        loadErrorHandler(response, `load Pagination Page ${page}`)
      }

      sleep(1)
    }
  })
}
