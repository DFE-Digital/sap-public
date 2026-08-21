import { check, group } from 'k6'
import { Trend, Rate, Counter } from 'k6/metrics'

const loadResponseTimes = new Trend('load_response_time_trend')
const loadErrorRate = new Rate('load_error_rate')
const loadErrors4xx = new Rate('load_errors_4xx')
const loadErrors5xx = new Rate('load_errors_5xx')
const loadTimeouts = new Rate('load_timeouts')
const loadRateLimited = new Rate('load_rate_limited')
const loadContentErrors = new Counter('load_content_errors')
const loadSuccessRate = new Rate('load_success_rate')

export function loadPerformanceCheck (response, name, threshold = 3000) {
  loadResponseTimes.add(response.timings.duration)

  const is4xx = response.status >= 400 && response.status < 500
  const is5xx = response.status >= 500
  const isTimeout = response.timings.duration >= threshold
  const isRateLimit = response.status === 429
  const isSuccess = response.status === 200 && response.timings.duration < threshold

  const errorType = (() => {
    if (is4xx) return '4xx'
    if (is5xx) return '5xx'
    if (isTimeout) return 'timeout'
    return 'none'
  })()

  loadErrors4xx.add(is4xx)
  loadErrors5xx.add(is5xx)
  loadTimeouts.add(isTimeout)
  loadRateLimited.add(isRateLimit)
  loadSuccessRate.add(isSuccess)

  loadErrorRate.add(!isSuccess)

  check(response, {
    [`${name}: status is 200`]: (r) => r.status === 200,
    [`${name}: response time < ${threshold}ms`]: (r) => r.timings.duration < threshold,
    [`${name}: no server errors (5xx)`]: (r) => r.status < 500,
    [`${name}: no client errors (4xx)`]: (r) => r.status < 400,
    [`${name}: not rate limited (429)`]: (r) => r.status !== 429,
    [`${name}: response size > 0`]: (r) => r.body.length > 0
  }, {
    endpoint: name,
    service: 'school-profiles',
    success: isSuccess ? 'true' : 'false',
    error_type: errorType
  })

  return isSuccess
}

export function loadContentCheck (response, checkName, expectedContent) {
  const hasContent = response.body.includes(expectedContent)

  if (!hasContent) {
    loadContentErrors.add(1, {
      check_name: checkName,
      expected_content: expectedContent.substring(0, 50),
      status: response.status
    })
  }

  return check(response, {
    [`content-check ${checkName}: contains expected content`]: (r) => r.body.includes(expectedContent),
    [`content-check ${checkName}: content length > 100 chars`]: (r) => r.body.length > 100
  }, {
    content_check: checkName,
    service: 'load'
  })
}

export function loadErrorHandler (response, context) {
  return group('load Error Analysis', function () {
    const errorDetails = {
      status: response.status,
      url: response.url,
      duration: response.timings.duration,
      size: response.body ? response.body.length : 0
    }

    if (response.status >= 400) {
      console.error(`load Error [${context}]:`, JSON.stringify(errorDetails))
    }

    return errorDetails
  })
}
