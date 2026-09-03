import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'
import { loadRandomThinkTime } from '../utils/helpers.js'

const schoolDetails =
{
    schoolName: "Abacus Belsize Primary School",
    schoolUrn: "139837",
    schoolUrl: "abacus-belsize-primary-school"
};

const pages = [
  {
    urlSlug: "/about",
    pageTitle: "About",
    checkHeading: "About the school",
  },
  {
    urlSlug: "/admissions/primary",
    pageTitle: "Primary Admissions",
    checkHeading: "Admissions"
  },
  {
    urlSlug: "/curriculum/primary",
    pageTitle: "Curriculum",
    checkHeading: "Curriculum and extra-curricular activities"
  },
  {
    urlSlug: "/attendance",
    pageTitle: "Attendance",
    checkHeading: "Attendance"
  },
  {
    urlSlug: "/primary-performance/pupil-progress",
    pageTitle: "Pupil Progress",
    checkHeading: "Pupil progress"
  },
  {
    urlSlug: "/primary-performance/meeting-or-exceeding-standards",
    pageTitle: "Meeting or Exceeding Standards",
    checkHeading: "Meeting or exceeding standards"
  },
  {
    urlSlug: "/primary-performance/subject-scaled-scores",
    pageTitle: "Subject Scaled Scores",
    checkHeading: "Subject scaled scores"
  },
  {
    urlSlug: "/primary-performance/additional-measures",
    pageTitle: "Additional Measures",
    checkHeading: "Additional measures"
  }
];


export function ks2FullJourney (environment, config) {
  group('profiles: Full Journey - KS2', function () {
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
