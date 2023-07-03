import debug from './env'

export default ({ mock, setup }: { mock?: boolean; setup: () => void }) => {
  if (mock !== false && debug) setup()
}

export const successResponseWrap = (data: unknown) => {
  return {
    data,
    extras: '',
    statusCode: 200,
    succeeded: true,
    timestamp: 123124124,
    errors: null
  }
}

export const failResponseWrap = (data: unknown, msg: string, code = 50000) => {
  return {
    data,
    extras: '',
    statusCode: code,
    succeeded: false,
    timestamp: 123124124,
    errors: 'fail'
  }
}
