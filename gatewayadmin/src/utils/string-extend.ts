export default function isJSON(str: string) {
  try {
    const obj = JSON.parse(str)
    if (typeof obj === 'object' && obj) {
      return true
    }
    return false
  } catch (e) {
    return false
  }
}
