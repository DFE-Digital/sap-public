import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'
import { loadRandomThinkTime } from '../utils/helpers.js'

const schoolDetails =
{
    schoolName: "Craven College",
    schoolUrn: "130591",
    schoolUrl: "craven-college"
};

const pages = [
  {
    urlSlug: "/about",
    pageTitle: "About",
    checkHeading: "About the school",
  },
  {
    urlSlug: "/16-to-19-performance/level-3-qualifications/alevel",
    pageTitle: "Level 3 - A Level",
    checkHeading: "A level"
  },
  {
    urlSlug: "/16-to-19-performance/level-3-qualifications/academic",
    pageTitle: "Level 3 - Academic",
    checkHeading: "Academic qualification"
  },
  {
    urlSlug: "/16-to-19-performance/level-3-qualifications/appliedgeneral",
    pageTitle: "Level 3 - Applied General",
    checkHeading: "Applied general qualification"
  },
  {
    urlSlug: "/16-to-19-performance/level-3-qualifications/techlevel",
    pageTitle: "Level 3 - Tech Level",
    checkHeading: "Tech level"
  },
    {
    urlSlug: "/16-to-19-performance/level-2-qualifications/techcert",
    pageTitle: "Level 2 - Tech cert",
    checkHeading: "Technical Certificate"
  },
  {
    urlSlug: "/destinations/16-to-19",
    pageTitle: "Destinations - 16 to 19",
    checkHeading: "Student destinations after 16 to 19 study"
  },
  {
    urlSlug: "/destinations/16-to-19-higher-level-study",
    pageTitle: "Destinations - Higher education",
    checkHeading: "Higher-level study"
  }
];


export function ks5FullJourney (environment, config) {
  group('profiles: Full Journey - KS5', function () {
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
