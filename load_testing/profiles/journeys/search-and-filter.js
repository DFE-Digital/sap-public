import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'

export function searchAndFilterJourney (environment, config) {
  group('search: Search and Filter Journey', function () {
    group('Basic Search', function () {

      const response = http.get(`${environment.baseUrl}/search/results?NameSearchTerm=school&Distance=3&PageNumber=1`)
      const isSuccess = loadPerformanceCheck(response, 'Basic Search', config.expectedResponseTimes.search)

      loadContentCheck(response, 'search-results', 'results for')
      loadContentCheck(response, 'has-result', 'Abacus')

      if (!isSuccess) {
        loadErrorHandler(response, 'Basic Search')
      }

      sleep(2)
    })

    group('Multi-Page Search', function () {
    const maxPages = 10

    for (let page = 1; page <= maxPages; page++) {

        const response = http.get(`${environment.baseUrl}/search/results?NameSearchTerm=school&Distance=3&pageNumber=${page}`)
        const isSuccess = loadPerformanceCheck(response, 'Multi-Page Search', config.expectedResponseTimes.search)

        loadContentCheck(response, 'search-results', 'results for')
        if (page > 1) {
          loadContentCheck(response, 'button-labels', 'Previous')
        }
        
        loadContentCheck(response, 'button-labels', 'Next')

        if (!isSuccess) {
          loadErrorHandler(response, 'Multi-Page Search Page ' + page)
        }

        sleep(2)
      }
    })

    group('Advanced Filter Search', function () {

      const response = http.get(`${environment.baseUrl}/search/results?NameSearchTerm=school&LocationSearchTerm=N1C%204PF&Distance=3&pageNumber=1`)
      const isSuccess = loadPerformanceCheck(response, 'Advanced Filter Search', config.expectedResponseTimes.search)

      loadContentCheck(response, 'sort', 'Sorted by distance')
      loadContentCheck(response, 'search-results', 'results for')
      loadContentCheck(response, 'button-labels', 'Next')
      loadContentCheck(response, 'has-result', 'Pancras')

      if (!isSuccess) {
        loadErrorHandler(response, 'Advanced Filter Search')
      }

      sleep(1)
    })
  })
}
