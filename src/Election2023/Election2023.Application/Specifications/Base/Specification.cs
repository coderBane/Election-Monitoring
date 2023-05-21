using Election2023.Domain.Common;
using Election2023.Application.Extensions;

namespace Election2023.Application.Specifications.Base;

public class Specification<T> : ISpecification<T> where T : class, IEntity
{
    public Expression<Func<T, bool>> Criteria { get; set; } = c => true;

    public List<Expression<Func<T, object>>> Includes { get; } = new();

    public List<string> IncludeStrings { get; } = new();

    protected virtual void AddInclude(Expression<Func<T, object>> predicate)
        => Includes.Add(predicate);

    protected virtual void AddIncludes(string includeStrings)
        => IncludeStrings.Add(includeStrings);

    public Expression<Func<T, bool>> And(Expression<Func<T, bool>> query)
    {
        return Criteria = Criteria is null ? query : Criteria.And(query);
    }

    public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query)
    {
        return Criteria = Criteria is null ? query : Criteria.Or(query);
    }

    // public void GetProperties()
    // {
    //     Dictionary<string, Expression<Func<T, bool>>> exp = new();

    //     var type = typeof(T);
    //     PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    //     var stringProps = properties.Where(x => x.PropertyType.IsEquivalentTo(typeof(string))).ToArray();
    //     var enumProps = properties.Where(x => x.PropertyType.IsEnum).ToArray();
    //     var numericProps = properties.Where(x => x.PropertyType.IsEquivalentTo(typeof(int))).ToArray();

    //     foreach(var property in properties.Where(x => x.CanRead).ToArray())
    //     {
    //         var name = property.Name;
    //         if (!exp.ContainsKey(name))
    //         {
    //             if (property.PropertyType.IsEquivalentTo(typeof(string)))
    //             {
    //                 // exp.Add(name, T => property.GetConstantValue() as string == property.GetConstantValue() as string);
    //             }
    //             else if (property.PropertyType.IsEnum)
    //             {
    //                 // exp[name] = T => Enum.Equals(prop.GetConstantValue(), prop.GetConstantValue()); 
    //             }
    //         }
    //     }

        // var names = properties.Select(p => p.Name).ToList();
    // }
}