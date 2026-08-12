import http from 'k6/http'
import { group, sleep } from 'k6'
import { loadPerformanceCheck, loadContentCheck, loadErrorHandler } from '../utils/checks.js'

const compareDetails =
{
    slug: "urns=119052&urns=138858&urns=137421&urns=145724&urns=134283",
    schoolName1: "Abbey Court Foundation",
    schoolName2: "Abbeyfield",
    schoolName3: "Accrington St Christopher",
    schoolName4: "Ada Lovelace",
    schoolName5: "Alder Community", 
};

const pages = [
  {
    urlSlug: "/compare/secondary/about-your-schools",
    pageTitle: "About",
    checkHeading: "About your schools",
  },
  {
    urlSlug: "/compare/secondary/pupil-attainment",
    pageTitle: "Attainment",
    checkHeading: "Attainment 8 for the previous academic year"
  },
  {
    urlSlug: "/compare/secondary/english-and-maths-results",
    pageTitle: "Curriculum",
    checkHeading: "Pupils achieving grades in English and maths GCSEs for the previous academic year"
  },
  {
    urlSlug: "/compare/secondary/destinations-after-year-11",
    pageTitle: "Destinations",
    checkHeading: "Destinations after year 11"
  },
  {
    urlSlug: "/compare/secondary/next-steps",
    pageTitle: "Next",
    checkHeading: "Next Steps"
  }
];


export function ks4MySchools (environment, config) {
  group('myschools: Full Journey - KS4', function () {
      pages.forEach((page) => {

        const response = http.get(`${environment.baseUrl}${page.urlSlug}?${compareDetails.slug}`)
        const isSuccess = loadPerformanceCheck(response, `${page.pageTitle}`, config.expectedResponseTimes.homepage)
        loadContentCheck(response, `main-heading-${page.pageTitle}`, 'School Profiles')
        loadContentCheck(response, `page-heading-${page.pageTitle}`, `${page.checkHeading}`)

        loadContentCheck(response, `${page.pageTitle}-contains-school-1`, `${compareDetails.schoolName1}`)
        loadContentCheck(response, `${page.pageTitle}-contains-school-2`, `${compareDetails.schoolName2}`)
        loadContentCheck(response, `${page.pageTitle}-contains-school-3`, `${compareDetails.schoolName3}`)
        loadContentCheck(response, `${page.pageTitle}-contains-school-4`, `${compareDetails.schoolName4}`)
        loadContentCheck(response, `${page.pageTitle}-contains-school-5`, `${compareDetails.schoolName5}`)

        if (!isSuccess) {
          loadErrorHandler(response, `${page.pageTitle}`)
        }

        sleep(2)

      });
  })
}
