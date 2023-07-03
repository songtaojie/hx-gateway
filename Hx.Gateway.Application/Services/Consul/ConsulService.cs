
using System.Web;
using Hx.Gateway.Application.Services.Consul.Dtos;

namespace Hx.Gateway.Application.Services.Consul
{
    public class ConsulService : IDynamicApiController, ITransient
    {
        private readonly IConsulHttp _consulHttp;
        private readonly SqlSugarRepository<TgSettingBak> _settingBakRep;

        public ConsulService(IConsulHttp consulHttp, SqlSugarRepository<TgSettingBak> settingBakRep)
        {
            _consulHttp = consulHttp;
            _settingBakRep = settingBakRep;
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> PostRollBackAsync(int id)
        {
            var settingBak = await _settingBakRep.Context.Queryable<TgSettingBak>().FirstAsync(o => o.Id == id);
            if (settingBak == null)
            {
                throw Oops.Bah("未找到历史备份");
            }
            var editResult = await _consulHttp.SaveConsulKeyValueAsync(settingBak.ConsulKey, settingBak.ConsulDc, settingBak.BakJson);
            if (editResult)
            {
                return "回滚成功";
            }
            else
            {
                throw Oops.Bah("回滚失败");
            }
        }


        /// <summary>
        /// 分页查询备份信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SqlSugarPagedList<SettingBakOutput>> GetPageSettingBaksAsync(PageSettingBakInput input)
        {
            var settingBaks = await _settingBakRep.Context.Queryable<TgSettingBak>().With(SqlWith.NoLock)
              .WhereIF(!string.IsNullOrWhiteSpace(input.ConsulDc), o => o.ConsulDc == input.ConsulDc)
              .WhereIF(input.BakTime.HasValue, o => o.BakTime >= input.BakTime.Value.Date && o.BakTime < input.BakTime.Value.Date.AddDays(1).AddSeconds(-1))
              .WhereIF(!string.IsNullOrWhiteSpace(input.ConsulKey), o => o.ConsulKey == input.ConsulKey)
              .OrderByDescending(o => o.Id)
              .ToPagedListAsync(input.Page, input.PageSize);
            return settingBaks.Adapt<SqlSugarPagedList<SettingBakOutput>>();
        }

        /// <summary>
        /// 获取最近的备份
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dc"></param>
        /// <returns></returns>
        public async Task<TgSettingBak> GetLastSettingBakAsync(string key, string dc)
        {
            return await _settingBakRep.Context.Queryable<TgSettingBak>().OrderByDescending(o => o.Id)
                .FirstAsync(o => o.ConsulKey == key && o.ConsulDc == dc);
        }

        /// <summary>
        /// 新增备份记录
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dc"></param>
        /// <param name="json"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public async Task<bool> InsertSettingBakAsync(string key, string dc, string json, string remark)
        {
            return await _settingBakRep.Context.Insertable(new TgSettingBak()
            {
                ConsulKey = key,
                ConsulDc = dc,
                BakJson = json,
                BakTime = DateTime.Now,
                Remark = remark
            }).ExecuteCommandAsync() > 0;
        }

        public async Task<string> GetConsulKeyValueAsync(string key, string dc)
        {
            try
            {
                key = HttpUtility.UrlDecode(key);
                var valueList = await _consulHttp.GetConsulKeyValueAsync(key, dc);
                if (valueList.Any())
                {
                    byte[] outputb = Convert.FromBase64String(valueList.FirstOrDefault().Value);
                    return Encoding.Default.GetString(outputb);
                }
                else
                {
                    throw Oops.Bah("未找到指定consul配置");
                }
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex);
            }
        }

        public async Task<bool> SaveConsulKeyValueAsync(string key, string dc, string json)
        {
            try
            {
                key = HttpUtility.UrlDecode(key);
                return await _consulHttp.SaveConsulKeyValueAsync(key, dc, json);
            }
            catch (Exception ex)
            {
                throw Oops.Oh(ex);
            }
        }
    }
}