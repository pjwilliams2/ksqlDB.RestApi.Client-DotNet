﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Kafka.DotNet.ksqlDB.KSql.Query;
using Kafka.DotNet.ksqlDB.KSql.RestApi.Statements.Annotations;

namespace Kafka.DotNet.ksqlDB.Infrastructure.Extensions
{
  internal static class TypeExtensions
  {
    internal static bool IsAnonymousType(this Type type)
      => type.Name.StartsWith("<>", StringComparison.Ordinal)
         && type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), inherit: false).Length > 0
         && type.Name.Contains("AnonymousType");

    internal static Type TryFindProviderAncestor(this Type type)
    {
      while (type != null)
      {
        if (type.Name == nameof(KSet))
        {
          return type;
        }

        type = type.BaseType;
      }

      return null;
    }
    
    internal static bool IsStruct(this Type source) 
    {
      return source.IsValueType && !source.IsEnum && !source.IsPrimitive;
    }

    internal static bool IsDictionary(this Type type)
    {
      if (!type.IsGenericType)
        return false;

      var isDictionary = type.GetGenericTypeDefinition() == typeof(IDictionary<,>) || type.GetGenericTypeDefinition() == typeof(Dictionary<,>);

      return isDictionary;
    }

    internal static bool IsList(this Type type)
    {
      if (!type.IsGenericType)
        return false;

      var isList = type.GetGenericTypeDefinition() == typeof(IList<>) || type.GetGenericTypeDefinition() == typeof(List<>);

      return isList;
    }

    internal static bool HasKey(this MemberInfo typeInfo)
    {
      return typeInfo.GetCustomAttributes().OfType<KeyAttribute>().Any();
    }

    internal static IEnumerable<Type> GetEnumerableTypeDefinition(this Type type)
    {
      if (!type.IsGenericType && type == typeof(IEnumerable))
        return new[] { type };

      if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        return new[] { type };

      var enumerableTypes = type
        .GetInterfaces()
        .Where(t => t == typeof(IEnumerable) ||
                     (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)));

      return enumerableTypes;
    }
  }
}