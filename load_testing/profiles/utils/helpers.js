import { URLSearchParams } from 'https://jslib.k6.io/url/1.0.0/index.js'

export function loadRandomThinkTime (min = 1, max = 5) {
  return Math.random() * (max - min) + min
}
