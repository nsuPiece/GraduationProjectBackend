using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Mapster;
using TDengineDriver.Impl;

namespace Taos.Extensions;
public class SqlQueryExtensions<T> where T : class
{
    // 获取 T 的所有公共实例属性且不为Obsolete
    //PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    static PropertyInfo[] declaredProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
    static PropertyInfo[] propertiesToReturn = declaredProperties.Where(p => !Attribute.IsDefined(p, typeof(ObsoleteAttribute))).ToArray();

    private string _tableName;
    private List<string> _selectColumns;
    private List<Func<string>> _whereConditions;
    private List<string> _oderbyConditions;
    private string _limitConditions;

    private string sql;
    private StringBuilder query;

    public SqlQueryExtensions(string TName)
    {
        _tableName = TName;
    }

    /// <summary>
    /// 查询表中所有列
    /// </summary>
    /// <returns></returns>
    public SqlQueryExtensions<T> SelectAll()
    {
        _selectColumns.Add(" * ");
        return this;
    }

    /// <summary>
    /// 选择查询列，不同种类型不能写一起，不使用Select时默认查询T中除Obsolete外的所有公共实例属性
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> Select<TProperty>(params Expression<Func<T, TProperty>>[] columns)
    {
        if (_selectColumns == null)
        {
            _selectColumns = new List<string>();
        }
        _selectColumns.AddRange(columns.Select(c => "`" + GetPropertyName(c) + "`").ToList());
        return this;
    }

    /// <summary>
    /// 查询条件，运算符两边应为常量或变量表达式，DateTime要转换成TimeStamp进行比较
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> Where(Expression<Func<T, bool>> condition)
    {
        if (_whereConditions == null)
        {
            _whereConditions = new List<Func<string>>();
        }
        _whereConditions.Add(() => GetSqlCondition(condition));

        return this;
    }

    /// <summary>
    /// 条件查询，运算符两边应为常量或变量表达式，DateTime要转换成TimeStamp进行比较
    /// </summary>
    /// <param name="f"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> WhereIf(bool f, Expression<Func<T, bool>> condition)
    {
        if (!f) return this;
        if (_whereConditions == null)
        {
            _whereConditions = new List<Func<string>>();
        }
        _whereConditions.Add(() => GetSqlCondition(condition));

        return this;
    }

    /// <summary>
    /// 模糊查询(string)
    /// </summary>
    /// <param name="columns"></param>
    /// <param name="like"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> WhereLike(Expression<Func<T, string>> columns, string like)
    {
        if (_whereConditions == null)
        {
            _whereConditions = new List<Func<string>>();
        }
        _whereConditions.Add(() => "`" + GetPropertyName(columns) + $"` LIKE \"%{like}%\" ");
        return this;
    }

    /// <summary>
    /// 升序排序,不同种类型不能写一起
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> OrderBy<TProperty>(params Expression<Func<T, TProperty>>[] columns)
    {
        if (_oderbyConditions == null)
        {
            _oderbyConditions = new List<string>();
        }
        _oderbyConditions.AddRange(columns.Select(c => "`" + GetPropertyName(c) + "` ").ToList());
        return this;
    }

    /// <summary>
    /// 降序排序，不同种类型不能写一起
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="columns"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> OrderByDescending<TProperty>(params Expression<Func<T, TProperty>>[] columns)
    {
        if (_oderbyConditions == null)
        {
            _oderbyConditions = new List<string>();
        }
        _oderbyConditions.AddRange(columns.Select(c => "`" + GetPropertyName(c) + "` DESC ").ToList());
        return this;
    }

    /// <summary>
    /// 控制输出条数，从第x条开始往后y条
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public SqlQueryExtensions<T> LIMIT(long x, long y)
    {
        x = (x - 1) * y;
        _limitConditions = $" LIMIT {x} , {y} ";
        return this;
    }

    /// <summary>
    /// 获取sql语句
    /// </summary>
    /// <returns></returns>
    public string GetSql()
    {
        if (_selectColumns == null || _selectColumns.Count == 0)
        {
            _selectColumns = new List<string>();
            // 枚举属性名
            foreach (PropertyInfo property in propertiesToReturn)
            {
                string propertyName = property.Name;
                _selectColumns.Add("`" + propertyName + "`");
            }
        }
        query = new StringBuilder();
        query.Append($"SELECT {string.Join(", ", _selectColumns)} ");
        query.Append($"FROM `{_tableName}` ");
        if (_whereConditions != null && _whereConditions.Any())
        {
            query.Append($"WHERE {string.Join(" AND ", _whereConditions.Select(c => c()))} ");
        }
        if (_oderbyConditions != null && _oderbyConditions.Any())
        {
            query.Append($"ORDER BY {string.Join(", ", _oderbyConditions)} ");
        }
        if (_limitConditions != null && _limitConditions.Length > 0)
        {
            query.Append(_limitConditions);
        }
        return query.ToString();
    }

    /// <summary>
    /// 查询结果条数
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public long Count()
    {
        _selectColumns = new List<string> { " Count(*) " };
        if (sql == null || sql.Length == 0) sql = GetSql();
        var res = TDengineDriver.TDengine.Query(TaosExtensions.Conn, sql);
        if (TDengineDriver.TDengine.ErrorNo(res) != 0)
        {
            throw new Exception($"failed to query data since: {TDengineDriver.TDengine.Error(res)}");
        }
        var items = LibTaos.GetData(res);

        TDengineDriver.TDengine.FreeResult(res);

        return long.Parse(items.FirstOrDefault()!.ToString() ?? "0");
    }

    /// <summary>
    /// 查询一条数据
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public T ToOne()
    {
        _limitConditions = " LIMIT 0 , 1 ";
        if (sql == null || sql.Length == 0) sql = GetSql();

        var res = TDengineDriver.TDengine.Query(TaosExtensions.Conn, sql);

        if (TDengineDriver.TDengine.ErrorNo(res) != 0)
        {
            throw new Exception($"failed to query data since: {TDengineDriver.TDengine.Error(res)}");
        }

        var metas = LibTaos.GetMeta(res);
        var items = LibTaos.GetData(res);

        var dic = new Dictionary<string, object>();
        var data = new List<T>();

        for (var i = 0; i < items.Count;)
        {
            foreach (var meta in metas)
            {
                dic[meta.name] = items[i];
                i++;
            }
            data.Add(dic.Adapt<T>());
        }

        TDengineDriver.TDengine.FreeResult(res);

        return data.FirstOrDefault();
    }
    /// <summary>
    /// 异步查询一条数据
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Task<T> ToOneAsync()
    {
        _limitConditions = " LIMIT 0 , 1 ";
        if (sql == null || sql.Length == 0) sql = GetSql();

        var res = TDengineDriver.TDengine.Query(TaosExtensions.Conn, sql);

        if (TDengineDriver.TDengine.ErrorNo(res) != 0)
        {
            throw new Exception($"failed to query data since: {TDengineDriver.TDengine.Error(res)}");
        }

        var metas = LibTaos.GetMeta(res);
        var items = LibTaos.GetData(res);

        var dic = new Dictionary<string, object>();
        var data = new List<T>();

        for (var i = 0; i < items.Count;)
        {
            foreach (var meta in metas)
            {
                dic[meta.name] = items[i];
                i++;
            }
            data.Add(dic.Adapt<T>());
        }

        TDengineDriver.TDengine.FreeResult(res);

        return Task.FromResult(data.FirstOrDefault());
    }

    /// <summary>
    /// 将查询结果返回为List
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public List<T> ToList()
    {
        if (sql == null || sql.Length == 0) sql = GetSql();

        var res = TDengineDriver.TDengine.Query(TaosExtensions.Conn, sql);

        if (TDengineDriver.TDengine.ErrorNo(res) != 0)
        {
            throw new Exception($"failed to query data since: {TDengineDriver.TDengine.Error(res)}");
        }

        var metas = LibTaos.GetMeta(res);
        var items = LibTaos.GetData(res);

        var dic = new Dictionary<string, object>();
        var data = new List<T>();

        for (var i = 0; i < items.Count;)
        {
            foreach (var meta in metas)
            {
                dic[meta.name] = items[i];
                i++;
            }
            data.Add(dic.Adapt<T>());
        }

        TDengineDriver.TDengine.FreeResult(res);

        return data;
    }
    /// <summary>
    /// 异步将查询结果返回为List
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public Task<List<T>> ToListAsync()
    {
        if (sql == null || sql.Length == 0) sql = GetSql();

        var res = TDengineDriver.TDengine.Query(TaosExtensions.Conn, sql);

        if (TDengineDriver.TDengine.ErrorNo(res) != 0)
        {
            throw new Exception($"failed to query data since: {TDengineDriver.TDengine.Error(res)}");
        }

        var metas = LibTaos.GetMeta(res);
        var items = LibTaos.GetData(res);

        var dic = new Dictionary<string, object>();
        var data = new List<T>();

        for (var i = 0; i < items.Count;)
        {
            foreach (var meta in metas)
            {
                dic[meta.name] = items[i];
                i++;
            }
            data.Add(dic.Adapt<T>());
        }

        TDengineDriver.TDengine.FreeResult(res);

        return Task.FromResult(data);
    }

    private static string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var memberExpression = expression.Body as MemberExpression;
        if (memberExpression == null)
        {
            throw new ArgumentException("Expression is not a member access expression.", "expression");
        }
        var propertyInfo = memberExpression.Member as PropertyInfo;
        if (propertyInfo == null)
        {
            throw new ArgumentException("Expression is not a property access expression.", "expression");
        }
        return propertyInfo.Name;
    }

    private static string GetSqlCondition(Expression<Func<T, bool>> expression)
    {
        var binaryExpr = expression.Body as BinaryExpression;
        if (binaryExpr == null)
        {
            throw new ArgumentException("Expression is not a binary expression.");
        }

        var left = binaryExpr.Left as MemberExpression;
        if (left == null)
        {
            throw new ArgumentException("Left operand is not a member expression.");
        }

        var op = GetSqlOperator(binaryExpr.NodeType);

        var rightExpression = binaryExpr.Right as MemberExpression;
        if (rightExpression == null)
        {
            throw new ArgumentException("Right operand is not a constant or variable expression.");
        }
        var rightObject = Expression.Lambda(rightExpression).Compile().DynamicInvoke();
        var right = "";
        if (rightObject is string)
            right += "\"" + rightObject + "\"";
        else right = rightObject.ToString();

        return $" `{left.Member.Name}` {op} {right} ";
    }

    private static string GetSqlOperator(ExpressionType opType)
    {
        switch (opType)
        {
            case ExpressionType.Equal:
                return "=";
            case ExpressionType.NotEqual:
                return "<>";
            case ExpressionType.LessThan:
                return "<";
            case ExpressionType.LessThanOrEqual:
                return "<=";
            case ExpressionType.GreaterThan:
                return ">";
            case ExpressionType.GreaterThanOrEqual:
                return ">=";
            default:
                throw new ArgumentException($"Invalid operator type: {opType}");
        }
    }
}
