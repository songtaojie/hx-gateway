using Hx.Core;
using Hx.Gateway.Application.Services.Dictionary.Dtos;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Hx.Gateway.Application.Services.Dictionary
{
    public class DictCacheService :  ITransient
    {
        private readonly SqlSugarRepository<TgDictType> _tgDictionaryRep;
        private readonly CacheManager _cacheManager;
        public DictCacheService(SqlSugarRepository<TgDictType> tgDictionaryRep,
            CacheManager cacheManager)
        {
            _tgDictionaryRep = tgDictionaryRep;
            _cacheManager = cacheManager;
        }

        public async Task<AllSelectOutput> GetAllSelectAsync()
        {
            var res = new AllSelectOutput();
            var selectKey = new DictTypeEnum[] 
            {
                DictTypeEnum.ConsulSettingKey,
                DictTypeEnum.ConsulDC,
            };
            var dictionaries = _cacheManager.Get<List<TgDictType>>(GatewayCacheConst.AllSelect);
            if (dictionaries == null)
            {
                dictionaries =  await _tgDictionaryRep.AsQueryable()
                        .Includes(o => o.DictionaryItems)
                        .Where(o => selectKey.Contains(o.Type))
                        .ToListAsync();
                _cacheManager.Set(GatewayCacheConst.AllSelect, dictionaries);
            }
            foreach (var dictionary in dictionaries)
            {
                switch (dictionary.Type)
                {
                    case DictTypeEnum.ConsulSettingKey:
                        res.ConsulSettingKey = dictionary.DictionaryItems.Select(o => new BaseSelectDto<string>()
                        {
                            Label = o.Code,
                            Value = o.Value
                        }).ToList();
                        ; break;
                    default:
                        res.ConsulDC = dictionary.DictionaryItems.Select(o => new BaseSelectDto<string>()
                        {
                            Label = o.Code,
                            Value = o.Value
                        }).ToList();
                        ; break;
                }
            }
            return res;
        }
    }
}
