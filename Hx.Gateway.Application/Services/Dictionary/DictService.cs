using Hx.Gateway.Application.Services.Dictionary.Dtos;

namespace Hx.Gateway.Application.Services.Dictionary
{
    public class DictService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<TgDictType> _dictionaryRep;
        private readonly DictCacheService _dictionaryCacheService;
        public DictService(SqlSugarRepository<TgDictType> dictionaryRep, DictCacheService dictionaryCacheService)
        {
            _dictionaryRep = dictionaryRep;
            _dictionaryCacheService = dictionaryCacheService;
        }

        #region 查询

        /// <summary>
        /// 分页查询字典信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SqlSugarPagedList<TgDictType>> GetPageDictionaryAsync(BasePageInput input)
        {
            return await _dictionaryRep.Context.Queryable<TgDictType>()
                .OrderByDescending(o => o.Id)
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        /// <summary>
        /// 查询字典项信息
        /// </summary>
        /// <param name="dictType"></param>
        /// <returns></returns>

        public async Task<List<BaseSelectDto<string>>> GetDictItemAsync(DictTypeEnum dictType)
        {
            var dic = await _dictionaryRep.Context.Queryable<TgDictType>()
                .Includes(o => o.DictionaryItems)
                .FirstAsync(o => o.Type == dictType);
            if (dic == null)
            {
                throw Oops.Bah("该字典未配置");
            }
            return dic.DictionaryItems.Where(o => o.Status == StatusEnum.Enable)
                .Select(o => new BaseSelectDto<string>()
            {
                Label = o.Code,
                Value = o.Value
            }).ToList();
        }

        #endregion

        /// <summary>
        /// 获取所有下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<AllSelectOutput> GetAllSelectAsync()
        {
            return await _dictionaryCacheService.GetAllSelectAsync();
        }
    }
}