namespace Election2023.Application.Specifications.Base;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }

    List<Expression<Func<T, object>>> Includes { get; }

    List<string> IncludeStrings { get; }

    Expression<Func<T, bool>> And(Expression<Func<T, bool>> query);

    Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query);

    // void GetProperties();
}