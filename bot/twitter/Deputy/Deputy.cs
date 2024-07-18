namespace twitter.Deputy;

public class Deputy(string name, string constituency, string[] facts, string[] refs)
{
    public string Name { get; set; } = name;
    public string Constituency { get; set; } = constituency;
    public string[] Facts { get; set; } = facts;
    public string[] Refs { get; set; } = refs;

    public List<string> SplitDeputy()
    {
        List<string> parts =
        [
            $"Je suis {Name}, {Constituency}"
        ];
        
        return parts
            .Concat(Facts)
            .Concat(Refs)
            .ToList();
    }
}