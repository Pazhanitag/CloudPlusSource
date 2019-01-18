namespace CloudPlus.Services.Database.WorkflowActivity
{
    public interface IJsonValueSqlBuilder
    {
        IJsonValueSqlBuilder Where(string key, object value);
        IJsonValueSqlBuilder And(string key, object value);
        IJsonValueSqlBuilder Or(string key, object value);
        /// <summary>
        /// This also empties query that was build so the new one can be built
        /// </summary>
        /// <returns>Json query where statement</returns>
        string Build(string expressionToQuery);
    }
}