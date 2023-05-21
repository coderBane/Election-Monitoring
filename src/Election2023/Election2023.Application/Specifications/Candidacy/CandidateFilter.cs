using Election2023.Application.Specifications.Base;

namespace Election2023.Application.Specifications.Candidacy;

public class CandidateFilter : Specification<Candidate>
{
    // private readonly Dictionary<string, Expression<Func<Candidate, bool>>> _expressions  = new();

    public CandidateFilter(string? searchString, bool include, int category, string? id)
    {
        // Expression<Func<Candidate, bool>> ex = e => true;

        if (include)
            Includes.Add(c => c.Party);

        if(!string.IsNullOrEmpty(id))
            Criteria = c => c.Id == id;

        if (!string.IsNullOrEmpty(searchString))
            this.And(c => c.Firstname.Contains(searchString) || c.Surname.Contains(searchString));

        if (category > 0)
            this.And(c => (int)c.Category == category);
    }
}