using ksqlDB.RestApi.Client.KSql.Linq;
using ksqlDB.RestApi.Client.KSql.Linq.PullQueries;
using ksqlDB.RestApi.Client.KSql.RestApi.Parameters;
using ksqlDB.RestApi.Client.KSql.RestApi.Statements.Inserts;
using ksqlDB.RestApi.Client.KSql.RestApi.Statements.Properties;

namespace ksqlDB.RestApi.Client.KSql.Query.Context;

public interface IKSqlDBContext : IKSqlDBStatementsContext, IAsyncDisposable, IDisposable
{
#if !NETSTANDARD
  IQbservable<TEntity> CreateQueryStream<TEntity>(string fromItemName = null);
  IAsyncEnumerable<TEntity> CreateQueryStream<TEntity>(QueryStreamParameters queryStreamParameters, CancellationToken cancellationToken = default);
#endif

  IQbservable<TEntity> CreateQuery<TEntity>(string fromItemName = null);
  IAsyncEnumerable<TEntity> CreateQuery<TEntity>(QueryParameters queryParameters, CancellationToken cancellationToken = default);

  IPullable<TEntity> CreatePullQuery<TEntity>(string tableName = null);
  ValueTask<TEntity> ExecutePullQuery<TEntity>(string ksql, CancellationToken cancellationToken = default);

  /// <summary>
  /// Add entity for insertion. In order to save them call SaveChangesAsync.
  /// </summary>
  /// <typeparam name="T">Type of entity to add.</typeparam>
  /// <param name="insertValues">Configurable insert values.</param>
  /// <param name="insertProperties">Optional insert properties.</param>
  void Add<T>(InsertValues<T> insertValues, InsertProperties insertProperties = null);

  /// <summary>
  /// Add entity for insertion. In order to save them call SaveChangesAsync.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="entity">Entity to add</param>
  /// <param name="insertProperties">Optional insert properties.</param>
  void Add<T>(T entity, InsertProperties insertProperties = null);

  /// <summary>
  /// Save the entities added to context.
  /// </summary>
  /// <returns>Save response.</returns>
  Task<HttpResponseMessage> SaveChangesAsync(CancellationToken cancellationToken = default);
}
