import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'
import { extractloadCourseLinks } from '../utils/helpers.js'

export function courseDetailsJourney (environment, config) {
  group('load: Course Detail Journey', function () {
    group('Course Search to Detail', function () {
      const searchResponse = http.get(`${environment.baseUrl}/results?subjects[]=13`)
      const courseLinks = extractloadCourseLinks(searchResponse.body)

      if (courseLinks.length === 0) {
        console.error('No course links found in search results')
        return
      }

      const randomCourseLink = courseLinks[Math.floor(Math.random() * Math.min(courseLinks.length, 3))]
      sleep(1)

      const courseResponse = http.get(`${environment.baseUrl}${randomCourseLink}`)
      const isSuccess = loadPerformanceCheck(courseResponse, 'Course Detail Page', config.expectedResponseTimes.courseDetails)

      loadContentCheck(courseResponse, 'course-info', 'Course summary')
      loadContentCheck(courseResponse, 'apply-section', 'Apply for this course')
      loadContentCheck(courseResponse, 'about-course', 'About the course')

      if (!isSuccess) {
        loadErrorHandler(courseResponse, 'Course Detail Page')
      }

      sleep(3)
    })

    group('Course Apply Journey', function () {
      const searchResponse = http.get(`${environment.baseUrl}/results?subjects[]=G1`)
      const courseLinks = extractloadCourseLinks(searchResponse.body)

      if (courseLinks.length > 0) {
        const courseLink = courseLinks[0]
        const courseResponse = http.get(`${environment.baseUrl}${courseLink}`)

        const hasApplyButton = courseResponse.body.includes('Apply for this course')

        loadContentCheck(courseResponse, 'apply-button', 'Apply')

        if (hasApplyButton) {
          const isSuccess = loadPerformanceCheck(courseResponse, 'Apply Journey', config.expectedResponseTimes.courseDetails)

          if (!isSuccess) {
            loadErrorHandler(courseResponse, 'Apply Journey')
          }
        }
      }

      sleep(2)
    })
  })
}
