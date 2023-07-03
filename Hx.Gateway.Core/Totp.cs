using System;
using System.Security.Cryptography;
using System.Text;

namespace Hx.Gateway.Core
{
    public class Totp
    {
        private readonly OtpHashAlgorithmEnum _otpHashAlgorithmEnum;
        private readonly int _codeSize;

        /// <summary>
        /// timestep
        /// 30s(Recommend)
        /// </summary>
        private readonly long _timeStepTicks;

        public Totp() : this(OtpHashAlgorithmEnum.SHA1, 6) { }

        public Totp(OtpHashAlgorithmEnum otpHashAlgorithmEnum, int codeSize)
        {
            _otpHashAlgorithmEnum = otpHashAlgorithmEnum;

            if (codeSize <= 0 || codeSize > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(codeSize), codeSize, "length must between 1 and 9");
            }
            _codeSize = codeSize;
            _timeStepTicks = TimeSpan.TicksPerSecond * 30;
        }

        public Totp(OtpHashAlgorithmEnum otpHashAlgorithmEnum, int codeSize, int countDown)
        {
            _otpHashAlgorithmEnum = otpHashAlgorithmEnum;

            if (codeSize <= 0 || codeSize > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(codeSize), codeSize, "length must between 1 and 9");
            }
            _codeSize = codeSize;
            _timeStepTicks = TimeSpan.TicksPerSecond * countDown;
        }

        private static readonly Encoding Encoding = new UTF8Encoding(false, true);

        public virtual (string code, DateTime date) Compute(string securityToken) => Compute(Encoding.GetBytes(securityToken));

        public virtual (string code, DateTime date) Compute(byte[] securityToken) => Compute(securityToken, GetCurrentTimeStepNumber());

        private (string code, DateTime date) Compute(byte[] securityToken, long counter)
        {
            HMAC hmac = _otpHashAlgorithmEnum switch
            {
                OtpHashAlgorithmEnum.SHA1 => new HMACSHA1(securityToken),
                OtpHashAlgorithmEnum.SHA256 => new HMACSHA256(securityToken),
                OtpHashAlgorithmEnum.SHA512 => new HMACSHA512(securityToken),
                _ => throw new ArgumentOutOfRangeException(nameof(_otpHashAlgorithmEnum), _otpHashAlgorithmEnum, null),
            };
            var time = counter * _timeStepTicks;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddTicks(time).AddTicks(_timeStepTicks).ToLocalTime();
            using (hmac)
            {
                var stepBytes = BitConverter.GetBytes(counter);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(stepBytes); // need BigEndian
                }
                var hashResult = hmac.ComputeHash(stepBytes);

                var offset = hashResult[hashResult.Length - 1] & 0xf;
                var p = "";
                for (var i = 0; i < 4; i++)
                {
                    p += hashResult[offset + i].ToString("X2");
                }
                var num = Convert.ToInt64(p, 16) & 0x7FFFFFFF;
                return ((num % (int)Math.Pow(10, _codeSize)).ToString(), date);
            }
        }

        public virtual bool Verify(string securityToken, string code) => Verify(Encoding.GetBytes(securityToken), code);

        public virtual bool Verify(string securityToken, string code, TimeSpan timeToleration) => Verify(Encoding.GetBytes(securityToken), code, timeToleration);

        public virtual bool Verify(byte[] securityToken, string code) => Verify(securityToken, code, TimeSpan.Zero);

        public virtual bool Verify(byte[] securityToken, string code, TimeSpan timeToleration)
        {
            var futureStep = (int)(timeToleration.TotalSeconds / 30);
            var step = GetCurrentTimeStepNumber();
            for (int i = -futureStep; i <= futureStep; i++)
            {
                if (step + i < 0)
                {
                    continue;
                }
                var (totp, _) = Compute(securityToken, step + i);
                if (totp == code)
                {
                    return true;
                }
            }
            return false;
        }

        private static readonly DateTime _unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);


        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private long GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - _unixEpoch;
            return delta.Ticks / _timeStepTicks;
        }
    }
}
