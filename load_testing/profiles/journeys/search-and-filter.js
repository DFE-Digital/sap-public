import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'
import { buildloadSearchParams } from '../utils/helpers.js'
import { getRandomSubject, getRandomLocation } from '../data/subjects.js'

export function searchAndFilterJourney (environment, config) {
  group('load: Search and Filter Journey', function () {
    group('Basic Search', function () {
      const basicSearchParams = buildloadSearchParams({
        subjects: [getRandomSubject()],
        location: getRandomLocation(),
        radius: 50
      })

      const response = http.get(`${environment.baseUrl}/results?${basicSearchParams}`)
      const isSuccess = loadPerformanceCheck(response, 'Basic Search', config.expectedResponseTimes.search)

      loadContentCheck(response, 'search-results', 'courses found')
      loadContentCheck(response, 'filter-options', 'Filters')
      loadContentCheck(response, 'course-listings', 'Age group')

      if (!isSuccess) {
        loadErrorHandler(response, 'Basic Search')
      }

      sleep(2)
    })

    group('Multi-Filter Search', function () {
      const multiFilterParams = buildloadSearchParams({
        subjects: [getRandomSubject()],
        study_types: ['part_time'],
        location: getRandomLocation(),
        radius: 25,
        order: 'course_name_ascending',
        visa_sponsorship: true
      })

      const response = http.get(`${environment.baseUrl}/results?${multiFilterParams}`)
      const isSuccess = loadPerformanceCheck(response, 'Multi-Filter Search', config.expectedResponseTimes.search)

      loadContentCheck(response, 'filter-validation', 'Part time (18 to 24 months)')
      loadContentCheck(response, 'filtered-results', 'courses found')

      if (!isSuccess) {
        loadErrorHandler(response, 'Multi-Filter Search')
      }

      sleep(1)
    })

    group('Advanced Filter Search', function () {
      const advancedParams = buildloadSearchParams({
        subjects: [getRandomSubject()],
        qualifications: ['pgce', 'pgde'],
        funding_types: ['salary', 'bursary'],
        location: getRandomLocation(),
        radius: 10
      })

      const response = http.get(`${environment.baseUrl}/results?${advancedParams}`)
      const isSuccess = loadPerformanceCheck(response, 'Advanced Filter Search', config.expectedResponseTimes.search)

      loadContentCheck(response, 'qualification-filter', 'PGCE')
      loadContentCheck(response, 'funding-filter', 'Salary')

      if (!isSuccess) {
        loadErrorHandler(response, 'Advanced Filter Search')
      }

      sleep(1)
    })
  })
}
