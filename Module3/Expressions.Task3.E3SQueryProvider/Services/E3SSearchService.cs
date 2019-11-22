using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using Expressions.Task3.E3SQueryProvider.Models.Response;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Expressions.Task3.E3SQueryProvider.Services
{
    public class E3SSearchService : IE3SSearchService
    {
        #region private readonly fields

        private readonly string _baseAddress;
        private readonly HttpClient _httpClient;
        
        #endregion

        public E3SSearchService(HttpClient httpClient, string baseAddress)
        {
            _baseAddress = baseAddress ?? throw new ArgumentNullException(nameof(baseAddress));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        #region public methods

        public IEnumerable<T> SearchFTS<T>(string query, int start = 0, int limit = 0) where T : BaseE3SEntity
        {
            var requestGenerator = new FTSRequestGenerator(_baseAddress);

            Uri request = requestGenerator.GenerateRequestUrl<T>(query, start, limit);

            string resultString = _httpClient.GetStringAsync(request).Result;

            return JsonConvert.DeserializeObject<FTSResponse<T>>(resultString).Items.Select(t => t.Data);
        }
        
        public IEnumerable SearchFTS(Type type, string query, int start = 0, int limit = 0)
        {
            var requestGenerator = new FTSRequestGenerator(_baseAddress);

            Uri request = requestGenerator.GenerateRequestUrl(type, query, start, limit);

            string resultString = _httpClient.GetStringAsync(request).Result;
            Type endType = typeof(FTSResponse<>).MakeGenericType(type);
            object result = JsonConvert.DeserializeObject(resultString, endType);

            var list = Activator.CreateInstance(typeof(List<>).MakeGenericType(type)) as IList;

            foreach (object item in (IEnumerable)endType.GetProperty("items").GetValue(result))
            {
                list.Add(item.GetType().GetProperty("data").GetValue(item));
            }

            return list;
        }

        #endregion
    }
}
