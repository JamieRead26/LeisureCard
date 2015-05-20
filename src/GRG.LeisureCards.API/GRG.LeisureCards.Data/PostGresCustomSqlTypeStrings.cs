namespace GRG.LeisureCards.Data
{
    public class PostGresCustomSqlTypeStrings : CustomSqlTypeStringsBase
    {
        public PostGresCustomSqlTypeStrings()
        {
            _dictionary.Add(CustomSqlType.NText, "TEXT");
        }
    }
}
