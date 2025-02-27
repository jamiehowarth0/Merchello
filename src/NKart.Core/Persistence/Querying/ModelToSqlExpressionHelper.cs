﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NKart.Core.Persistence.Mappers;
using Umbraco.Core.Persistence.Querying;
using Umbraco.Core.Persistence.SqlSyntax;

namespace NKart.Core.Persistence.Querying
{
    internal class ModelToSqlExpressionHelper<T> : BaseExpressionHelper<T>
    {

        private readonly BaseMapper _mapper;

        public ModelToSqlExpressionHelper()
        {
            _mapper = MerchelloMapper.Current.ResolveByType(typeof(T)).Result;

            //MappingResolver.Current.ResolveMapperByType(typeof(T));
        }

        protected override string VisitMemberAccess(MemberExpression m)
        {
            if (m.Expression != null &&
                m.Expression.NodeType == ExpressionType.Parameter
                && m.Expression.Type == typeof(T))
            {
                var field = _mapper.Map(m.Member.Name);
                return field;
            }

            if (m.Expression != null && m.Expression.NodeType == ExpressionType.Convert)
            {
                var field = _mapper.Map(m.Member.Name);
                return field;
            }

            var member = Expression.Convert(m, typeof(object));
            var lambda = Expression.Lambda<Func<object>>(member);
            var getter = lambda.Compile();
            object o = getter();

            SqlParameters.Add(o);
            return string.Format("@{0}", SqlParameters.Count - 1);

            //return GetQuotedValue(o, o != null ? o.GetType() : null);

        }

        //protected bool IsFieldName(string quotedExp)
        //{
        //    //Not entirely sure this is reliable, but its better then simply returning true
        //    return quotedExp.LastIndexOf("'", StringComparison.InvariantCultureIgnoreCase) + 1 != quotedExp.Length;
        //}
    }
}