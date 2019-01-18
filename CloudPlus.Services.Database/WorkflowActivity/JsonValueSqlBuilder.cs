using System;
using System.Collections.Generic;
using System.Linq;

namespace CloudPlus.Services.Database.WorkflowActivity
{
    public class JsonValueSqlBuilder : IJsonValueSqlBuilder
    {
        private readonly List<JsonValueSql> _jsonValueSqlProps;
        public JsonValueSqlBuilder()
        {
            _jsonValueSqlProps = new List<JsonValueSql>();
        }

        public IJsonValueSqlBuilder Where(string key, object value)
        {
            AddProp(key, value);
            return this;
        }

        public IJsonValueSqlBuilder And(string key, object value)
        {
            AddProp(key, value);
            return this;
        }

        public IJsonValueSqlBuilder Or(string key, object value)
        {
            AddProp(key, value, JsonValueSqlConditionType.Or);
            return this;
        }

        public string Build(string expressionToQuery)
        {
            if (string.IsNullOrWhiteSpace(expressionToQuery))
                throw new ArgumentException($"{nameof(expressionToQuery)} cannot be null or empty");

            if (_jsonValueSqlProps.Count == 0)
                throw new Exception("No properties to build 'WHERE' statement");

            var jsonQuery = "WHERE";

            for (var i = 0; i < _jsonValueSqlProps.Count; i++)
            {
                var jsonValueSqlProp = _jsonValueSqlProps.ElementAt(i);

                if (i == 0)
                {
                    jsonQuery += $" {JsonValueCondition(jsonValueSqlProp.Key, jsonValueSqlProp.Value, expressionToQuery)}";
                }
                else
                {
                    jsonQuery +=
                        $" {jsonValueSqlProp.JsonValueSqlConditionType.ToString()} {JsonValueCondition(jsonValueSqlProp.Key, jsonValueSqlProp.Value, expressionToQuery)}";
                }
            }

            _jsonValueSqlProps.Clear();
            return jsonQuery;
        }

        private void AddProp(string key, object value, JsonValueSqlConditionType condition = JsonValueSqlConditionType.And)
        {
            if(string.IsNullOrWhiteSpace(key))
                throw new ArgumentException($"{nameof(key)} cannot be null or empty");

            if(value == null)
                throw new ArgumentNullException(nameof(value));

            _jsonValueSqlProps.Add(new JsonValueSql
            {
                Key = key,
                Value = value,
                JsonValueSqlConditionType = condition
            });
        }
        private static string JsonValueCondition(string key, object value, string expressionToQuery)
        {
            return $"JSON_VALUE({expressionToQuery}, '$.{key}') = '{value}'";
        }
    }

    public class JsonValueSql
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public JsonValueSqlConditionType JsonValueSqlConditionType { get; set; }

    }
    public enum JsonValueSqlConditionType
    {
        And,
        Or,
        None
    }
}