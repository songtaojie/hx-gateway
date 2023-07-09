
//using System.Security.Claims;
//using Hx.Gateway.Application.Services.System.Dtos;

//namespace Hx.Gateway.Application.Services.System
//{
//    public class SystemService :  ITransientDependency
//    {
//        private readonly SqlSugarRepository<TgUserAccount> _repository;
//        public SystemService(SqlSugarRepository<TgUserAccount> repository)
//        {
//            _repository = repository;
//        }

//        /// <summary>
//        /// 登录授权
//        /// </summary>
//        /// <param name="request"></param>
//        /// <returns></returns>
//        [AllowAnonymous]
//        public async Task<LoginOutput> LoginAsync(LoginInput request)
//        {
//            TgUserAccount userAccount = await LoginByUserAccountAsync(request);
//            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
//            {
//                {"userId",userAccount.Id },
//                {"fullName",userAccount.Name },
//                {"openId",userAccount.OpenId }
//            });
//            var refreshExpirdTime = int.TryParse(App.Configuration["JWTSettings:RefreshTokenExpiredTime"], out int intRefreshTokenExpiredTime) ? intRefreshTokenExpiredTime : 43200;
//            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshExpirdTime);
//            return new LoginOutput()
//            {
//                AccessToken = accessToken,
//                RefreshToken = refreshToken,
//                FullName = userAccount.Name,
//                Account = userAccount.Account,
//            };
//        }

//        /// <summary>
//        /// 使用用户名密码登录
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        private async Task<TgUserAccount> LoginByUserAccountAsync(LoginInput input)
//        {
//            var encryptPasswod = MD5Encryption.Encrypt(input.Password);
//            var userAccount = await _repository.Context.Queryable<TgUserAccount>().FirstAsync(o => o.Account == input.Account && o.Password == encryptPasswod);
//            if (userAccount == null)
//            {
//                throw Oops.Bah("用户名或密码错误");
//            }
//            if (userAccount.Status == StatusEnum.Disable)
//            {
//                throw Oops.Bah("该用户已被禁用");
//            }
//            return userAccount;
//        }

//        /// <summary>
//        /// 使用Totp登录
//        /// </summary>
//        /// <param name="input"></param>
//        /// <returns></returns>
//        private async Task<TgUserAccount> LoginByTotpAsync(LoginInput input)
//        {
//            var encryptPasswod = MD5Encryption.Encrypt(input.Password);
//            var userAccount = await _repository.Context.Queryable<TgUserAccount>()
//                .FirstAsync(u => u.Account == input.Account && u.Password == encryptPasswod);
//            if (userAccount == null)throw Oops.Bah("用户名或密码错误");
//            if (userAccount.Status == StatusEnum.Disable)throw Oops.Bah("该用户已被禁用");
//            var otp = new Totp(OtpHashAlgorithmEnum.SHA256, 6);
//            var verifyResult = otp.Verify(userAccount.Id.ToString(), input.Password, TimeSpan.FromSeconds(60));
//            if (!verifyResult)
//            {
//                throw Oops.Bah("动态密码无效");
//            }
//            return userAccount;
//        }

//        public TotpOutput GetTotp()
//        {
//            var openId = App.User?.FindFirstValue("openId");
//            if (string.IsNullOrWhiteSpace(openId))
//            {
//                throw Oops.Bah("动态密码生成失败");
//            }
//            var otp = new Totp(OtpHashAlgorithmEnum.SHA256, 6);
//            var (output, date) = otp.Compute(openId);
//            var now = DateTime.Now;
//            var countDown = (date - now).Seconds;
//            if (countDown == 0)
//            {
//                Thread.Sleep(1000);
//                var (output2, date2) = otp.Compute(openId);
//                return new TotpOutput()
//                {
//                    CountDown = (date2 - now).Seconds,
//                    TotpPwd = output2
//                };
//            }
//            return new TotpOutput()
//            {
//                CountDown = countDown,
//                TotpPwd = output
//            };
//        }

//    }
//}