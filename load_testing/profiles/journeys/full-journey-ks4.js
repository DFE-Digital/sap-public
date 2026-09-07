import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'
import { loadRandomThinkTime } from '../utils/helpers.js'

const schoolDetails =
{
    schoolName: "Babington Academy",
    schoolUrn: "143247",
    schoolUrl: "babington-academy"
};

const pages = [
  {
    urlSlug: "/about",
    pageTitle: "About",
    checkHeading: "About the school",
  },
  {
    urlSlug: "/admissions/secondary",
    pageTitle: "Secondary Admissions",
    checkHeading: "Admissions"
  },
  {
    urlSlug: "/curriculum/secondary",
    pageTitle: "Curriculum",
    checkHeading: "Curriculum and extra-curricular activities"
  },
  {
    urlSlug: "/attendance",
    pageTitle: "Attendance",
    checkHeading: "Attendance"
  },
  {
    urlSlug: "/secondary-performance/progress-attainment/current",
    pageTitle: "Pupil Progress",
    checkHeading: "Pupil progress and attainment"
  },
  {
    urlSlug: "/secondary-performance/english-and-maths/grade-5-and-above",
    pageTitle: "English and maths results",
    checkHeading: "English and maths results for the previous academic year"
  },
  {
    urlSlug: "/secondary-performance/subjects-entered",
    pageTitle: "Subjects entered",
    checkHeading: "Subjects entered for the previous academic year"
  },
  {
    urlSlug: "/secondary-performance/additional-measures",
    pageTitle: "Additional Measures",
    checkHeading: "Additional measures"
  },
  {
    urlSlug: "/destinations/secondary",
    pageTitle: "Destinations",
    checkHeading: "Destinations of pupils who finished year 11"
  }
];


export function ks4FullJourney (environment, config) {
  group('profiles: Full Journey - KS4', function () {
      pages.forEach((page) => {

        const response = http.get(`${environment.baseUrl}/school/${schoolDetails.schoolUrn}/${schoolDetails.schoolUrl}${page.urlSlug}`)
        const isSuccess = loadPerformanceCheck(response, `${page.pageTitle}`, config.expectedResponseTimes.homepage)
        if (isSuccess) {
          loadContentCheck(response, `main-heading-${page.pageTitle}`, 'School Profiles')
          loadContentCheck(response, `page-heading-${page.pageTitle}`, `${page.checkHeading}`)
        }
        if (!isSuccess) {
          loadErrorHandler(response, `${page.pageTitle}`)
        }

        sleep(loadRandomThinkTime(1, 5))

      });
  })
}
