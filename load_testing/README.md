# School Profiles - Load Testing

Comprehensive and scalable load testing suite for School Profiles services using [k6](https://grafana.com/products/k6/).
This was "borrowed" from https://github.com/DFE-Digital/publish-teacher-training/tree/main/load_testing

## Setup

1. **Install k6:**

*macOS*
```
  brew install k6
```

*Linux (Debian/Ubuntu)*

```
  sudo apt install k6
```

## Services

#### Local/Development Runs

**Super quick local test (5 users, 10s):**
```
k6 run -e SESSION_ID=<gatewaycookieid> load-test.js
```

**Baseline, peak, and stress local:**
```

k6 run -e SESSION_ID=<gatewaycookieid> --env SCENARIO=baseline load-test.js
k6 run -e SESSION_ID=<gatewaycookieid> --env SCENARIO=peak-surge load-test.js
k6 run -e SESSION_ID=<gatewaycookieid> --env SCENARIO=stress load-test.js
k6 run -e SESSION_ID=<gatewaycookieid> --env SCENARIO=quick load-test.js
```

## Test Scenarios - TBC

### Baseline Test
- **Users**: 200 concurrent
- **Duration**: 14 minutes
- **Purpose**: Normal operations validation

### Peak Surge Test
- **Users**: 3000 concurrent at peak
- **Duration**: 15 minutes
- **Purpose**: "load opens" event (45k requests in 5 minutes)
- **Target RPS**: 150 sustained

### Stress Test
- **Users**: 4000+ concurrent
- **Duration**: 25 minutes
- **Purpose**: Breaking point identification
- **Target RPS**: 200+ sustained

***

## User Journey Mix

- User visits home page (All tests)
- User visits all KS2 profile pages (15%)
- User visits all KS4 profile pages (15%)
- User visits all KS5 profile pages (15%)
- User searches for "school", pages through 5 page, then filters on postcode, and clicks through to the about page (25%)
- User has 5 schools in their MySchools list and clicks every page to view their comparison (30%)

***

## Key Metrics - TBC

- **Response Time**: <3s for 95% of requests
- **Error Rate**: <1% during normal load
- **Throughput**: 150 RPS during peaks
- **Availability**: 99.9% uptime target

***

## Output & Monitoring

- **Local:**
  Results printed in terminal; JSON (`*-summary.json`) can be exported for deeper analysis.
- **Cloud (Grafana):**
  Real-time dashboards, historic tracking, and alerting available in Grafana Cloud (requires authentication).
