using Expressions.Task3.E3SQueryProvider.Models.Entitites;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Expressions.Task3.E3SQueryProvider.Services
{
    public interface IE3SSearchService
    {
        IEnumerable<T> SearchFTS<T>(string query, int start = 0, int limit = 0) where T : BaseE3SEntity;

        IEnumerable SearchFTS(Type type, string query, int start = 0, int limit = 0);
    }
}
